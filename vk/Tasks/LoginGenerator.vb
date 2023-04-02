Imports System.IO

Public Class LoginGenerator
    Private ReadOnly fioList As List(Of String)
    Private ReadOnly usedLogins As HashSet(Of String)
    Private ReadOnly loginCounts As Dictionary(Of String, Integer)

    Public Sub New(filePath As String)
        fioList = File.ReadAllLines(filePath).ToList()
        usedLogins = New HashSet(Of String)()
        loginCounts = New Dictionary(Of String, Integer)()
    End Sub

    Public Sub Generate(outputFilePath As String)
        Using writer As New StreamWriter(outputFilePath)
            For Each fio In fioList
                Dim login = GenerateLogin(fio)
                usedLogins.Add(login)
                writer.WriteLine(login)
            Next
        End Using
    End Sub

    Private Function GenerateLogin(fio As String) As String
        Dim parts = fio.Split(" ")

        Dim lastName = parts(0)
        Dim firstName = parts(1).ToLower()
        Dim login = $"{firstName}.{lastName.ToLower()}"
        If IsValidLogin(login) Then
            Return login
        End If

        Dim firstNameInitial = firstName.Substring(0, 1).ToLower()
        login = $"{firstNameInitial}.{lastName.ToLower()}"
        If IsValidLogin(login) Then
            Return login
        End If

        Dim secondNameInitial = parts(2).Substring(0, 1).ToLower()
        login = $"{firstNameInitial}.{secondNameInitial}.{lastName.ToLower()}"
        If IsValidLogin(login.ToLower()) Then
            Return login.ToLower()
        End If

        If lastName.Contains("-") And IsValidLogin(lastName) Then
            Return lastName
        End If

        If IsValidLogin(lastName.ToLower()) Then
            Return lastName.ToLower()
        End If

        Dim count = 0
        If loginCounts.ContainsKey(login) Then
            count = loginCounts(login)
        End If
        Do
            count += 1
            login = $"{firstNameInitial}.{lastName.ToLower()}{count}"
        Loop Until IsValidLogin(login)
        loginCounts(login) = count
        Return login
    End Function

    Private Function IsValidLogin(login As String) As Boolean
        Return login.Length <= 20 AndAlso Not usedLogins.Contains(login)
    End Function
End Class