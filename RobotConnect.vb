Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class RobotConnect

    'Public attributes that each object of the class will use
    Public connectName As String
    Public connected As Boolean = False
    Public ipAddress As IPAddress
    Public serverIPaddress As IPAddress
    Public robotSock As Socket
    Public listeningPort As Integer
    Public clientConnected As Boolean = False
    Public exception As String = Nothing

    'Constructor that sets the ip address of each object
    Public Sub New(name As String, ip As String, ip2 As String, port As Integer)
        connectName = name
        ipAddress = IPAddress.Parse(ip)
        serverIPaddress = IPAddress.Parse(ip2)
        listeningPort = port
    End Sub

    'Destructor that closes socket connections upon end of the program
    Protected Overrides Sub Finalize()
        If connected = True Then
            robotSock.Close()
        End If
    End Sub

    'Creates a socket connection to the specified robot dashboard port
    Public Sub Connect()
        Dim robotEP As New IPEndPoint(ipAddress, 29999)
        Dim s As New Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
        Dim ConnectError As Integer = 0
        While connected = False
            If ConnectError = 5 Then
                MsgBox("The connection to the robots has errored out 5 times, please try again later")
            End If
            Try
                s.Connect(robotEP)
                connected = True
                robotSock = s
            Catch ex As Exception
                ConnectError += 1
                Threading.Thread.Sleep(500)
            End Try
        End While

        Try
            Dim bytes(1024) As Byte
            Dim bytesRec As Integer = s.Receive(bytes)
            If bytesRec <> 0 Then
                Dim strRec = Encoding.ASCII.GetString(bytes)
                Console.WriteLine(strRec)
            End If
        Catch ex As Exception
            MsgBox("Error receiving information after connecting to the robot: " & ex.Message)
        End Try
    End Sub

    'Opens up a network stream with the robot and allows for direct communication with strings
    Public Sub WriteTCP(listenFor As String, response As String)
        Try
            Dim tcpListener As New TcpListener(serverIPaddress, listeningPort)
            tcpListener.Start()
            Dim tcpClient As TcpClient = tcpListener.AcceptTcpClient()
            Dim stream As NetworkStream = tcpClient.GetStream()

            While tcpClient.Client.Connected()
                Dim arrayBytesRequest As Byte() = New Byte(tcpClient.Available - 1) {}
                Dim nRead = stream.Read(arrayBytesRequest, 0, arrayBytesRequest.Length)

                If nRead > 0 Then
                    Dim ReadStr = Encoding.ASCII.GetString(arrayBytesRequest)
                    If ReadStr = listenFor Then
                        Dim responseBytes = Encoding.ASCII.GetBytes(response & Environment.NewLine)
                        stream.Write(responseBytes, 0, responseBytes.Length)
                        tcpListener.Stop()
                        tcpClient.Close()
                        Exit While
                    Else
                        Console.WriteLine("Received something other than expected: " & ReadStr)
                    End If
                Else
                    If tcpClient.Available = 0 Then
                        stream.Close()
                    End If
                End If
            End While
        Catch ex As Exception
            MsgBox("Write TCP Error: " & ex.Message)
        End Try
    End Sub

    Public Function ReceiveTCP(listenFor As String, secs As Integer)
        Try
            Dim ClientThread As New Threading.Thread(New Threading.ThreadStart(Sub() BeginConnect(listenFor)))
            ClientThread.IsBackground = True
            ClientThread.Start()
            ClientThread.Join(secs * 1000)
            ClientThread.Abort()
            If clientConnected = False Then
                If exception <> Nothing Then
                    ErrorLog("Error receiving TCP in BeginConnect(): " & exception & " " & DateTime.Now.ToString())
                End If
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            ErrorLog("ReceiveTCP Error 2: " & ex.Message & " " & DateTime.Now.ToString())
            Return False
        End Try
    End Function

    Public Sub BeginConnect(ByVal listenFor As String)
        Try
            clientConnected = False
            exception = Nothing

            Dim tcpListener As New TcpListener(listeningPort)
            tcpListener.Start()

            While tcpListener.Pending() = False
                Threading.Thread.Sleep(100)
            End While

            Dim tcpClient As TcpClient = tcpListener.AcceptTcpClient()
            Dim stream As NetworkStream = tcpClient.GetStream()

            While tcpClient.Client.Connected()
                Dim arrayBytesRequest As Byte() = New Byte(tcpClient.Available - 1) {}
                Dim nRead = stream.Read(arrayBytesRequest, 0, arrayBytesRequest.Length)

                If nRead > 0 Then
                    Dim ReadStr = Encoding.ASCII.GetString(arrayBytesRequest)
                    If ReadStr = listenFor Then
                        tcpListener.Stop()
                        tcpClient.Close()
                        clientConnected = True
                        Exit While
                    Else
                        stream.Close()
                        Exit While
                    End If
                Else
                    If tcpClient.Available = 0 Then
                        Exit While
                    End If
                End If
            End While
        Catch ex As Exception
            exception = ex.Message
        End Try
    End Sub

    Public Function SendAndReceive(cmd As String)
        Try
            If connected = True Then
                Dim cmdStr = cmd & Environment.NewLine
                Dim cmdBytes = Encoding.ASCII.GetBytes(cmdStr)
                robotSock.Send(cmdBytes)

                Threading.Thread.Sleep(100)
                Dim bytes(1024) As Byte
                Dim bytesRec As Integer = robotSock.Receive(bytes)
                If bytesRec <> 0 Then
                    Dim strRec = Encoding.ASCII.GetString(bytes, 0, bytesRec)
                    Return strRec
                Else
                    Return "Nothing"
                End If
            Else
                Return "Error"
            End If
        Catch ex As Exception
            MsgBox("Error when sending/receiving commands to the robot dashboard server: " & ex.Message)
            Return "Error"
        End Try
    End Function

    Public Function Play()
        Dim cmd = "play"
        Dim response As String = SendAndReceive(cmd)
        If response.Contains("Starting program") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Pause()
        Dim cmd = "pause"
        Dim response As String = SendAndReceive(cmd)
        If response.Contains("Pausing program") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function StopRobot()
        Dim cmd = "stop"
        Dim response As String = SendAndReceive(cmd)
        If response.Contains("Stopped") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Load(program As String)
        Dim cmd = "load " & program
        Dim response As String = SendAndReceive(cmd)
        If response.Contains("Loading program") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Quit()
        Dim cmd = "quit"
        Dim response As String = SendAndReceive(cmd)
        If response.Contains("Disconnected") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Shutdown()
        Dim cmd = "shutdown"
        Dim response As String = SendAndReceive(cmd)
        If response.Contains("Shutting down") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Running()
        Dim cmd = "running"
        Dim response As String = SendAndReceive(cmd)
        If response.Contains("true") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetLoadedProgram()
        Dim cmd = "get loaded program"
        Dim response As String = SendAndReceive(cmd)
        Return response.Split("/")(2).Split(".")(0) & ".urp"
    End Function

    Public Function ProgramState()
        Dim cmd = "programState"
        Dim response As String = SendAndReceive(cmd)
        Return response
    End Function

    Public Function PowerOn()
        Dim cmd = "power on"
        Dim response As String = SendAndReceive(cmd)
        Return response
    End Function

    Public Function PowerOff()
        Dim cmd = "power off"
        Dim response As String = SendAndReceive(cmd)
        Return response
    End Function

    Public Function BrakeRelease()
        Dim cmd = "brake release"
        Dim response As String = SendAndReceive(cmd)
        Return response
    End Function

    Public Function SafetyStatus()
        Dim cmd = "safetystatus"
        Dim response As String = SendAndReceive(cmd)
        If response.Contains("Nothing") Or response.Contains("Error") Then
            Return Nothing
        End If
        Return response.Split(": ")(1)
    End Function

    Public Function UnlockProtectiveStop()
        Dim cmd = "unlock protective stop"
        Dim response As String = SendAndReceive(cmd)
        If response.Contains("Protective stop releasing") Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function StartProgram(program As String)
        If Load(program) Then
            Threading.Thread.Sleep(250)
            If Play() Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
End Class
