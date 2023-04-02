Imports System.IO
Imports JetBrains.Annotations
Imports Newtonsoft.Json

Public Class DataExchanger
    Private ReadOnly startDataFilePath As String
    Private ReadOnly employeeDataFilePath As String

    Public Sub New (startDataFilePath As String, employeeDataFilePath As String)
        Me.startDataFilePath = startDataFilePath
        Me.employeeDataFilePath = employeeDataFilePath
    End Sub
    Public Sub Exchange(finalDataFilePath As String)
        Dim startData = JsonConvert.DeserializeObject (Of List(Of IDMData))(File.ReadAllText(startDataFilePath))
        Dim employeeData = JsonConvert.DeserializeObject (Of List(Of EmployeeData))(File.ReadAllText(employeeDataFilePath))

        Dim finalDictionary = startData.ToDictionary(function(data) data.PersonId)
        Dim actualEmployees = employeeData.GroupBy(function(data) data.Login) _
                                 .Select(function(data) data.OrderBy(function(x) x.DateFired Is Nothing) _
                                                            .ThenBy(function(y) y.DateFired) _
                                                            .Last())

        For Each idm In actualEmployees
            If Not finalDictionary.ContainsKey(idm.PersonId) Then
                If idm.DateFired Is Nothing Then
                    finalDictionary.Add(idm.PersonId, New IDMData(idm))
                End If
                Continue For
            End If
            finalDictionary(idm.PersonId) = New IDMData(idm)
        Next
        File.WriteAllText(finalDataFilePath, JsonConvert.SerializeObject(finalDictionary.Select(function(pair) pair.Value)))
    End Sub
End Class

Public Class EmployeeData
    <JsonProperty("id")>
    Public Property Id As Integer
    <JsonProperty("login")>
    Public Property Login As String
    <JsonProperty("personId")>
    Public Property PersonId As Integer
    <JsonProperty("specialization")>
    Public Property Specialization As String
    <JsonProperty("date_hired")>
    Public Property DateHired As String
    <JsonProperty("date_fired")>
    Public Property DateFired As String
End Class

<UsedImplicitly>
Public Class IDMData
    Protected Sub New()
    End Sub

    Public Sub New(employeeData as EmployeeData)
        Me.New(employeeData.Login, employeeData.PersonId, employeeData.Specialization, employeeData.DateHired,
               employeeData.DateFired)
    End Sub

    Public Sub New(login as String, personId as String, specialization as String, date_hired As String,
                   date_fired As String)
        Me.Login = login
        Me.PersonId = personId
        Me.Specialization = specialization
        Me.DateHired = date_hired
        Me.DateFired = date_fired
    End Sub

    <JsonProperty("login")>
    Public Property Login As String

    <JsonProperty("personId")>
    Public Property PersonId As String

    <JsonProperty("specialization")>
    Public Property Specialization As String

    <JsonProperty("date_hired")>
    Public Property DateHired As String

    <JsonProperty("date_fired")>
    Public Property DateFired As String

    <JsonProperty("disabled")>
    Public Property Disabled As Boolean = DateFired Is Nothing
End Class