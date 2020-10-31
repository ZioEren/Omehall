Imports Discord.WebSocket
Imports Discord
Public Class Couple
    Dim User1 As SocketUser
    Dim User2 As SocketUser
    Dim User3 As Telegram.Bot.Types.User
    Dim User4 As Telegram.Bot.Types.User
    Public Sub New(ByVal U1 As SocketUser, ByVal U2 As SocketUser)
        User1 = U1
        User2 = U2
    End Sub
    Public Sub New(ByVal U1 As Telegram.Bot.Types.User, ByVal U2 As Telegram.Bot.Types.User)
        User3 = U1
        User4 = U2
    End Sub
    Public Function GetUser1() As SocketUser
        Return User1
    End Function
    Public Function GetUser2() As SocketUser
        Return User2
    End Function
    Public Function GetUser3() As Telegram.Bot.Types.User
        Return User3
    End Function
    Public Function GetUser4() As Telegram.Bot.Types.User
        Return User4
    End Function
    Public Sub Message(ByVal User As SocketUser, ByVal Msg As String)
        If User.Id = User1.Id Then
            If UserVariables.GetLanguage(User2.Id).Equals("eng") Then
                User2.SendMessageAsync(":star: **Stranger:** " + Msg)
            ElseIf UserVariables.GetLanguage(User2.Id).Equals("ita") Then
                User2.SendMessageAsync(":star: **Sconosciuto:** " + Msg)
            End If
        ElseIf User.Id = User2.Id Then
            If UserVariables.GetLanguage(User1.Id).Equals("eng") Then
                User1.SendMessageAsync(":star: **Stranger:** " + Msg)
            ElseIf UserVariables.GetLanguage(User1.Id).Equals("ita") Then
                User1.SendMessageAsync(":star: **Sconosciuto:** " + Msg)
            End If
        End If
    End Sub
    Public Sub Message(ByVal User As Telegram.Bot.Types.User, ByVal Msg As String)
        If User.Id = User3.Id Then
            If UserVariables.GetLanguage(User4.Id).Equals("eng") Then
                TelegramBot.GetTelegram().SendTextMessageAsync(User4.Id, "⭐️ Stranger: " + Msg)
            ElseIf UserVariables.GetLanguage(User4.Id).Equals("ita") Then
                TelegramBot.GetTelegram().SendTextMessageAsync(User4.Id, "⭐️ Sconosciuto: " + Msg)
            End If
        ElseIf User.Id = User4.Id Then
            If UserVariables.GetLanguage(User3.Id).Equals("eng") Then
                TelegramBot.GetTelegram().SendTextMessageAsync(User3.Id, "⭐️ Stranger: " + Msg)
            ElseIf UserVariables.GetLanguage(User3.Id).Equals("ita") Then
                TelegramBot.GetTelegram().SendTextMessageAsync(User3.Id, "⭐️ Sconosciuto: " + Msg)
            End If
        End If
    End Sub
End Class