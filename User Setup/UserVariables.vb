Public Class UserVariables
    Public Shared Sub SetName(ByVal UserID As ULong, ByVal Name As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\name.txt", Name)
    End Sub
    Public Shared Function GetName(ByVal UserID As ULong) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\name.txt")
    End Function
    Public Shared Sub SetAge(ByVal UserID As ULong, ByVal Age As Integer)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\age.txt", Age.ToString())
    End Sub
    Public Shared Function GetAge(ByVal UserID As ULong) As Integer
        Return Int32.Parse(System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\age.txt"))
    End Function
    Public Shared Sub SetCountry(ByVal UserID As ULong, ByVal Country As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\country.txt", Country)
    End Sub
    Public Shared Function GetCountry(ByVal UserID As ULong) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\country.txt")
    End Function
    Public Shared Sub SetStatus(ByVal UserID As ULong, ByVal Status As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\status.txt", Status)
    End Sub
    Public Shared Function GetStatus(ByVal UserID As ULong) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\status.txt")
    End Function
    Public Shared Sub SetGender(ByVal UserID As ULong, ByVal Gender As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\gender.txt", Gender)
    End Sub
    Public Shared Function GetGender(ByVal UserID As ULong) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\gender.txt")
    End Function
    Public Shared Sub SetRole(ByVal UserID As ULong, ByVal Role As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\role.txt", Role)
    End Sub
    Public Shared Function GetRole(ByVal UserID As ULong) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\role.txt")
    End Function
    Public Shared Sub SetBanned(ByVal UserID As ULong, ByVal Banned As Boolean)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\banned.txt", Banned.ToString())
    End Sub
    Public Shared Function GetBanned(ByVal UserID As ULong) As Boolean
        Return Boolean.Parse(System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\banned.txt"))
    End Function
    Public Shared Sub SetBanReason(ByVal UserID As ULong, ByVal BanReason As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\ban_reason.txt", BanReason)
    End Sub
    Public Shared Function GetBanReason(ByVal UserID As ULong) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\ban_reason.txt")
    End Function
    Public Shared Sub SetOnlyGirls(ByVal UserID As ULong, ByVal OnlyGirls As Boolean)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\only_girls.txt", OnlyGirls.ToString())
    End Sub
    Public Shared Function GetOnlyGirls(ByVal UserID As ULong) As Boolean
        Return Boolean.Parse(System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\only_girls.txt"))
    End Function
    Public Shared Sub SetReports(ByVal UserID As ULong, ByVal Reports As Integer)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\reports.txt", Reports.ToString())
    End Sub
    Public Shared Function GetReports(ByVal UserID As ULong) As Integer
        Return Int32.Parse(System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\reports.txt"))
    End Function
    Public Shared Sub SetLanguage(ByVal UserID As ULong, ByVal Language As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\language.txt", Language.ToLower())
    End Sub
    Public Shared Function GetLanguage(ByVal UserID As ULong) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\language.txt").ToLower()
    End Function
    Public Shared Sub SetName(ByVal UserID As Integer, ByVal Name As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\name.txt", Name)
    End Sub
    Public Shared Function GetName(ByVal UserID As Integer) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\name.txt")
    End Function
    Public Shared Sub SetAge(ByVal UserID As Integer, ByVal Age As Integer)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\age.txt", Age.ToString())
    End Sub
    Public Shared Function GetAge(ByVal UserID As Integer) As Integer
        Return Int32.Parse(System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\age.txt"))
    End Function
    Public Shared Sub SetCountry(ByVal UserID As Integer, ByVal Country As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\country.txt", Country)
    End Sub
    Public Shared Function GetCountry(ByVal UserID As Integer) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\country.txt")
    End Function
    Public Shared Sub SetStatus(ByVal UserID As Integer, ByVal Status As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\status.txt", Status)
    End Sub
    Public Shared Function GetStatus(ByVal UserID As Integer) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\status.txt")
    End Function
    Public Shared Sub SetGender(ByVal UserID As Integer, ByVal Gender As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\gender.txt", Gender)
    End Sub
    Public Shared Function GetGender(ByVal UserID As Integer) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\gender.txt")
    End Function
    Public Shared Sub SetRole(ByVal UserID As Integer, ByVal Role As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\role.txt", Role)
    End Sub
    Public Shared Function GetRole(ByVal UserID As Integer) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\role.txt")
    End Function
    Public Shared Sub SetBanned(ByVal UserID As Integer, ByVal Banned As Boolean)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\banned.txt", Banned.ToString())
    End Sub
    Public Shared Function GetBanned(ByVal UserID As Integer) As Boolean
        Return Boolean.Parse(System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\banned.txt"))
    End Function
    Public Shared Sub SetBanReason(ByVal UserID As Integer, ByVal BanReason As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\ban_reason.txt", BanReason)
    End Sub
    Public Shared Function GetBanReason(ByVal UserID As Integer) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\ban_reason.txt")
    End Function
    Public Shared Sub SetOnlyGirls(ByVal UserID As Integer, ByVal OnlyGirls As Boolean)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\only_girls.txt", OnlyGirls.ToString())
    End Sub
    Public Shared Function GetOnlyGirls(ByVal UserID As Integer) As Boolean
        Return Boolean.Parse(System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\only_girls.txt"))
    End Function
    Public Shared Sub SetReports(ByVal UserID As Integer, ByVal Reports As Integer)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\reports.txt", Reports.ToString())
    End Sub
    Public Shared Function GetReports(ByVal UserID As Integer) As Integer
        Return Int32.Parse(System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\reports.txt"))
    End Function
    Public Shared Sub SetLanguage(ByVal UserID As Integer, ByVal Language As String)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\language.txt", Language.ToLower())
    End Sub
    Public Shared Function GetLanguage(ByVal UserID As Integer) As String
        Return System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\language.txt").ToLower()
    End Function
End Class