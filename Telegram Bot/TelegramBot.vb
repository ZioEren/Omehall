Imports Telegram.Bot.Args
Public Class TelegramBot
    Private Shared WithEvents Telegram As Telegram.Bot.TelegramBotClient
    Private Shared Token As String
    Public Shared Function GetTelegram() As Telegram.Bot.TelegramBotClient
        Return Telegram
    End Function
    Public Shared Function GetToken() As String
        Return Token
    End Function
    Public Shared Sub SetToken(ByVal tToken As String)
        Token = tToken
    End Sub
    Public Shared Sub SetupTelegramBot()
        Telegram = New Telegram.Bot.TelegramBotClient(GetToken())
        Telegram.StartReceiving()
    End Sub
    Public Shared Sub Disconnect()
        Telegram.StopReceiving()
    End Sub
    Public Shared Sub Telegram_OnMessage(sender As Object, e As MessageEventArgs) Handles Telegram.OnMessage
        If Not e.Message.From.Id = Telegram.BotId And Not e.Message.From.IsBot Then
            Dim Coso As Boolean = False
            Dim CanProceed As Boolean = True
            Dim Author As Telegram.Bot.Types.User = e.Message.From
            Dim AuthorID As Integer = Author.Id
            Try
                If BlockedWords.ContainsBlockedWord(e.Message.Text) And Not UserVariables.GetRole(Author.Id) = "Gold" And Not UserVariables.GetRole(Author.Id) = "Mod" And Not UserVariables.GetRole(Author.Id) = "Admin" Then
                    UserVariables.SetBanned(AuthorID, True)
                    If UserVariables.GetLanguage(AuthorID).Equals("eng") Then
                        UserVariables.SetBanReason(AuthorID, "Usage of blocked words")
                    ElseIf UserVariables.GetLanguage(AuthorID).Equals("ita") Then
                        UserVariables.SetBanReason(AuthorID, "Uso di parole bloccate")
                    End If
                    Telegram.SendTextMessageAsync(AuthorID, "You have been banned automatically from the bot! Reason: " + UserVariables.GetBanReason(AuthorID))
                End If
            Catch ex As Exception
            End Try
            Try
                If UserVariables.GetReports(Author.Id) >= 3 Then
                    UserVariables.SetReports(AuthorID, 0)
                    UserVariables.SetBanReason(AuthorID, "Bad behaviour")
                    Telegram.SendTextMessageAsync(AuthorID, "You have been banned automatically from the bot! Reason: " + UserVariables.GetBanReason(AuthorID))
                End If
            Catch ex As Exception
            End Try
            Try
                If UserVariables.GetBanned(AuthorID) And Not UserVariables.GetRole(AuthorID) = "Gold" And Not UserVariables.GetRole(AuthorID) = "Mod" And Not UserVariables.GetRole(AuthorID) = "Admin" Then
                    Telegram.SendTextMessageAsync(AuthorID, "You are banned from the bot! Reason: " + UserVariables.GetBanReason(AuthorID))
                    CanProceed = False
                    If QueueManager.GetMatchedUsers1().Contains(Author) Then
                        QueueManager.GetMatchedUsers1().Remove(Author)
                    End If
                    If QueueManager.GetWaitingUsers1().Contains(Author) Then
                        QueueManager.GetWaitingUsers1().Remove(Author)
                    End If
                    For Each coupleh As Couple In QueueManager.GetCouples()
                        If coupleh.GetUser3().Id = Author.Id Or coupleh.GetUser4().Id = Author.Id Then
                            If coupleh.GetUser3().Id = Author.Id Then
                                Telegram.SendTextMessageAsync(coupleh.GetUser4().Id, "The user that you were talking with is now banned. Do you want to talk with another stranger?")
                            ElseIf coupleh.GetUser4().Id = Author.Id Then
                                Telegram.SendTextMessageAsync(coupleh.GetUser3().Id, "The user that you were talking with is now banned. Do you want to talk with another stranger?")
                            End If
                            If QueueManager.GetMatchedUsers1().Contains(coupleh.GetUser4()) Then
                                QueueManager.GetMatchedUsers1().Remove(coupleh.GetUser4())
                            End If
                            If QueueManager.GetMatchedUsers1().Contains(coupleh.GetUser3()) Then
                                QueueManager.GetMatchedUsers1().Remove(coupleh.GetUser3())
                            End If
                            If QueueManager.GetMatchedUsers1().Contains(Author) Then
                                QueueManager.GetMatchedUsers1().Remove(Author)
                            End If
                            If QueueManager.GetCouples().Contains(coupleh) Then
                                QueueManager.GetCouples().Remove(coupleh)
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
            End Try
            If CanProceed Then
                Try
                    If QueueManager.GetMatchedUsers1().Contains(e.Message.From) Then
                        If e.Message.Text.Replace(" ", "").ToLower().Equals(CommandExecutor.GetTrigger() + "stop") Then
                            Try
                                For Each coupleh As Couple In QueueManager.GetCouples()
                                    Try
                                        If coupleh.GetUser3().Id = e.Message.From.Id Or coupleh.GetUser4().Id = e.Message.From.Id Then
                                            If e.Message.Text.Replace(" ", "").ToLower().Equals(CommandExecutor.GetTrigger() + "stop") Then
                                                Coso = True
                                                Telegram.SendTextMessageAsync(AuthorID, "You have been disconnected from your stranger. Do you want to connect with another stranger?")
                                                If coupleh.GetUser3().Id = e.Message.From.Id Then
                                                    Telegram.SendTextMessageAsync(coupleh.GetUser4().Id, "Your stranger has been disconnected from the chat. Do you want to get connected with another stranger?")
                                                ElseIf coupleh.GetUser4().Id = e.Message.From.Id Then
                                                    Telegram.SendTextMessageAsync(coupleh.GetUser3().Id, "Your stranger has been disconnected from the chat. Do you want to get connected with another stranger?")
                                                End If
                                                If QueueManager.GetMatchedUsers1().Contains(coupleh.GetUser4()) Then
                                                    QueueManager.GetMatchedUsers1().Remove(coupleh.GetUser4())
                                                End If
                                                If QueueManager.GetMatchedUsers1().Contains(coupleh.GetUser3()) Then
                                                    QueueManager.GetMatchedUsers1().Remove(coupleh.GetUser3())
                                                End If
                                                If QueueManager.GetMatchedUsers1().Contains(Author) Then
                                                    QueueManager.GetMatchedUsers1().Remove(Author)
                                                End If
                                                If QueueManager.GetCouples().Contains(coupleh) Then
                                                    QueueManager.GetCouples().Remove(coupleh)
                                                End If
                                            Else
                                                coupleh.Message(e.Message.From, e.Message.Text)
                                            End If
                                        End If
                                    Catch ex As Exception
                                    End Try
                                Next
                            Catch ex As Exception
                            End Try
                        ElseIf e.Message.Text.Replace(" ", "").ToLower().Equals(CommandExecutor.GetTrigger() + "report") Then
                            Try
                                For Each coupleh As Couple In QueueManager.GetCouples()
                                    Try
                                        If coupleh.GetUser3().Id = e.Message.From.Id Or coupleh.GetUser4().Id = e.Message.From.Id Then
                                            If e.Message.Text.Replace(" ", "").ToLower().Equals(CommandExecutor.GetTrigger() + "report") Then
                                                Coso = True
                                                Telegram.SendTextMessageAsync(AuthorID, "This user has been reported, thanks for your support. Do you want to connect with another stranger?")
                                                If coupleh.GetUser3().Id = e.Message.From.Id Then
                                                    Dim Reports As Integer = UserVariables.GetReports(coupleh.GetUser4().Id) + 1
                                                    UserVariables.SetReports(coupleh.GetUser4().Id, Reports)
                                                    Telegram.SendTextMessageAsync(coupleh.GetUser4().Id, "Your stranger has been disconnected from the chat. Do you want to get connected with another stranger?")
                                                ElseIf coupleh.GetUser4().Id = e.Message.From.Id Then
                                                    Dim Reports As Integer = UserVariables.GetReports(coupleh.GetUser3().Id) + 1
                                                    UserVariables.SetReports(coupleh.GetUser3().Id, Reports)
                                                    Telegram.SendTextMessageAsync(coupleh.GetUser3().Id, "Your stranger has been disconnected from the chat. Do you want to get connected with another stranger?")
                                                End If
                                                If QueueManager.GetMatchedUsers1().Contains(coupleh.GetUser4()) Then
                                                    QueueManager.GetMatchedUsers1().Remove(coupleh.GetUser4())
                                                End If
                                                If QueueManager.GetMatchedUsers1().Contains(coupleh.GetUser3()) Then
                                                    QueueManager.GetMatchedUsers1().Remove(coupleh.GetUser3())
                                                End If
                                                If QueueManager.GetMatchedUsers1().Contains(Author) Then
                                                    QueueManager.GetMatchedUsers1().Remove(Author)
                                                End If
                                                If QueueManager.GetCouples().Contains(coupleh) Then
                                                    QueueManager.GetCouples().Remove(coupleh)
                                                End If
                                            Else
                                                coupleh.Message(e.Message.From, e.Message.Text)
                                            End If
                                        End If
                                    Catch ex As Exception
                                    End Try
                                Next
                            Catch ex As Exception
                            End Try
                        End If
                    End If
                Catch ex As Exception
                End Try
                If Not Coso Then
                    If Not UserSetup.IsSetupComplete(Author, Author.Id) Then
                        UserSetup.StartSetup(Author, e)
                    Else
                        If e.Message.Text.StartsWith(CommandExecutor.GetTrigger()) And Not QueueManager.GetWaitingUsers1().Contains(e.Message.From) And Not QueueManager.GetMatchedUsers1().Contains(e.Message.From) Then
                            CommandExecutor.ExecuteCommand(e.Message.Text.Substring(CommandExecutor.GetTrigger().Length, e.Message.Text.Length - CommandExecutor.GetTrigger().Length), e)
                        Else
                            If QueueManager.GetMatchedUsers1().Contains(e.Message.From) Then
                                If Not System.IO.File.Exists(Application.StartupPath & "\setup1\" + AuthorID.ToString() + "\logs.txt") Then
                                    System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + AuthorID.ToString() + "\logs.txt", "[" + DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString() + "] " + e.Message.Text)
                                Else
                                    System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + AuthorID.ToString() + "\logs.txt", System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + AuthorID.ToString() + "\logs.txt") + vbNewLine + "[" + DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString() + "] " + e.Message.Text)
                                End If
                                If Not System.IO.File.Exists(Application.StartupPath & "\bot\logs1.txt") Then
                                    System.IO.File.WriteAllText(Application.StartupPath & "\bot\logs1.txt", "[" + DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString() + "] " + AuthorID.ToString() + ": " + e.Message.Text)
                                Else
                                    System.IO.File.WriteAllText(Application.StartupPath & "\bot\logs1.txt", System.IO.File.ReadAllText(Application.StartupPath & "\bot\logs1.txt") + vbNewLine + "[" + DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString() + "] " + AuthorID.ToString() + ": " + e.Message.Text)
                                End If
                                For Each coupleh As Couple In QueueManager.GetCouples()
                                    If coupleh.GetUser3().Id = Author.Id Or coupleh.GetUser4().Id = Author.Id Then
                                        coupleh.Message(Author, e.Message.Text)
                                    End If
                                Next
                            ElseIf QueueManager.GetWaitingUsers1().Contains(e.Message.From) Then
                                If e.Message.Text.Replace(" ", "").ToLower().Equals(CommandExecutor.GetTrigger() + "stop") Then
                                    QueueManager.GetWaitingUsers1().Remove(Author)
                                    Telegram.SendTextMessageAsync(AuthorID, "You have been succesfully removed from the waiting queue.")
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub
End Class