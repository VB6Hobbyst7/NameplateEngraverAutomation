Imports System.IO
Imports RECV1.FilePaths

'Module of common database queries

Module DatabaseQuery

    '---------------------------------------------------- Functions ------------------------------------------------------------

    ' CJD 5/2/2019
    'Checks if an item is in the Queue. Returns true if it is and false if it is not in the queue already
    Public Function ExistingCheck(ByRef SerialNum As String, ByRef DrawingNum As String)
        Dim dalExisting As New DALControl
        Dim ExistingQuery = "SELECT * FROM SFM_RCH.NP_QUEUE_NEW_API WHERE ACTIUAL_DATE IS NULL AND SN = '" & SerialNum & "' AND DRAWING = '" & DrawingNum & "'"
        dalExisting.RunQuery(ExistingQuery)
        Dim rowCount = dalExisting.SQLDataset01.Tables(0).Rows.Count
        If rowCount > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    ' CJD 5/2/2019
    ' CJD 5/2/2019
    'Gets the top-most item in the queue and returns the row
    Public Function GetFirstQueueItem()
        Dim dalFirst As New DALControl
        Dim FirstQuery = "SELECT * FROM SFM_RCH.NP_QUEUE_NEW_API WHERE ACTIUAL_DATE IS NULL AND PRIORITY IS NOT NULL ORDER BY REQUEST_DATE ASC, SN ASC, DRAWING ASC"
        dalFirst.RunQuery(FirstQuery)
        Dim rowCount = dalFirst.SQLDataset01.Tables(0).Rows.Count
        If rowCount > 0 Then
            Return dalFirst.SQLDataset01.Tables(0).Rows(0)
        Else
            dalFirst.RunQuery("SELECT * FROM SFM_RCH.NP_QUEUE_NEW_API WHERE ACTIUAL_DATE IS NULL AND PRIORITY IS NULL ORDER BY REQUEST_DATE ASC, SN ASC, DRAWING ASC")
            If dalFirst.SQLDataset01.Tables(0).Rows.Count > 0 Then
                Return dalFirst.SQLDataset01.Tables(0).Rows(0)
            End If
        End If
        Return Nothing
    End Function
    ' CJD 5/2/2019
    '----------------------------------------------------------------------------------------------------------------------------


    '---------------------------------------------------- Subs ------------------------------------------------------------

    'EPM
    'Sends a new request for the robot program to run and returns the date they were requested
    Function RequestRobotProgram(sProgram As String) As String

        Dim sDate As String = "TO_DATE('" & Date.Now.ToString("dd-MM-yyyy HH:mm:ss") & "', 'DD-MM-YYYY HH24:MI:SS')"

        Dim dalControl As New DALControl
        Dim sTempSQL As String = "INSERT INTO SFM_RCH.Y_ROBOT_PROGRAM_QUEUE (PROGRAM, REQUEST_DATE) VALUES ('" & sProgram & "', " & sDate & ")"
        dalControl.RunQuery(sTempSQL)

        Return sDate

    End Function
    'EPM

    'EPM
    'Checks to see if the robot program has been completed
    Function IsRobotProgramComplete(sProgram As String, sDate As String) As Boolean

        Dim dalControl As New DALControl
        Dim sTempSQL As String = "SELECT ACTUAL_DATE FROM SFM_RCH.Y_ROBOT_PROGRAM_QUEUE WHERE PROGRAM = '" & sProgram & "' AND REQUEST_DATE = " & sDate
        dalControl.RunQuery(sTempSQL)

        Dim QueryResult = dalControl.SQLDataset01.Tables(0).Rows(0).Item(0)

        If IsDBNull(QueryResult) Then            'If the request has NOT been completed
            Return False
        Else
            Return True
        End If

    End Function

    Public Function GetNextRobotProg() As DataRow

        Dim dal As New DALControl
        Dim Query = "SELECT * FROM SFM_RCH.Y_ROBOT_PROGRAM_QUEUE WHERE ACTUAL_DATE IS NULL ORDER BY REQUEST_DATE ASC"
        dal.RunQuery(Query)

        If dal.SQLDataset01.Tables(0).Rows.Count > 0 Then
            Return dal.SQLDataset01.Tables(0).Rows(0)
        Else
            Return Nothing
        End If

    End Function

    Public Sub UpdateQueueRobot(sProgram As String, Optional sStatus As String = Nothing)

        Dim sDate As String = "TO_DATE('" & Date.Now.ToString("dd-MM-yyyy HH:mm:ss") & "', 'DD-MM-YYYY HH24:MI:SS')"

        Dim dalUpdate As New DALControl
        Dim Query As String
        If sStatus = Nothing Then
            Query = "UPDATE SFM_RCH.Y_ROBOT_PROGRAM_QUEUE SET ACTUAL_DATE = " & sDate & " WHERE PROGRAM = '" & sProgram &
                        "' AND REQUEST_DATE = (SELECT MAX(REQUEST_DATE) FROM SFM_RCH.Y_ROBOT_PROGRAM_QUEUE WHERE PROGRAM = '" & sProgram & "')"
        Else
            Query = "UPDATE SFM_RCH.Y_ROBOT_PROGRAM_QUEUE SET ACTUAL_DATE = " & sDate & ", STATUS = '" & sStatus & "' WHERE PROGRAM = '" & sProgram &
                        "' AND REQUEST_DATE = (SELECT MAX(REQUEST_DATE) FROM SFM_RCH.Y_ROBOT_PROGRAM_QUEUE WHERE PROGRAM = '" & sProgram & "')"
        End If

        dalUpdate.RunQuery(Query)

    End Sub
    'EPM

    ' CJD 5/2/2019
    'Sends everything that's been engraved to a database
    Public Sub EngravedOrdersQuery(ByRef SerialNum As String, ByRef DrawingNum As String)
        Dim todaysDate As System.DateTime = System.DateTime.Now
        Dim currentDate As String = String.Format("{0:dd-MMM-yy}", todaysDate)
        Dim dalCompleted As New DALControl
        Dim CompleteQuery As String = "INSERT INTO SFM_RCH.ENGRAVED_ORDERS (SN, DOCUMENT, ENGRAVED_DATE) VALUES ('" & SerialNum & "','" & DrawingNum & "'," & currentDate & ")"
        dalCompleted.RunQuery(CompleteQuery)
    End Sub
    ' CJD 5/2/2019

    ' CJD 5/2/2019
    'Sets an item to completion by setting the ACTUAL_DATE (this removes it from the queue)
    Public Sub CompletedQuery(ByRef SerialNum As String, ByRef DrawingNum As String)
        Dim todaysDate As System.DateTime = System.DateTime.Now
        Dim currentDate As String = String.Format("{0:dd-MMM-yy}", todaysDate)
        Dim dalCompleted As New DALControl, dalSelect As New DALControl
        Dim sQuery As String, sQuerySelect As String
        Dim sQuantity As String
        If SerialNum.Substring(0, 1) = "3" Then     'CB order
            sQuery = "update SFM_RCH.NP_QUEUE_NEW_API set ACTIUAL_DATE = '" & currentDate & "' where SN = '" & SerialNum & "' AND DRAWING = '" & DrawingNum & "' AND ACTIUAL_DATE IS NULL"
        ElseIf SerialNum.Substring(0, 1) = "9" Then 'VR Order
            sQuerySelect = "SELECT QUANTITY_COUNT FROM SFM_RCH.NP_QUEUE_NEW_API WHERE SN = '" & SerialNum & "' AND DRAWING = '" &
                DrawingNum & "' AND ACTIUAL_DATE IS NULL AND QUANTITY_COUNT > 0 ORDER BY QUANTITY_COUNT DESC"
            dalSelect.RunQuery(sQuerySelect)
            sQuantity = dalSelect.SQLDataset01.Tables(0).Rows(0).Item(0)
            sQuery = "update SFM_RCH.NP_QUEUE_NEW_API set ACTIUAL_DATE = '" & currentDate & "', QUANTITY_COUNT = 0 where SN = '" &
                SerialNum & "' AND DRAWING = '" & DrawingNum & "' AND ACTIUAL_DATE IS NULL AND QUANTITY_COUNT = " & sQuantity
        End If
        dalCompleted.RunQuery(sQuery)
    End Sub
    ' CJD 5/2/2019

    ' ___ 5/2/2019
    'Uploads each order to Magnets for SFM
    Public Sub UploadToMagnets(ByVal SerialNum As String)

        Dim dalMagnet As New DALControl
        'Log("Started UploadedToDataBase", False)
        Dim todaysdate As System.DateTime = System.DateTime.Now
        Dim CurrentDate As String = String.Format("{0:dd-MMM-yy}", todaysdate)
        'Log("Started UploadedToDataBase for SerialNum = " & SerialNum, False)

        Dim query As String, query2, PO As String
        Dim count As Integer = 0

        query = "select value from SFM_RCH.magnets where po_sequence = '" & SerialNum & "' and columns = '#NP'"
        dalMagnet.RunQuery(query)

        Try 'If rows exist, then count. Else exit the try
            count = dalMagnet.SQLDataset01.Tables(0).Rows.Count
        Catch
        End Try

        'Log("Count = " & count, False)

        query2 = "SELECT PO FROM SFM_RCH.VIEW_PRODUCTIONSCHEDULE_SERIAL WHERE SERIAL = '" & SerialNum & "' and (PRODLINE = 'MOD' or PRODUCT = 'CB')"
        dalMagnet.RunQuery(query2)
        PO = dalMagnet.SQLDataset01.Tables(0).Rows(0).Item(0)

        'Log("PO = " & PO, False)

        If count = 0 Then
            dalMagnet.RunQuery("INSERT into SFM_RCH.magnets (po,po_sequence,columns,value) VALUES ('" & PO & "','" & SerialNum & "','#NP','4')")
            'Log("Ran Query: INSERT into SFM_RCH.magnets (po,po_sequence,columns,value) VALUES ('" & PO & "','" & SerialNum & "','#NP','4')"")", False)
        Else
            dalMagnet.RunQuery("update SFM_RCH.magnets set value = '4' where po_sequence = '" & SerialNum & "' and columns = '#NP' and PO = '" & PO & "'")
            'Log("Ran Query: update SFM_RCH.magnets set value = '4' where po_sequence = '" & SerialNum & "' and columns = '#NP' and PO = '" & PO & "'", False)
        End If

    End Sub
    ' ___ 5/2/2019

    '----------------------------------------------------------------------------------------------------------------------------

End Module
