Imports Discord.WebSocket
Imports Discord
Public Class DiscordBot
    Private Shared WithEvents Discord As DiscordSocketClient
    Private Shared Token As String
    Public Shared Function GetDiscord() As DiscordSocketClient
        Return Discord
    End Function
    Public Shared Sub SetDiscord(ByVal tDiscord As DiscordSocketClient)
        Discord = tDiscord
    End Sub
    Public Shared Function GetToken() As String
        Return Token
    End Function
    Public Shared Sub SetToken(ByVal tToken As String)
        Token = tToken
    End Sub
    Public Shared Async Sub SetupDiscordBot()
        Discord = New DiscordSocketClient(New DiscordSocketConfig() With
                                          {
                                          .WebSocketProvider = Net.WebSockets.DefaultWebSocketProvider.Instance,
                                          .UdpSocketProvider = Net.Udp.DefaultUdpSocketProvider.Instance,
                                          .MessageCacheSize = 50
                                          })
        Await Discord.LoginAsync(TokenType.Bot, GetToken(), True).ConfigureAwait(False)
        Await Discord.StartAsync().ConfigureAwait(False)
        Await Discord.SetStatusAsync(UserStatus.Online).ConfigureAwait(False)
    End Sub
    Public Shared Async Sub Disconnect()
        Await Discord.SetGameAsync("", Nothing, ActivityType.Playing).ConfigureAwait(False)
        Await Discord.SetStatusAsync(UserStatus.Offline).ConfigureAwait(False)
        Await Discord.LogoutAsync().ConfigureAwait(False)
        Await Discord.StopAsync().ConfigureAwait(False)
    End Sub
    Public Shared Function Discord_MessageReceived(arg As SocketMessage) As Task Handles Discord.MessageReceived
        Dim Author As SocketUser = arg.Author
        Dim AuthorID As ULong = Author.Id
        If Not AuthorID = Discord.CurrentUser.Id Then
            Dim Coso As Boolean = False
            Dim CanProceed As Boolean = True
            Try
                If BlockedWords.ContainsBlockedWord(arg.Content) And Not UserVariables.GetRole(Author.Id) = "Gold" And Not UserVariables.GetRole(Author.Id) = "Mod" And Not UserVariables.GetRole(Author.Id) = "Admin" Then
                    UserVariables.SetBanned(AuthorID, True)
                    If UserVariables.GetLanguage(AuthorID).Equals("eng") Then
                        UserVariables.SetBanReason(AuthorID, "Usage of blocked words")
                    ElseIf UserVariables.GetLanguage(AuthorID).Equals("ita") Then
                        UserVariables.SetBanReason(AuthorID, "Uso di parole bloccate")
                    End If
                    If UserVariables.GetLanguage(AuthorID).Equals("eng") Then
                        Author.SendMessageAsync("You have been banned automatically from the bot! Reason: " + UserVariables.GetBanReason(AuthorID))
                    ElseIf UserVariables.GetLanguage(AuthorID).Equals("ita") Then
                        Author.SendMessageAsync("Sei stato bannato automaticamente dal bot! Motivo: " + UserVariables.GetBanReason(AuthorID))
                    End If
                End If
            Catch ex As Exception
            End Try
            Try
                If UserVariables.GetReports(Author.Id) >= 3 Then
                    UserVariables.SetReports(AuthorID, 0)
                    If UserVariables.GetLanguage(AuthorID).Equals("eng") Then
                        UserVariables.SetBanReason(AuthorID, "Bad behaviour")
                    ElseIf UserVariables.GetLanguage(AuthorID).Equals("ita") Then
                        UserVariables.SetBanReason(AuthorID, "Comportamento scorretto")
                    End If
                    If UserVariables.GetLanguage(AuthorID).Equals("eng") Then
                        Author.SendMessageAsync("You have been banned automatically from the bot! Reason: " + UserVariables.GetBanReason(AuthorID))
                    ElseIf UserVariables.GetLanguage(AuthorID).Equals("ita") Then
                        Author.SendMessageAsync("Sei stato bannato automaticamente dal bot! Motivo: " + UserVariables.GetBanReason(AuthorID))
                    End If
                End If
            Catch ex As Exception
            End Try
            Try
                If UserVariables.GetBanned(AuthorID) And Not UserVariables.GetRole(AuthorID) = "Gold" And Not UserVariables.GetRole(AuthorID) = "Mod" And Not UserVariables.GetRole(AuthorID) = "Admin" Then
                    If UserVariables.GetLanguage(AuthorID).Equals("eng") Then
                        Author.SendMessageAsync("You are banned from the bot! Reason: " + UserVariables.GetBanReason(AuthorID))
                    ElseIf UserVariables.GetLanguage(AuthorID).Equals("ita") Then
                        Author.SendMessageAsync("Sei stato bannato dal bot! Motivo: " + UserVariables.GetBanReason(AuthorID))
                    End If
                    CanProceed = False
                    If QueueManager.GetMatchedUsers().Contains(Author) Then
                        QueueManager.GetMatchedUsers().Remove(Author)
                    End If
                    If QueueManager.GetWaitingUsers().Contains(Author) Then
                        QueueManager.GetWaitingUsers().Remove(Author)
                    End If
                    For Each coupleh As Couple In QueueManager.GetCouples()
                        If coupleh.GetUser1().Id = Author.Id Or coupleh.GetUser2().Id = Author.Id Then
                            If coupleh.GetUser1().Id = Author.Id Then
                                If UserVariables.GetLanguage(coupleh.GetUser2().Id).Equals("eng") Then
                                    coupleh.GetUser2().SendMessageAsync("The user that you were talking with is now banned. Do you want to talk with another stranger?")
                                ElseIf UserVariables.GetLanguage(coupleh.GetUser2().Id).Equals("ita") Then
                                    coupleh.GetUser2().SendMessageAsync("L'utente con cui stavi parlando è stato bannato. Vuoi connetterti con un'altra persona?")
                                End If
                            ElseIf coupleh.GetUser2().Id = Author.Id Then
                                If UserVariables.GetLanguage(coupleh.GetUser1().Id).Equals("eng") Then
                                    coupleh.GetUser1().SendMessageAsync("The user that you were talking with is now banned. Do you want to talk with another stranger?")
                                ElseIf UserVariables.GetLanguage(coupleh.GetUser1().Id).Equals("ita") Then
                                    coupleh.GetUser1().SendMessageAsync("L'utente con cui stavi parlando è stato bannato. Vuoi connetterti con un'altra persona?")
                                End If
                            End If
                            If QueueManager.GetMatchedUsers().Contains(coupleh.GetUser2()) Then
                                QueueManager.GetMatchedUsers().Remove(coupleh.GetUser2())
                            End If
                            If QueueManager.GetMatchedUsers().Contains(coupleh.GetUser1()) Then
                                QueueManager.GetMatchedUsers().Remove(coupleh.GetUser1())
                            End If
                            If QueueManager.GetMatchedUsers().Contains(Author) Then
                                QueueManager.GetMatchedUsers().Remove(Author)
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
                If QueueManager.GetMatchedUsers().Contains(arg.Author) Then
                    If arg.Content.Replace(" ", "").ToLower().Equals(CommandExecutor.GetTrigger() + "stop") Then
                        Try
                            For Each coupleh As Couple In QueueManager.GetCouples()
                                Try
                                    If coupleh.GetUser1().Id = arg.Author.Id Or coupleh.GetUser2().Id = arg.Author.Id Then
                                        If arg.Content.Replace(" ", "").ToLower().Equals(CommandExecutor.GetTrigger() + "stop") Then
                                            Coso = True
                                            If UserVariables.GetLanguage(Author.Id).Equals("eng") Then
                                                Author.SendMessageAsync("You have been disconnected from your stranger. Do you want to connect with another stranger?")
                                            ElseIf UserVariables.GetLanguage(Author.Id).Equals("ita") Then
                                                Author.SendMessageAsync("Ti sei disconnesso dal tuo sconosciuto. Vuoi connetterti con un'altra persona?")
                                            End If
                                            If coupleh.GetUser1().Id = arg.Author.Id Then
                                                If UserVariables.GetLanguage(coupleh.GetUser2().Id).Equals("eng") Then
                                                    coupleh.GetUser2().SendMessageAsync("Your stranger has been disconnected from the chat. Do you want to get connected with another stranger?")
                                                ElseIf UserVariables.GetLanguage(coupleh.GetUser2().Id).Equals("ita") Then
                                                    coupleh.GetUser2().SendMessageAsync("Il tuo sconosciuto si è disconnesso dalla chat. Vuoi connetterti con un'altra persona?")
                                                End If
                                            ElseIf coupleh.GetUser2().Id = arg.Author.Id Then
                                                If UserVariables.GetLanguage(coupleh.GetUser1().Id).Equals("eng") Then
                                                    coupleh.GetUser1().SendMessageAsync("Your stranger has been disconnected from the chat. Do you want to get connected with another stranger?")
                                                ElseIf UserVariables.GetLanguage(coupleh.GetUser1().Id).Equals("ita") Then
                                                    coupleh.GetUser1().SendMessageAsync("Il tuo sconosciuto si è disconnesso dalla chat. Vuoi connetterti con un'altra persona?")
                                                End If
                                            End If
                                            If QueueManager.GetMatchedUsers().Contains(coupleh.GetUser2()) Then
                                                QueueManager.GetMatchedUsers().Remove(coupleh.GetUser2())
                                            End If
                                            If QueueManager.GetMatchedUsers().Contains(coupleh.GetUser1()) Then
                                                QueueManager.GetMatchedUsers().Remove(coupleh.GetUser1())
                                            End If
                                            If QueueManager.GetMatchedUsers().Contains(Author) Then
                                                QueueManager.GetMatchedUsers().Remove(Author)
                                            End If
                                            If QueueManager.GetCouples().Contains(coupleh) Then
                                                QueueManager.GetCouples().Remove(coupleh)
                                            End If
                                        Else
                                            coupleh.Message(arg.Author, arg.Content)
                                        End If
                                    End If
                                Catch ex As Exception
                                End Try
                            Next
                        Catch ex As Exception
                        End Try
                    ElseIf arg.Content.Replace(" ", "").ToLower().Equals(CommandExecutor.GetTrigger() + "report") Then
                        Try
                            For Each coupleh As Couple In QueueManager.GetCouples()
                                Try
                                    If coupleh.GetUser1().Id = arg.Author.Id Or coupleh.GetUser2().Id = arg.Author.Id Then
                                        If arg.Content.Replace(" ", "").ToLower().Equals(CommandExecutor.GetTrigger() + "report") Then
                                            Coso = True
                                            If UserVariables.GetLanguage(AuthorID).Equals("eng") Then
                                                Author.SendMessageAsync("This user has been reported, thanks for your support. Do you want to connect with another stranger?")
                                            ElseIf UserVariables.GetLanguage(AuthorID).Equals("ita") Then
                                                Author.SendMessageAsync("Questo utente è stato segnalato, grazie per il tuo supporto. Vuoi connetterti con un'altra persona?")
                                            End If
                                            If coupleh.GetUser1().Id = arg.Author.Id Then
                                                Dim Reports As Integer = UserVariables.GetReports(coupleh.GetUser2().Id) + 1
                                                UserVariables.SetReports(coupleh.GetUser2().Id, Reports)
                                                If UserVariables.GetLanguage(coupleh.GetUser2().Id).Equals("eng") Then
                                                    coupleh.GetUser2().SendMessageAsync("Your stranger has been disconnected from the chat. Do you want to get connected with another stranger?")
                                                ElseIf UserVariables.GetLanguage(coupleh.GetUser2().Id).Equals("ita") Then
                                                    coupleh.GetUser2().SendMessageAsync("Il tuo sconosciuto si è disconnesso dalla chat. Vuoi connetterti con un'altra persona?")
                                                End If
                                            ElseIf coupleh.GetUser2().Id = arg.Author.Id Then
                                                Dim Reports As Integer = UserVariables.GetReports(coupleh.GetUser1().Id) + 1
                                                UserVariables.SetReports(coupleh.GetUser1().Id, Reports)
                                                If UserVariables.GetLanguage(coupleh.GetUser1().Id).Equals("eng") Then
                                                    coupleh.GetUser1().SendMessageAsync("Your stranger has been disconnected from the chat. Do you want to get connected with another stranger?")
                                                ElseIf UserVariables.GetLanguage(coupleh.GetUser1().Id).Equals("ita") Then
                                                    coupleh.GetUser1().SendMessageAsync("Il tuo sconosciuto si è disconnesso dalla chat. Vuoi connetterti con un'altra persona?")
                                                End If
                                            End If
                                            If QueueManager.GetMatchedUsers().Contains(coupleh.GetUser2()) Then
                                                QueueManager.GetMatchedUsers().Remove(coupleh.GetUser2())
                                            End If
                                            If QueueManager.GetMatchedUsers().Contains(coupleh.GetUser1()) Then
                                                QueueManager.GetMatchedUsers().Remove(coupleh.GetUser1())
                                            End If
                                            If QueueManager.GetMatchedUsers().Contains(Author) Then
                                                QueueManager.GetMatchedUsers().Remove(Author)
                                            End If
                                            If QueueManager.GetCouples().Contains(coupleh) Then
                                                QueueManager.GetCouples().Remove(coupleh)
                                            End If
                                        Else
                                            coupleh.Message(arg.Author, arg.Content)
                                        End If
                                    End If
                                Catch ex As Exception
                                End Try
                            Next
                        Catch ex As Exception
                        End Try
                    End If
                End If
                If Not Coso Then
                    If Not UserSetup.IsSetupComplete(Author, AuthorID) Then
                        UserSetup.StartSetup(Author, arg)
                    Else
                        If arg.Content.StartsWith(CommandExecutor.GetTrigger()) And arg.Channel.Name.Contains("@" + Author.Username + "#") And Not QueueManager.GetWaitingUsers().Contains(arg.Author) And Not QueueManager.GetMatchedUsers().Contains(arg.Author) Then
                            CommandExecutor.ExecuteCommand(arg.Content.Substring(CommandExecutor.GetTrigger().Length, arg.Content.Length - CommandExecutor.GetTrigger().Length), arg)
                        Else
                            If QueueManager.GetMatchedUsers().Contains(arg.Author) Then
                                If Not System.IO.File.Exists(Application.StartupPath & "\setup\" + AuthorID.ToString() + "\logs.txt") Then
                                    System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + AuthorID.ToString() + "\logs.txt", "[" + DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString() + "] " + arg.Content)
                                Else
                                    System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + AuthorID.ToString() + "\logs.txt", System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + AuthorID.ToString() + "\logs.txt") + vbNewLine + "[" + DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString() + "] " + arg.Content)
                                End If
                                If Not System.IO.File.Exists(Application.StartupPath & "\bot\logs.txt") Then
                                    System.IO.File.WriteAllText(Application.StartupPath & "\bot\logs.txt", "[" + DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString() + "] " + AuthorID.ToString() + ": " + arg.Content)
                                Else
                                    System.IO.File.WriteAllText(Application.StartupPath & "\bot\logs.txt", System.IO.File.ReadAllText(Application.StartupPath & "\bot\logs.txt") + vbNewLine + "[" + DateTime.Now.ToShortDateString() + "-" + DateTime.Now.ToLongTimeString() + "] " + AuthorID.ToString() + ": " + arg.Content)
                                End If
                                For Each coupleh As Couple In QueueManager.GetCouples()
                                    If coupleh.GetUser1().Id = Author.Id Or coupleh.GetUser2().Id = Author.Id Then
                                        coupleh.Message(Author, arg.Content)
                                    End If
                                Next
                            ElseIf QueueManager.GetWaitingUsers().Contains(arg.Author) Then
                                If arg.Content.Replace(" ", "").ToLower().Equals(CommandExecutor.GetTrigger() + "stop") Then
                                    QueueManager.GetWaitingUsers().Remove(Author)
                                    If UserVariables.GetLanguage(AuthorID).Equals("eng") Then
                                        Author.SendMessageAsync("You have been succesfully removed from the waiting queue.")
                                    ElseIf UserVariables.GetLanguage(AuthorID).Equals("ita") Then
                                        Author.SendMessageAsync("Sei stato rimosso con successo dalla coda d'attesa.")
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Function
End Class