Imports Discord.WebSocket
Imports Discord
Public Class UserSetup
    Public Shared Sub SetSetupStatus(ByVal UserID As ULong, ByVal SetupStatus As Integer)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\setup_status.txt", SetupStatus.ToString())
    End Sub
    Public Shared Function GetSetupStatus(ByVal UserID As ULong) As Integer
        Return Int32.Parse(System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\setup_status.txt"))
    End Function
    Public Shared Function IsSetupComplete(ByVal Author As SocketUser, ByVal UserID As ULong) As Boolean
        Dim Complete As Boolean = True
        If Not System.IO.Directory.Exists(Application.StartupPath & "\setup") Then
            Complete = False
        End If
        If Not System.IO.Directory.Exists(Application.StartupPath & "\setup\" + UserID.ToString()) Then
            System.IO.Directory.CreateDirectory(Application.StartupPath & "\setup\" + UserID.ToString())
            Complete = False
        End If
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\username.txt", Author.Username)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\mention.txt", Author.Mention)
        If Not System.IO.File.Exists(Application.StartupPath & "\setup\" + UserID.ToString() + "\setup_status.txt") Then
            System.IO.File.WriteAllText(Application.StartupPath & "\setup\" + UserID.ToString() + "\setup_status.txt", "1")
            Complete = False
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup\" + UserID.ToString() + "\reports.txt") Then
            UserVariables.SetReports(UserID, 0)
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup\" + UserID.ToString() + "\only_girls.txt") Then
            UserVariables.SetOnlyGirls(UserID, False)
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup\" + UserID.ToString() + "\banned.txt") Then
            UserVariables.SetBanned(UserID, False)
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup\" + UserID.ToString() + "\ban_reason.txt") Then
            UserVariables.SetBanReason(UserID, "")
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup\" + UserID.ToString() + "\role.txt") Then
            UserVariables.SetRole(UserID, "User")
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup\" + UserID.ToString() + "\language.txt") Then
            UserVariables.SetLanguage(UserID, "ENG")
        End If
        If Not GetSetupStatus(UserID) = 0 Then
            Complete = False
        End If
        Return Complete
    End Function
    Public Shared Sub StartSetup(ByVal Author As SocketUser, ByVal Message As SocketMessage)
        If GetSetupStatus(Author.Id) = 1 Then
            If UserVariables.GetLanguage(Author.Id).Equals("eng") Then
                Author.SendMessageAsync("Welcome to Omehall, " + Author.Username + "!" + vbNewLine +
                                "To get started on Omehall, we need to know something about yourself. Let's do it!" + vbNewLine + vbNewLine +
                                "What's your real name?")
            ElseIf UserVariables.GetLanguage(Author.Id).Equals("ita") Then
                Author.SendMessageAsync("Benvenuto su Omehall " + Author.Username + "!" + vbNewLine +
                                "Per iniziare ad usare Omehall, dobbiamo conoscere qualcosa su di te. Facciamolo!" + vbNewLine + vbNewLine +
                                "Come ti chiami?")
            End If
            SetSetupStatus(Author.Id, 2)
        End If
        If Message.Channel.Name.Contains("@" + Author.Username + "#") Then
            If GetSetupStatus(Author.Id) = 2 Then
                If Message.Content.Length < 16 Then
                    UserVariables.SetName(Author.Id, Message.Content)
                    SetSetupStatus(Author.Id, 3)
                    If UserVariables.GetLanguage(Author.Id).Equals("eng") Then
                        Author.SendMessageAsync("Ok, " + UserVariables.GetName(Author.Id) + ". Now, how old are you?")
                    ElseIf UserVariables.GetLanguage(Author.Id).Equals("ita") Then
                        Author.SendMessageAsync("Ok, " + UserVariables.GetName(Author.Id) + ". Adesso, quanti anni hai?")
                    End If
                Else
                    If UserVariables.GetLanguage(Author.Id).Equals("eng") Then
                        Author.SendMessageAsync("Name needs to be with a length lower of 16 chars.")
                    ElseIf UserVariables.GetLanguage(Author.Id).Equals("ita") Then
                        Author.SendMessageAsync("Il nome deve avere una lunghezza inferiore di 16 caratteri.")
                    End If
                End If
            ElseIf GetSetupStatus(Author.Id) = 3 Then
                Try
                    UserVariables.SetAge(Author.Id, Int32.Parse(Message.Content))
                Catch ex As Exception
                    If UserVariables.GetLanguage(Author.Id).Equals("eng") Then
                        Author.SendMessageAsync(Author.Id, "The age you inserted is not valid! Please, re-try.")
                    ElseIf UserVariables.GetLanguage(Author.Id).Equals("ita") Then
                        Author.SendMessageAsync(Author.Id, "L'età che hai inserito non è valida! Per favore, riprova.")
                    End If
                End Try
                SetSetupStatus(Author.Id, 4)
                If UserVariables.GetLanguage(Author.Id).Equals("eng") Then
                    Author.SendMessageAsync("Ok, and where are you from?")
                ElseIf UserVariables.GetLanguage(Author.Id).Equals("ita") Then
                    Author.SendMessageAsync("Ok, e di dove sei?")
                End If
            ElseIf GetSetupStatus(Author.Id) = 4 Then
                If Message.Content.Length < 10 Then
                    UserVariables.SetCountry(Author.Id, Message.Content)
                    SetSetupStatus(Author.Id, 5)
                    If UserVariables.GetLanguage(Author.Id).Equals("eng") Then
                        Author.SendMessageAsync("Ok, are you single or you have a boyfriend/girlfriend?")
                    ElseIf UserVariables.GetLanguage(Author.Id).Equals("ita") Then
                        Author.SendMessageAsync("Ok, sei single o hai un ragazzo o una ragazza?")
                    End If
                Else
                    If UserVariables.GetLanguage(Author.Id).Equals("eng") Then
                        Author.SendMessageAsync("Country needs to be with a length lower of 10 chars.")
                    ElseIf UserVariables.GetLanguage(Author.Id).Equals("ita") Then
                        Author.SendMessageAsync("La nazionalità deve avere una lunghezza inferiore di 10 caratteri.")
                    End If
                End If
            ElseIf GetSetupStatus(Author.Id) = 5 Then
                If Message.Content.Length < 50 Then
                    UserVariables.SetStatus(Author.Id, Message.Content)
                    SetSetupStatus(Author.Id, 6)
                    If UserVariables.GetLanguage(Author.Id).Equals("eng") Then
                        Author.SendMessageAsync("Ok, are you male or female?")
                    ElseIf UserVariables.GetLanguage(Author.Id).Equals("ita") Then
                        Author.SendMessageAsync("Ok, sei maschio o femmina?")
                    End If
                Else
                    If UserVariables.GetLanguage(Author.Id).Equals("eng") Then
                        Author.SendMessageAsync("Status needs to be with a length lower of 50 chars.")
                    ElseIf UserVariables.GetLanguage(Author.Id).Equals("ita") Then
                        Author.SendMessageAsync("Lo stato deve avere una lunghezza inferiore di 50 caratteri.")
                    End If
                End If
            ElseIf GetSetupStatus(Author.Id) = 6 Then
                If Message.Content.ToLower().Equals("male") Or Message.Content.ToLower().Equals("female") Then
                    UserVariables.SetGender(Author.Id, Message.Content)
                    SetSetupStatus(Author.Id, 7)
                    Author.SendMessageAsync("Are you Italian or English/other lang? With this, you can set the language used by the bot for you, so pay attention on what you write. Use ITA for Italian language, ENG For English/other language.")
                Else
                    Author.SendMessageAsync("Invalid gender, please insert male or female.")
                End If
            ElseIf GetSetupStatus(Author.Id) = 7 Then
                If Message.Content.ToLower.Equals("ita") Or Message.Content.ToLower.Equals("eng") Then
                    UserVariables.SetLanguage(Author.Id, Message.Content)
                    SetSetupStatus(Author.Id, 0)
                    Author.SendMessageAsync("Ok, we finished the setup yet. Now you're ready to use Omehall. Get started with it." + vbNewLine + vbNewLine + "Type " + CommandExecutor.GetTrigger() + "help for a list of all commands.")
                    For Each d As String In System.IO.Directory.GetDirectories(Application.StartupPath & "\setup")
                        d = d.Replace(Application.StartupPath & "\setup", "")
                        d = d.Replace(" ", "")
                        d = d.Replace("\", "")
                        Dim ID As ULong = ULong.Parse(d)
                        If UserVariables.GetRole(ID) = "Admin" Then
                            DiscordBot.GetDiscord().GetUser(ID).SendMessageAsync("A new user has been subscribed to the bot! Info: " + vbNewLine + vbNewLine +
                                                                       "ID: " + Author.Id.ToString() + vbNewLine +
                                                                       "Age: " + UserVariables.GetAge(Author.Id).ToString() + vbNewLine +
                                        "Banned: " + UserVariables.GetBanned(Author.Id).ToString() + vbNewLine +
                                        "Ban reason: " + UserVariables.GetBanReason(Author.Id) + vbNewLine +
                                        "Country: " + UserVariables.GetCountry(Author.Id) + vbNewLine +
                                        "Gender: " + UserVariables.GetGender(Author.Id) + vbNewLine +
                                        "Name: " + UserVariables.GetName(Author.Id) + vbNewLine +
                                        "Only girls: " + UserVariables.GetOnlyGirls(Author.Id).ToString() + vbNewLine +
                                        "Role: " + UserVariables.GetRole(Author.Id) + vbNewLine +
                                        "Status: " + UserVariables.GetStatus(Author.Id) + vbNewLine +
                                        "Setup status: " + UserSetup.GetSetupStatus(Author.Id).ToString() + vbNewLine +
                                        "Username: " + System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + Author.Id.ToString() + "\username.txt") + vbNewLine +
                                        "Mention: " + System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + Author.Id.ToString() + "\mention.txt") + vbNewLine +
                                        "Reports: " + UserVariables.GetReports(Author.Id).ToString() + vbNewLine +
                                        "Language: " + UserVariables.GetLanguage(Author.Id))
                        End If
                    Next
                Else
                    Author.SendMessageAsync("Invalid language. Please, use ITA or ENG.")
                End If
            End If
        End If
    End Sub
    Public Shared Sub SetSetupStatus(ByVal UserID As Integer, ByVal SetupStatus As Integer)
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\setup_status.txt", SetupStatus.ToString())
    End Sub
    Public Shared Function GetSetupStatus(ByVal UserID As Integer) As Integer
        Return Int32.Parse(System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\setup_status.txt"))
    End Function
    Public Shared Function IsSetupComplete(ByVal Author As Telegram.Bot.Types.User, ByVal UserID As Integer) As Boolean
        Dim Complete As Boolean = True
        If Not System.IO.Directory.Exists(Application.StartupPath & "\setup1") Then
            Complete = False
        End If
        If Not System.IO.Directory.Exists(Application.StartupPath & "\setup1\" + UserID.ToString()) Then
            System.IO.Directory.CreateDirectory(Application.StartupPath & "\setup1\" + UserID.ToString())
            Complete = False
        End If
        System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\username.txt", Author.Username)
        If Not System.IO.File.Exists(Application.StartupPath & "\setup1\" + UserID.ToString() + "\setup_status.txt") Then
            System.IO.File.WriteAllText(Application.StartupPath & "\setup1\" + UserID.ToString() + "\setup_status.txt", "1")
            Complete = False
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup1\" + UserID.ToString() + "\reports.txt") Then
            UserVariables.SetReports(UserID, 0)
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup1\" + UserID.ToString() + "\only_girls.txt") Then
            UserVariables.SetOnlyGirls(UserID, False)
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup1\" + UserID.ToString() + "\banned.txt") Then
            UserVariables.SetBanned(UserID, False)
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup1\" + UserID.ToString() + "\ban_reason.txt") Then
            UserVariables.SetBanReason(UserID, "")
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup1\" + UserID.ToString() + "\role.txt") Then
            UserVariables.SetRole(UserID, "User")
        End If
        If Not System.IO.File.Exists(Application.StartupPath & "\setup1\" + UserID.ToString() + "\language.txt") Then
            UserVariables.SetLanguage(UserID, "ENG")
        End If
        If Not GetSetupStatus(UserID) = 0 Then
            Complete = False
        End If
        Return Complete
    End Function
    Public Shared Sub StartSetup(ByVal Author As Telegram.Bot.Types.User, ByVal Message As Telegram.Bot.Args.MessageEventArgs)
        If GetSetupStatus(Author.Id) = 1 Then
            TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Welcome to Omehall, " + Author.Username + "!" + vbNewLine +
                                "To get started on Omehall, we need to know something about yourself. Let's do it!" + vbNewLine + vbNewLine +
                                "What's your real name?")
            SetSetupStatus(Author.Id, 2)
        End If
        If GetSetupStatus(Author.Id) = 2 Then
            If Message.Message.Text.ToLower().Equals("/start") Then
                Return
            End If
            If Message.Message.Text.Length < 16 Then
                UserVariables.SetName(Author.Id, Message.Message.Text)
                SetSetupStatus(Author.Id, 3)
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Ok, " + UserVariables.GetName(Author.Id) + ". Now, how old are you?")
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Name needs to be with a minimum length of 16 chars.")
            End If
        ElseIf GetSetupStatus(Author.Id) = 3 Then
            Try
                UserVariables.SetAge(Author.Id, Int32.Parse(Message.Message.Text))
            Catch ex As Exception
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "The age you inserted is not valid! Please, re-try.")
            End Try
            SetSetupStatus(Author.Id, 4)
            TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Ok, and where are you from?")
        ElseIf GetSetupStatus(Author.Id) = 4 Then
            If Message.Message.Text.Length < 10 Then
                UserVariables.SetCountry(Author.Id, Message.Message.Text)
                SetSetupStatus(Author.Id, 5)
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Ok, are you single or you have a boyfriend/girlfriend?")
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Country needs to be with a minimum length of 10 chars.")
            End If
        ElseIf GetSetupStatus(Author.Id) = 5 Then
            If Message.Message.Text.Length < 50 Then
                UserVariables.SetStatus(Author.Id, Message.Message.Text)
                SetSetupStatus(Author.Id, 6)
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Ok, are you male or female?")
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Status needs to be with a minimum length of 50 chars.")
            End If
        ElseIf GetSetupStatus(Author.Id) = 6 Then
            If Message.Message.Text.ToLower().Equals("male") Or Message.Message.Text.ToLower().Equals("female") Then
                UserVariables.SetGender(Author.Id, Message.Message.Text)
                SetSetupStatus(Author.Id, 7)
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Are you Italian or English/other lang? With this, you can set the language used by the bot for you, so pay attention on what you write. Use ITA for Italian language, ENG for English/other language.")
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Invalid gender, please insert male or female.")
            End If
        ElseIf GetSetupStatus(Author.Id) = 7 Then
            If Message.Message.Text.ToLower.Equals("ita") Or Message.Message.Text.ToLower.Equals("eng") Then
                UserVariables.SetLanguage(Author.Id, Message.Message.Text)
                SetSetupStatus(Author.Id, 0)
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Ok, we finished the setup yet. Now you're ready to use Omehall. Get started with it." + vbNewLine + vbNewLine + "Type " + CommandExecutor.GetTrigger() + "help for a list of all commands.")
                For Each d As String In System.IO.Directory.GetDirectories(Application.StartupPath & "\setup1")
                    d = d.Replace(Application.StartupPath & "\setup1", "")
                    d = d.Replace(" ", "")
                    d = d.Replace("\", "")
                    Dim ID As Integer = Integer.Parse(d)
                    If UserVariables.GetRole(ID) = "Admin" Then
                        TelegramBot.GetTelegram().SendTextMessageAsync(ID, "A new user has been subscribed to the bot! Info: " + vbNewLine + vbNewLine +
                                                                       "ID: " + Author.Id.ToString() + vbNewLine +
                                                                       "Age: " + UserVariables.GetAge(Author.Id).ToString() + vbNewLine +
                                        "Banned: " + UserVariables.GetBanned(Author.Id).ToString() + vbNewLine +
                                        "Ban reason: " + UserVariables.GetBanReason(Author.Id) + vbNewLine +
                                        "Country: " + UserVariables.GetCountry(Author.Id) + vbNewLine +
                                        "Gender: " + UserVariables.GetGender(Author.Id) + vbNewLine +
                                        "Name: " + UserVariables.GetName(Author.Id) + vbNewLine +
                                        "Only girls: " + UserVariables.GetOnlyGirls(Author.Id).ToString() + vbNewLine +
                                        "Role: " + UserVariables.GetRole(Author.Id) + vbNewLine +
                                        "Status: " + UserVariables.GetStatus(Author.Id) + vbNewLine +
                                        "Setup status: " + UserSetup.GetSetupStatus(Author.Id).ToString() + vbNewLine +
                                        "Username: " + System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + Author.Id.ToString() + "\username.txt") + vbNewLine +
                                        "Reports: " + UserVariables.GetReports(Author.Id).ToString() + vbNewLine +
                                        "Language: " + UserVariables.GetLanguage(Author.Id))
                    End If
                Next
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Invalid language. Please, use ITA or ENG.")
            End If
        End If
    End Sub
End Class