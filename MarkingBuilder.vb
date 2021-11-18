Imports System.IO

'Marking Processes

Public Module MarkingBuilder

    Public bError As Boolean = False
    Dim AxMBActX1 As AxMB3Lib4.AxMBActX = LoadingForm.AxMBActX1
    Dim markingUnit As MB3Lib4.MarkingUnitTypes = MB3Lib4.MarkingUnitTypes.MARKINGUNIT_MDX1520
    Dim sCroppedNameplatesPath As String = "I:\Shared\CroppedNameplates2\"
    Dim sHatchFilePath As String = "C:\UserData\z003vant\Documents\Marking Builder 3\Logo\"

    'EPM 7/31/2019
    'Main Sequence Program
    Public Sub UploadToEngraver(ByVal SerialNum As String, ByVal DrawingNum As String, ByVal PartNum As String)

        Dim iProgramNo As Integer
        Dim sFileNameDXF As String = SerialNum.Split("-")(0) & "\(" & SerialNum.Split("-")(1) & ")" & DrawingNum & "_" & PartNum
        Dim sFileDXF As String = sCroppedNameplatesPath & sFileNameDXF & ".dxf"
        Dim sFileNameMHL As String = SerialNum.Split("-")(0) & "_(" & SerialNum.Split("-")(1) & ")" & DrawingNum & "_" & PartNum
        Dim sFileHatch As String = sHatchFilePath & sFileNameMHL & ".mhl"

        iProgramNo = GetProgramNo(PartNum)

        ConvertToHatch(sFileDXF, sFileHatch)

        If IsConnected() = False Then
            InitializeEngraver(True)
        End If

        ClearErrors()

        If currentProgram() = iProgramNo Then 'IF THE ACTIVE PROGRAM IS THE PROGRAM THAT NEEDS TO BE EDITTED, SWITCH TO PROGRAM 1
            SwitchProgram(1)
        End If

        SendFile(iProgramNo, sFileNameMHL)

        ReceiveProgramFromController("ActiveProgram.MX4", iProgramNo)
        ViewProgram("ActiveProgram.MX4")
        SendProgramToController("ActiveProgram.MX4", 1)

        If currentProgram() <> iProgramNo Then 'IF THE ACTIVE PROGRAM IS NOT THE PROGRAM THAT WAS JUST EDITED, SWITCH THE ACTIVE TO THE PROPER PROGRAM NUMBER
            SwitchProgram(iProgramNo)
        End If
        If bError = False Then
            StartMarking(SerialNum, DrawingNum, PartNum)
        End If

    End Sub
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Returns the currently active program number
    Private Function GetProgramNo(ByVal PartNum As String) As Integer

        Dim programQuery As String = "SELECT PROGRAM_NUMBER FROM SFM_RCH.Y_PROGRAM_ASSIGNMENTS WHERE PART_NUMBER = '" & PartNum & "'"
        Dim dalSelectProgram As New DALControl
        dalSelectProgram.RunQuery(programQuery)
        GetProgramNo = CInt(dalSelectProgram.SQLDataset01.Tables(0).Rows(0).Item(0))

    End Function
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Converts file from .dxf to .mhl
    Private Sub ConvertToHatch(ByVal sFileDXF As String, ByVal sFileHatch As String)

        Try
            AxMBActX1.Dxf.Convert(sFileDXF, sFileHatch, True)
        Catch ex As Exception
            ErrorLog("ConverToHatch Error: " & ex.Message & DateTime.Now.ToString())
            bError = True
        End Try

    End Sub
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Start Marking Command
    Private Sub StartMarking(ByVal SerialNum As String, ByVal DrawingNum As String, ByVal PartNum As String)


        Try
            AxMBActX1.Operation.StartMarking()
        Catch ex As Exception
            ErrorLog("StartMarking Error: " & ex.Message & DateTime.Now.ToString())
            bError = True
        End Try
    End Sub
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Shows drawing on MainUserForm
    Private Sub ViewProgram(ByVal sProgram As String)

        Try
            AxMBActX1.OpenProgram(sProgram)
            AxMBActX1.ZoomAdjust()
        Catch ex As Exception
            ErrorLog("ViewProgram Error: " & ex.Message & DateTime.Now.ToString())
            bError = True
        End Try

    End Sub
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Receives program from Controller to PC
    Private Sub ReceiveProgramFromController(ByVal sFileName As String, ByVal iProgramNo As Integer)

        Try
            AxMBActX1.ReceiveControllerProgram(sFileName, iProgramNo)
        Catch ex As Exception
            ErrorLog("ReceiveProgramFromController Error: " & ex.Message & DateTime.Now.ToString())
            bError = True
        End Try

    End Sub
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Sends program from PC to Controller
    Private Sub SendProgramToController(ByVal sFileName As String, ByVal iProgramNo As Integer)

        Try
            AxMBActX1.SendControllerProgram(iProgramNo, sFileName)
        Catch ex As Exception
            ErrorLog("SendProgramToController Error: " & ex.Message & DateTime.Now.ToString())
            bError = True
        End Try

    End Sub
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Returns whether the engraver is connected
    Public Function IsConnected() As Boolean

        IsConnected = False
        Try
            IsConnected = Not AxMBActX1.Comm.Offline()
        Catch ex As Exception
            ErrorLog("IsConnected Error: " & ex.Message & DateTime.Now.ToString())
            bError = True
        End Try

    End Function
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Sends logo .mhl file and QR string to template
    Private Sub SendFile(ByVal iProgramNumber As Integer, ByVal sFile As String)

        Dim sProgramNumber As String = iProgramNumber.ToString
        Dim sCommand As String
        Dim sReceived As String
        Dim sQRInfo As String
        Dim sBCInfo As String

        sCommand = "WX,PRG=" & sProgramNumber & ",BLK=000,CharacterString=%T<" & sFile & ">"

        Try
            sReceived = AxMBActX1.Comm.SendAndReceive(sCommand)
            If sReceived.Split(",")(1) <> "OK" Then
                ErrorLog("1SendFile Not OK Error: " & sReceived)
            End If
        Catch ex As Exception
            ErrorLog("1SendFile Error: " & ex.Message & DateTime.Now.ToString())
            bError = True
        End Try


        If sFile.Contains("QR") Then

            sQRInfo = getQRInfo(sFile)

            If sQRInfo <> Nothing Then

                sCommand = "WX,PRG=" & sProgramNumber & ",BLK=001,CharacterString=" & sQRInfo
                Try
                    sReceived = AxMBActX1.Comm.SendAndReceive(sCommand)
                    If sReceived.Split(",")(1) <> "OK" Then
                        ErrorLog("QR Not OK Error: " & sReceived)
                    End If
                Catch ex As Exception
                    ErrorLog("QR Error: " & ex.Message & DateTime.Now.ToString())
                    bError = True
                End Try
            End If
        End If

        If sFile.Contains("BC") Then

            sBCInfo = GetBarcode()
            File.Delete("C:\Temp\Finished.txt")

            If sBCInfo.Contains(";") Then                 'If there are two BCs
                Dim sBC As String() = sBCInfo.Split(";")
                Dim iCount As Integer = 1
                While iCount <= sBC.Count
                    sCommand = "WX,PRG=" & sProgramNumber & ",BLK=00" & iCount.ToString & ",CharacterString=" & sBC(iCount - 1)
                    Try
                        sReceived = AxMBActX1.Comm.SendAndReceive(sCommand)
                        If sReceived.Split(",")(1) <> "OK" Then
                            ErrorLog("2SendFile Not OK Error: " & sReceived & DateTime.Now.ToString())
                        End If
                    Catch ex As Exception
                        ErrorLog("2SendFile Error: " & ex.Message & DateTime.Now.ToString())
                        bError = True
                    End Try
                    iCount += 1
                End While
            Else                                            'If there is only one BC
                sCommand = "WX,PRG=" & sProgramNumber & ",BLK=001,CharacterString=" & sBCInfo
                Try
                    sReceived = AxMBActX1.Comm.SendAndReceive(sCommand)
                    If sReceived.Split(",")(1) <> "OK" Then
                        ErrorLog("3SendFile Not OK Error:" & sReceived & DateTime.Now.ToString())
                    End If
                Catch ex As Exception
                    ErrorLog("3SendFile Error: " & ex.Message & DateTime.Now.ToString())
                    bError = True
                End Try
            End If
        End If

    End Sub
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Initializes engraver
    Private Sub InitializeEngraver(ByVal bEdit As Boolean)
        Try
            AxMBActX1.InitMBActX(markingUnit)
            AxMBActX1.IsAutoRedraw = True
            AxMBActX1.IsBlockingCommunication = True
            AxMBActX1.Comm.ConnectionType = MB3Lib4.ConnectionTypes.CONNECTION_USB
            AxMBActX1.Comm.Online()
            If bEdit = True Then
                AxMBActX1.Context = MB3Lib4.ContextTypes.CONTEXT_EDITING
            Else
                AxMBActX1.Context = MB3Lib4.ContextTypes.CONTEXT_CONTROLLER
            End If
        Catch ex As Exception
            ErrorLog("InitializeEngraver Error: " & ex.Message & DateTime.Now.ToString())
            bError = True
        End Try

    End Sub
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Returns the current program number
    Private Function currentProgram() As Integer

        Dim iCurrentProgram As Integer
        Dim sProgramTitle As String


        With AxMBActX1
            iCurrentProgram = .Operation.GetCurrentProgramNo
            sProgramTitle = .Operation.GetProgramTitle(iCurrentProgram)
        End With

        currentProgram = iCurrentProgram

    End Function
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Switches the active program number to the requested number
    Private Sub SwitchProgram(ByVal ProgramNumber As Integer)

        Try
            AxMBActX1.Operation.SetCurrentProgramNo(ProgramNumber)
        Catch ex As Exception
            ErrorLog("SwitchProgram Error: " & ex.Message & DateTime.Now.ToString())
            bError = True
        End Try

    End Sub
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Gets the QR Info from SFM_RCH.VIEW_PRODUCTIONSCHEDULE_SERIAL
    Private Function getQRInfo(ByVal sFile As String) As String

        Dim dalinstQR As New DALControl
        Dim DrawingNo As String = sFile.Split(")")(1).Split("_")(1)
        Dim PartNo As String = sFile.Split("&")(0).Split("_")(2)
        Dim SerialNo As String = sFile.Split("_")(0) & "-" & sFile.Split("(")(1).Split(")")(0)
        Dim CurrentYear As String = DateTime.Now.ToString("yyyy")
        Dim count As Integer = 0
        Dim todaysdate As System.DateTime = System.DateTime.Now
        Dim CurrentDate As String = String.Format("{0:dd-MMM-yy}", todaysdate)
        Dim plateInfo As String = Nothing

        If PartNo = "72200183007" Or PartNo = "72200184027" Then
            dalinstQR.RunQuery("Select PRODLINE from SFM_RCH.VIEW_PRODUCTIONSCHEDULE_SERIAL where SERIAL = '" & SerialNo & "'")
            count = dalinstQR.SQLDataset01.Tables(0).Rows.Count
            If count <> 0 Then
                Dim MaterialInfo As String = dalinstQR.SQLDataset01.Tables(0).Rows(0).Item(0)
                plateInfo = "https://energy-assets.com/?S=" & SerialNo & "&9D=" & CurrentYear & "&1V=RCH&H=Siemens_Energy&1P=" & MaterialInfo
            Else
                Dim MaterialInfo As String = "100"
                plateInfo = "https://energy-assets.com/?S=" & SerialNo & "&9D=" & CurrentYear & "&1V=RCH&H=Siemens_Energy&1P=" & MaterialInfo
            End If
        End If

        If plateInfo = Nothing Then
            ErrorLog("Error: QR Info Not found for " & sFile)
        End If

        getQRInfo = plateInfo

    End Function
    'EPM 7/31/2019

    'EPM 7/31/2019
    'Clears all errors from engraver
    Private Sub ClearErrors()

        Try
            AxMBActX1.Operation.ClearError()
        Catch ex As Exception
            ErrorLog("ClearErrors Error: " & ex.Message & DateTime.Now.ToString())
            bError = True
        End Try

    End Sub
    'EPM 7/31/2019

    ' EPM 8/29/2019
    Public Function GetBarcode() As String
        Dim sFile As String = "C:\Temp\Finished.txt"
        If File.Exists(sFile) Then
            Dim sBarcode As String() = File.ReadAllLines(sFile)
            If sBarcode.Length = 1 Then
                GetBarcode = sBarcode(0)
            Else
                GetBarcode = sBarcode(0) & ";;;" & sBarcode(1)
            End If
        Else
            GetBarcode = ""
        End If
    End Function
    ' EPM 8/29/2019


    'DWS 10/13/2020
    Public Sub DeleteMarkingFile(FileName As String)
        Try
            AxMBActX1.DeleteControllerFile(FileName)
        Catch ex As Exception
            ErrorLog("DeleteMarkingFile Error: " & ex.Message & DateTime.Now.ToString())
            bError = True
        End Try

    End Sub

    Public Sub ErrorLog(msg As String)
        Using sw As New StreamWriter("C:\Users\z003vant\Desktop\ErrorLog.txt", True)
            sw.WriteLine(msg)
        End Using
    End Sub

End Module

