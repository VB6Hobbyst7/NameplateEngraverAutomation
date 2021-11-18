Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Net.Sockets
Imports System.Net

Public Class LoadingForm

    Public robot As New RobotConnect("robot", "169.254.245.127", "169.254.245.130", 55555)
    Public iAttempts As Integer = 0
    Public rError As Boolean = False
    ' EPM 8/30/2019
    Private Sub LoadingForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        robot.Connect()
        Me.Update()

        Dim QueueEmpty As Boolean = False
        Dim sFileDelete As String = " "
        Dim QueueCounter As Integer = 0

        'Opens the door at the beginning of the program, making open the default state
        Dim result As Boolean
        Dim count As Integer = 0
        Dim count2 As Integer = 0
        Do
            Do
                robot.StartProgram("/programs/open.urp")
                result = robot.ReceiveTCP("BeginOpen", 5)
                count += 1
            Loop While result = False And count < 3
            If count <> 3 Then
                result = robot.ReceiveTCP("EndOpen", 10)
                count2 += 1
            Else
                ErrorLog("TimeOut and Retry error in open.urp regarding BeginOpen " & DateTime.Now.ToString())
                rError = True
                Exit Do
            End If
        Loop While result = False And count2 < 3
        If count2 = 3 Then
            ErrorLog("TimeOut and Retry error in open.urp regarding EndOpen " & DateTime.Now.ToString())
            rError = True
        End If
        count = 0
        count2 = 0

        While QueueEmpty = False                                'while queue is not empty
            Dim TopItem = DatabaseQuery.GetFirstQueueItem()     'Get the first item in the queue

            If TopItem Is Nothing Then                          'If there is no item then set Queue empty = true and the while loop will exit
                QueueEmpty = True
            Else                                                'Else

                Dim SerialNum As String = TopItem.Item(0).ToString                          'Get the Serial num from the first item in the queue
                Dim DrawingNum As String = TopItem.Item(1).ToString                         'Get the Drawing num from the first item in the queue
                Dim PartNum As String = TopItem.Item(2).ToString                            'Get the Part num from the first item in the queue
                Dim RobotPartNum As String = TopItem.Item(2).ToString

                If SerialNum.Substring(0, 1) = "9" Then 'and TopItem.item(9).ToString = "2" Then         'If it is a VR order 'TODO: PUT ANOTHER CHECK IF IT IS THE FIRST 
                    CreateDrawing(SerialNum)
                    If MarkingBuilder.GetBarcode() <> "" Then 'If a barcode is required, change the drawingNum to add &BC
                        If MarkingBuilder.GetBarcode().Contains(";") Then
                            PartNum = PartNum & "&BC&BC"
                        Else
                            PartNum = PartNum & "&BC"
                        End If
                    End If
                End If

                If RobotPartNum.Contains("&QR") Then
                    RobotPartNum = RobotPartNum.Replace("&QR", "")
                End If

                'Places the plate into the engraver and awaits confirmation of success   ****Logic altered by EMK 10/6/2021
                If rError = False Then
                    Do
                        robot.StartProgram("/programs/" & RobotPartNum & ".urp")
                        result = robot.ReceiveTCP("PartNumBegin", 5)
                        count += 1
                    Loop While result = False And count < 3
                    If count = 3 Then
                        ErrorLog("TimeOut and Retry Error occurred in " & RobotPartNum & ".urp regarding PartNumBegin " & DateTime.Now.ToString())
                        rError = True
                    Else
                        'Add robot.ReceiveTCP("CalPickBegin", 30)
                        'if calpickbegin is not received, then we know the robot most likely got stuck on a plate...
                        'so release suction and go home, then let EAV1 restart the application
                        result = robot.ReceiveTCP("CalPickEnd", 45)
                        If result = False Then
                            robot.StartProgram("/programs/CalibrationFix.urp")
                            rError = True
                        End If
                        If rError = False Then
                            result = robot.ReceiveTCP("PartNumEnd", 45)
                            If result = False Then
                                ErrorLog("TimeOut Error occurred in " & RobotPartNum & ".urp regarding PartNumEnd" & DateTime.Now.ToString())
                                'probably go home and release suction here, section 5 on paper
                                rError = True
                            End If
                        End If
                    End If
                    count = 0
                End If

                ProgressBarLoading.Value = 33
                Me.Update()

                'Uploads plate to Engraver and waits to finish engraving
                If rError = False Then
                    MarkingBuilder.UploadToEngraver(SerialNum, DrawingNum, PartNum)
                Else
                    ErrorLog("Skipped UploadToEngraver due to rError " & DateTime.Now.ToString())
                End If


                ProgressBarLoading.Value = 67
                Me.Update()

                'Removes the plate from the engraver and awaits confirmation of success   ****Logic altered by EMK 10/6/2021
                If rError = False Then
                    Do
                        robot.StartProgram("/programs/" & RobotPartNum & "_RP.urp")
                        result = robot.ReceiveTCP("PartNumRPBegin", 5)
                        count += 1
                    Loop While result = False And count < 3
                    If count = 3 Then
                        ErrorLog("TimeOut and Retry Error occurred in " & RobotPartNum & "_RP.urp regarding PartNumRPBegin " & DateTime.Now.ToString())
                        rError = True
                    Else
                        result = robot.ReceiveTCP("PartNumRPEnd", 60)
                        If result = False Then
                            ErrorLog("TimeOut Error occurred in " & RobotPartNum & "_RP.urp regarding PartNumRPEnd" & DateTime.Now.ToString())
                            'probably go home and release suction, section 7 on paper
                            rError = True
                        End If
                    End If
                    count = 0
                End If

                If bError = True Or rError = True Then               'Error occurred: removes plate and then stops program
                    Application.Exit()
                    Exit Sub
                Else
                    DatabaseQuery.CompletedQuery(SerialNum, DrawingNum)             'Sets the completion of the part/drawing
                End If

                ProgressBarLoading.Value = 100
                Me.Update()


                'DWS 10/13/2020
                'Deletes the logo file from marking builder to prevent storage error
                'MsgBox(sFileDelete)
                'Try
                sFileDelete = SerialNum.Split("-")(0) & "_(" & SerialNum.Split("-")(1) & ")" & DrawingNum & "_" & PartNum & ".MHL"
                'MarkingBuilder.DeleteMarkingFile(sFileDelete)
                'Catch ex As Exception
                'ErrorLog("Deleting Error: " & sFileDelete & " " & ex.Message & " " & DateTime.Now.ToString())
                'End Try

            End If
        End While

        Do
            Do
                robot.StartProgram("/programs/CloseAllTheWay.urp")
                result = robot.ReceiveTCP("BeginClose", 5)
                count += 1
            Loop While result = False And count < 3
            If count <> 3 Then
                result = robot.ReceiveTCP("EndClose", 25)
                count2 += 1
            Else
                ErrorLog("TimeOut and Retry error in open.urp regarding BeginOpen " & DateTime.Now.ToString())
                Exit Do
            End If
        Loop While result = False And count2 < 3
        If count2 = 3 Then
            ErrorLog("TimeOut and Retry error in open.urp regarding EndOpen " & DateTime.Now.ToString())
        End If

        Application.Exit()

    End Sub
    ' EPM 8/30/2019

    Private Declare Function ShowWindow Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal nCmdShow As SHOW_WINDOW) As Boolean

    <Flags()>
    Private Enum SHOW_WINDOW As Integer
        SW_HIDE = 0
        SW_SHOWNORMAL = 1
        SW_NORMAL = 1
        SW_SHOWMINIMIZED = 2
        SW_SHOWMAXIMIZED = 3
        SW_MAXIMIZE = 3
        SW_SHOWNOACTIVATE = 4
        SW_SHOW = 5
        SW_MINIMIZE = 6
        SW_SHOWMINNOACTIVE = 7
        SW_SHOWNA = 8
        SW_RESTORE = 9
        SW_SHOWDEFAULT = 10
        SW_FORCEMINIMIZE = 11
        SW_MAX = 11
    End Enum

    Public Sub MaximizeProcess(sProcess As String)
        For Each p As Process In Process.GetProcessesByName(sProcess)
            ShowWindow(p.MainWindowHandle, SHOW_WINDOW.SW_MINIMIZE)
            ShowWindow(p.MainWindowHandle, SHOW_WINDOW.SW_MAXIMIZE)
        Next p
    End Sub
End Class