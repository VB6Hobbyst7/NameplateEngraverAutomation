Imports System.IO
Imports System.Runtime.InteropServices

Public Module AutocadPrep
    'EPM 8/1/2019
    'Creates VR plate drawings
    Public Sub CreateDrawing(ByVal SerialNum As String)

        Dim blankPlate As String, drawingNum As String, mfrDate As String, PerSO_Info As String, PerSN_Info As String

        Dim dalinst As New DALControl
        dalinst.RunQuery("SELECT * FROM SFM_RCH.NP_QUEUE_NEW_API WHERE SN = '" & SerialNum & "' AND ACTIUAL_DATE IS null ORDER BY REQUEST_DATE DESC")
        blankPlate = "P3R" & dalinst.SQLDataset01.Tables(0).Rows(0).Item(2)
        drawingNum = "P3R" & dalinst.SQLDataset01.Tables(0).Rows(0).Item(1)
        mfrDate = String.Format("{0:MM/yyyy}", dalinst.SQLDataset01.Tables(0).Rows(0).Item(3))
        Try
            PerSO_Info = dalinst.SQLDataset01.Tables(0).Rows(0).Item(7)
        Catch ex As Exception
            PerSO_Info = Nothing
        End Try
        Try
            PerSN_Info = dalinst.SQLDataset01.Tables(0).Rows(0).Item(8)
        Catch ex As Exception
            PerSN_Info = Nothing
        End Try

        System.IO.File.WriteAllText("I:\Shared\CroppedNameplates2\RegulatorTemplates\VR_NameplateInfo.txt", "")

        Using sw As StreamWriter = New StreamWriter("I:\Shared\CroppedNameplates2\RegulatorTemplates\VR_NameplateInfo.txt")

            sw.WriteLine(SerialNum)
            sw.WriteLine(blankPlate)
            sw.WriteLine(drawingNum)
            sw.WriteLine(mfrDate)

            'CDL - 2/22/19
            'Changed the if block and added the PerSN and PerSO variable clears
            If PerSO_Info IsNot Nothing And PerSN_Info IsNot Nothing Then
                sw.WriteLine(PerSO_Info & ";" & PerSN_Info)
            ElseIf PerSO_Info IsNot Nothing Then
                sw.WriteLine(PerSO_Info)
            ElseIf PerSN_Info IsNot Nothing Then
                sw.WriteLine(PerSN_Info)
            End If

        End Using

        drawingNum = drawingNum.Split("R")(1) & ".dwg"

        CloseAcadDWGs()
        StartAutolisp(drawingNum)
        CloseAcadDWGs()

        LoadingForm.MaximizeProcess("EAV1")

    End Sub
    ' EPM 8/1/2019

    ' EPM 8/1/2019
    Public Sub StartAutolisp(ByVal drawingNum As String)
        Dim FileInfo As FileInfo = New FileInfo(copiedVRDWG & "\" & drawingNum)

        While IsFileOpen(FileInfo) = True
            ErrorLog(copiedVRDWG & "\" & drawingNum & " is currently open. Force closed using AcadCloseDWGs().")
        End While

        While CheckIfRunning("acad") = False
            MsgBox("Please start autocad. In future, tell command batch to start autocad, make acad.lsp to write to textfile saying it's done, and put a sleep while the file is not equal to done ")
        End While

        'If File.Exists(acadLspPath & "acaddoc_NOTRUN_KEYENCE.lsp") Then
        'My.Computer.FileSystem.RenameFile(acadLspPath & "acaddoc_NOTRUN_KEYENCE.lsp", "acaddoc.lsp")
        'End If

        'If CheckIfRunning("acad") = True Then
        dwg2dxfbat = VrTemplatesPath & drawingNum
        'Else
        'dwg2dxfBat = "start " & acadPath & " " & csvPath & dwg '& "/nologo"
        'dwg2dxfbat = VrTemplatesPath & drawingNum '& "/nologo"
        'End If

        'Write the batch file to open and run AutoCAD
        Using sw As StreamWriter = New StreamWriter(batPath)
            Dim copyCmd As String = "copy " & dwg2dxfbat & " " & copiedVRDWG
            sw.WriteLine(copyCmd)
            sw.WriteLine(copiedVRDWG & "\" & drawingNum)
        End Using

        'Run the batch file
        Process.Start(batPath)

        'Waits for the drawing to open
        'While IsFileOpen(FileInfo) = False
        'Threading.Thread.Sleep(1000)
        'End While

        Dim EndTime As DateTime = DateTime.Now.AddSeconds(30)
        Dim counter2 = 0
        Try
            System.IO.File.WriteAllText("C:\Program Files\Autodesk\AutoCAD 2020\Support\ScriptComplete.txt", "0")
            'Waits for the drawing to close
            Dim read As String = ""
            While read.Contains("1") = False
                Using sw As StreamReader = New StreamReader("C:\Program Files\Autodesk\AutoCAD 2020\Support\ScriptComplete.txt")
                    read = sw.ReadLine()
                End Using
                If counter2 > 2 Then
                    ErrorLog("Too many batch file reattempts: ")
                    Exit Sub
                End If
                If DateTime.Now >= EndTime Then
                    Process.Start(batPath)
                    counter2 += 1
                    EndTime = DateTime.Now.AddSeconds(30)
                End If
                Threading.Thread.Sleep(1000)
            End While
            System.IO.File.WriteAllText("C:\Program Files\Autodesk\AutoCAD 2020\Support\ScriptComplete.txt", "0")
        Catch ex As Exception
            ErrorLog("Stream reading error @ " & DateTime.Now.ToString() & Environment.NewLine & ex.Message)
        End Try
        'My.Computer.FileSystem.RenameFile(acadLspPath & "acaddoc.lsp", "acaddoc_NOTRUN_KEYENCE.lsp")
    End Sub
    ' EPM 8/1/2019

    ' EPM 8/1/2019
    Public Function CheckIfRunning(ByVal sProcess As String) As Boolean
        Dim p() As Process = Process.GetProcessesByName(sProcess)
        If p.Count > 0 Then
            CheckIfRunning = True
        Else
            CheckIfRunning = False
        End If
    End Function
    ' EPM 8/1/2019

    ' EPM 8/1/2019
    Private Function IsFileOpen(ByVal file As FileInfo) As Boolean
        Dim stream As FileStream = Nothing
        Try
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None)
            stream.Close()
            IsFileOpen = False
        Catch ex As Exception
            If TypeOf ex Is IOException AndAlso IsFileLocked(ex) Then
                IsFileOpen = True
            End If
        End Try
    End Function
    ' EPM 8/1/2019

    ' EPM 8/1/2019
    Private Function IsFileLocked(exception As Exception) As Boolean
        Dim errorCode As Integer = Marshal.GetHRForException(exception) And ((1 << 16) - 1)
        Return errorCode = 32 OrElse errorCode = 33
    End Function
    ' EPM 8/1/2019

    'EMK 10/2021
    'purpose: Forces all AutoCAD drawings to close but allowing AutoCAD application to remain open
    Public Sub CloseAcadDWGs()
        Try
            Dim acadApp As Object = Marshal.GetActiveObject("AutoCAD.Application")
            Dim acadDocs = acadApp.Documents
            For Each doc In acadDocs
                doc.Quit()
            Next
        Catch ex As Exception
            'Nothing to close or not AutoCAD application to grab...
        End Try
    End Sub
End Module
