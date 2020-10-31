Imports Discord.WebSocket
Imports Discord
Public Class QueueManager
    Private Shared WaitingUsers As List(Of SocketUser) = New List(Of SocketUser)
    Private Shared MatchedUsers As List(Of SocketUser) = New List(Of SocketUser)
    Private Shared WaitingUsers1 As List(Of Telegram.Bot.Types.User) = New List(Of Telegram.Bot.Types.User)
    Private Shared MatchedUsers1 As List(Of Telegram.Bot.Types.User) = New List(Of Telegram.Bot.Types.User)
    Private Shared Couples As List(Of Couple) = New List(Of Couple)
    Public Shared Function GetWaitingUsers() As List(Of SocketUser)
        Return WaitingUsers
    End Function
    Public Shared Function GetMatchedUsers() As List(Of SocketUser)
        Return MatchedUsers
    End Function
    Public Shared Function GetWaitingUsers1() As List(Of Telegram.Bot.Types.User)
        Return WaitingUsers1
    End Function
    Public Shared Function GetMatchedUsers1() As List(Of Telegram.Bot.Types.User)
        Return MatchedUsers1
    End Function
    Public Shared Function GetCouples() As List(Of Couple)
        Return Couples
    End Function
    Public Shared Sub CreateChats()
        If GetWaitingUsers().Count > 1 Then
            Dim Random As New Random
            Dim A As Integer = Random.Next(0, GetWaitingUsers().Count)
            Dim B As Integer = GetWaitingUsers().Count - 1
            If Not A = B Then
                Dim A1 As SocketUser = WaitingUsers(A)
                Dim B1 As SocketUser = WaitingUsers(B)
                Dim CanProceed As Boolean = True
                If UserVariables.GetOnlyGirls(A1.Id) And Not UserVariables.GetGender(B1.Id).ToLower().Equals("female") Then
                    CanProceed = False
                End If
                If UserVariables.GetOnlyGirls(B1.Id) And Not UserVariables.GetGender(A1.Id).ToLower().Equals("female") Then
                    CanProceed = False
                End If
                If Not UserVariables.GetLanguage(A1.Id) = UserVariables.GetLanguage(B1.Id) Then
                    CanProceed = False
                End If
                If CanProceed Then
                    If Not MatchedUsers.Contains(A1) And Not MatchedUsers.Contains(B1) Then
                        WaitingUsers.Remove(A1)
                        WaitingUsers.Remove(B1)
                        MatchedUsers.Add(A1)
                        MatchedUsers.Add(B1)
                        Dim couple As Couple = New Couple(A1, B1)
                        Couples.Add(couple)
                        If UserVariables.GetLanguage(A1.Id).Equals("eng") Then
                            A1.SendMessageAsync("You have been matched. Have fun!" + vbNewLine + vbNewLine + "Info about this user: " + vbNewLine +
                                            "Name: " + UserVariables.GetName(B1.Id) + vbNewLine +
                                            "Country: " + UserVariables.GetCountry(B1.Id) + vbNewLine +
                                            "Gender: " + UserVariables.GetGender(B1.Id) + vbNewLine +
                                            "Status: " + UserVariables.GetStatus(B1.Id) + vbNewLine +
                                            "Age: " + UserVariables.GetAge(B1.Id).ToString() + vbNewLine + vbNewLine +
                                            "If your stranger is assuming a bad or incorrect behaviour, please, report him and conclude the chat with the command " + CommandExecutor.GetTrigger() + "report")
                        ElseIf UserVariables.GetLanguage(A1.Id).Equals("ita") Then
                            Dim Sex As String = ""
                            If UserVariables.GetGender(B1.Id).Equals("male") Then
                                Sex = "Maschio"
                            ElseIf UserVariables.GetGender(B1.Id).Equals("female") Then
                                Sex = "Femmina"
                            End If
                            A1.SendMessageAsync("Ti sei connesso con uno sconosciuto. Buon divertimento!" + vbNewLine + vbNewLine + "Informazioni su questo utente: " + vbNewLine +
                 "Nome: " + UserVariables.GetName(B1.Id) + vbNewLine +
                "Paese: " + UserVariables.GetCountry(B1.Id) + vbNewLine +
                "Sesso: " + Sex + vbNewLine +
                "Stato: " + UserVariables.GetStatus(B1.Id) + vbNewLine +
                "Età: " + UserVariables.GetAge(B1.Id).ToString() + vbNewLine + vbNewLine +
                "Se la persona con cui stai parlando sta assumendo un comportamento scorretto, per favore, segnalalo e concludi la chat con il comando " + CommandExecutor.GetTrigger() + "report")
                        End If
                        If UserVariables.GetLanguage(B1.Id).Equals("eng") Then
                            B1.SendMessageAsync("You have been matched. Have fun!" + vbNewLine + vbNewLine + "Info about this user: " + vbNewLine +
                                            "Name: " + UserVariables.GetName(A1.Id) + vbNewLine +
                                            "Country: " + UserVariables.GetCountry(A1.Id) + vbNewLine +
                                            "Gender: " + UserVariables.GetGender(A1.Id) + vbNewLine +
                                            "Status: " + UserVariables.GetStatus(A1.Id) + vbNewLine +
                                            "Age: " + UserVariables.GetAge(A1.Id).ToString() + vbNewLine + vbNewLine +
                                            "If your stranger is assuming a bad or incorrect behaviour, please, report him and conclude the chat with the command " + CommandExecutor.GetTrigger() + "report")
                        ElseIf UserVariables.GetLanguage(B1.Id).Equals("ita") Then
                            Dim Sex As String = ""
                            If UserVariables.GetGender(A1.Id).Equals("male") Then
                                Sex = "Maschio"
                            ElseIf UserVariables.GetGender(A1.Id).Equals("female") Then
                                Sex = "Femmina"
                            End If
                            B1.SendMessageAsync("Ti sei connesso con uno sconosciuto. Buon divertimento!" + vbNewLine + vbNewLine + "Informazioni su questo utente: " + vbNewLine +
                 "Nome: " + UserVariables.GetName(A1.Id) + vbNewLine +
                "Paese: " + UserVariables.GetCountry(A1.Id) + vbNewLine +
                "Sesso: " + Sex + vbNewLine +
                "Stato: " + UserVariables.GetStatus(A1.Id) + vbNewLine +
                "Età: " + UserVariables.GetAge(A1.Id).ToString() + vbNewLine + vbNewLine +
                "Se la persona con cui stai parlando sta assumendo un comportamento scorretto, per favore, segnalalo e concludi la chat con il comando " + CommandExecutor.GetTrigger() + "report")
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Public Shared Sub CreateChats1()
        If GetWaitingUsers1().Count > 1 Then
            Dim Random As New Random
            Dim A As Integer = Random.Next(0, GetWaitingUsers1().Count)
            Dim B As Integer = GetWaitingUsers1().Count - 1
            If Not A = B Then
                Dim A1 As Telegram.Bot.Types.User = WaitingUsers1(A)
                Dim B1 As Telegram.Bot.Types.User = WaitingUsers1(B)
                Dim CanProceed As Boolean = True
                If UserVariables.GetOnlyGirls(A1.Id) And Not UserVariables.GetGender(B1.Id).ToLower().Equals("female") Then
                    CanProceed = False
                End If
                If UserVariables.GetOnlyGirls(B1.Id) And Not UserVariables.GetGender(A1.Id).ToLower().Equals("female") Then
                    CanProceed = False
                End If
                If Not UserVariables.GetLanguage(A1.Id) = UserVariables.GetLanguage(B1.Id) Then
                    CanProceed = False
                End If
                If CanProceed Then
                    If Not MatchedUsers1.Contains(A1) And Not MatchedUsers1.Contains(B1) Then
                        WaitingUsers1.Remove(A1)
                        WaitingUsers1.Remove(B1)
                        MatchedUsers1.Add(A1)
                        MatchedUsers1.Add(B1)
                        Dim couple As Couple = New Couple(A1, B1)
                        Couples.Add(couple)
                        If UserVariables.GetLanguage(A1.Id).Equals("eng") Then
                            TelegramBot.GetTelegram().SendTextMessageAsync(A1.Id, "You have been matched. Have fun!" + vbNewLine + vbNewLine + "Info about this user: " + vbNewLine +
                                            "Name: " + UserVariables.GetName(B1.Id) + vbNewLine +
                                            "Country: " + UserVariables.GetCountry(B1.Id) + vbNewLine +
                                            "Gender: " + UserVariables.GetGender(B1.Id) + vbNewLine +
                                            "Status: " + UserVariables.GetStatus(B1.Id) + vbNewLine +
                                            "Age: " + UserVariables.GetAge(B1.Id).ToString() + vbNewLine + vbNewLine +
                                            "If your stranger is assuming a bad or incorrect behaviour, please, report him and conclude the chat with the command " + CommandExecutor.GetTrigger() + "report")
                        ElseIf UserVariables.GetLanguage(A1.Id).Equals("ita") Then
                            Dim Sex As String = ""
                            If UserVariables.GetGender(B1.Id).Equals("male") Then
                                Sex = "Maschio"
                            ElseIf UserVariables.GetGender(B1.Id).Equals("female") Then
                                Sex = "Femmina"
                            End If
                            TelegramBot.GetTelegram().SendTextMessageAsync(A1.Id, "Ti sei connesso con uno sconosciuto. Buon divertimento!" + vbNewLine + vbNewLine + "Informazioni su questo utente: " + vbNewLine +
                 "Nome: " + UserVariables.GetName(B1.Id) + vbNewLine +
                "Paese: " + UserVariables.GetCountry(B1.Id) + vbNewLine +
                "Sesso: " + Sex + vbNewLine +
                "Stato: " + UserVariables.GetStatus(B1.Id) + vbNewLine +
                "Età: " + UserVariables.GetAge(B1.Id).ToString() + vbNewLine + vbNewLine +
                "Se la persona con cui stai parlando sta assumendo un comportamento scorretto, per favore, segnalalo e concludi la chat con il comando " + CommandExecutor.GetTrigger() + "report")
                        End If
                        If UserVariables.GetLanguage(B1.Id).Equals("eng") Then
                            TelegramBot.GetTelegram().SendTextMessageAsync(B1.Id, "You have been matched. Have fun!" + vbNewLine + vbNewLine + "Info about this user: " + vbNewLine +
                                            "Name: " + UserVariables.GetName(A1.Id) + vbNewLine +
                                            "Country: " + UserVariables.GetCountry(A1.Id) + vbNewLine +
                                            "Gender: " + UserVariables.GetGender(A1.Id) + vbNewLine +
                                            "Status: " + UserVariables.GetStatus(A1.Id) + vbNewLine +
                                            "Age: " + UserVariables.GetAge(A1.Id).ToString() + vbNewLine + vbNewLine +
                                            "If your stranger is assuming a bad or incorrect behaviour, please, report him and conclude the chat with the command " + CommandExecutor.GetTrigger() + "report")
                        ElseIf UserVariables.GetLanguage(B1.Id).Equals("ita") Then
                            Dim Sex As String = ""
                            If UserVariables.GetGender(A1.Id).Equals("male") Then
                                Sex = "Maschio"
                            ElseIf UserVariables.GetGender(A1.Id).Equals("female") Then
                                Sex = "Femmina"
                            End If
                            TelegramBot.GetTelegram().SendTextMessageAsync(B1.Id, "Ti sei connesso con uno sconosciuto. Buon divertimento!" + vbNewLine + vbNewLine + "Informazioni su questo utente: " + vbNewLine +
                 "Nome: " + UserVariables.GetName(A1.Id) + vbNewLine +
                "Paese: " + UserVariables.GetCountry(A1.Id) + vbNewLine +
                "Sesso: " + Sex + vbNewLine +
                "Stato: " + UserVariables.GetStatus(A1.Id) + vbNewLine +
                "Età: " + UserVariables.GetAge(A1.Id).ToString() + vbNewLine + vbNewLine +
                "Se la persona con cui stai parlando sta assumendo un comportamento scorretto, per favore, segnalalo e concludi la chat con il comando " + CommandExecutor.GetTrigger() + "report")
                        End If
                    End If
                End If
            End If
        End If
    End Sub
End Class