Imports Discord.WebSocket
Imports Discord
Public Class CommandExecutor
    Private Shared Trigger As String = "!"
    Public Shared Function GetTrigger() As String
        Return Trigger
    End Function
    Public Shared Sub SetTrigger(ByVal tTrigger As String)
        Trigger = tTrigger
    End Sub
    Public Shared Sub ExecuteCommand(ByVal Cmd As String, ByVal Message As SocketMessage)
        Dim Arguments() As String = Nothing
        If Cmd.Contains(" ") Then
            Arguments = Cmd.Split(" ")
        End If
        Dim ACmd As String = Cmd.Replace(" ", "")
        Dim FirstArg As String = ""
        If Cmd.Contains(" ") Then
            FirstArg = Arguments(0)
        Else
            FirstArg = ACmd
        End If
        Dim Author As SocketUser = Message.Author
        FirstArg = FirstArg.ToLower()
        If FirstArg = "help" Then
            Dim Messaj As String = "Here is the list of all commands of Omehall (1/1):" + vbNewLine + vbNewLine +
                                   "[USER] " + Trigger + "help - See the list of all commands." + vbNewLine +
                                   "[USER] " + Trigger + "rules - See all the rules of the chat." + vbNewLine +
                                   "[USER] " + Trigger + "start - Match with a random stranger." + vbNewLine +
                                   "[USER] " + Trigger + "stop - End the chat with the stranger." + vbNewLine +
                                   "[USER] " + Trigger + "vip - See all the informations about VIP roles." + vbNewLine +
                                   "[USER] " + Trigger + "report - Report the current stranger." + vbNewLine +
                                   "[USER] " + Trigger + "setlang <ITA/ENG> - Set the language used by the bot." + vbNewLine +
                                   "[BRONZE] " + Trigger + "setname <name> - Set your name." + vbNewLine +
                                   "[BRONZE] " + Trigger + "setage <age> - Set your age." + vbNewLine +
                                   "[BRONZE] " + Trigger + "setcountry <country> - Set your country." + vbNewLine +
                                   "[BRONZE] " + Trigger + "setstatus <status> - Set your status." + vbNewLine +
                                   "[BRONZE] " + Trigger + "setgender <gender> - Set your gender." + vbNewLine +
                                   "[SILVER] " + Trigger + "onlygirls - You can meet only girls."
            If UserVariables.GetRole(Author.Id) = "Mod" Or UserVariables.GetRole(Author.Id) = "Admin" Then
                Messaj += vbNewLine +
                                   "[MOD] " + Trigger + "ban <id> <reason> - Ban an user with a specified reason." + vbNewLine +
                                   "[MOD] " + Trigger + "unban <id> - Unban an user." + vbNewLine +
                                   "[MOD] " + Trigger + "recent - Get the recent activity of the users." + vbNewLine +
                                   "[MOD] " + Trigger + "logs <id> - Get the recent activity of a specified user." + vbNewLine +
                                   "[MOD] " + Trigger + "profile <id> - Get the profile informations of a specified user." + vbNewLine +
                                   "[MOD] " + Trigger + "sendmsg <id> <message> - Send a message to a user through the bot."
            End If
            If UserVariables.GetRole(Author.Id) = "Admin" Then
                Messaj += vbNewLine +
                    "[ADMIN] " + Trigger + "banall <reason> - Ban all users in Omehall." + vbNewLine +
                                   "[ADMIN] " + Trigger + "unbanall - Unban all users in Omehall." + vbNewLine +
                                   "[ADMIN] " + Trigger + "setrole <id> <role> - Set the role of a user." + vbNewLine +
                                   "[ADMIN] " + Trigger + "broadcast <message> - Send a broadcast to all users."
            End If
            Messaj += vbNewLine + vbNewLine + "Bot created by: " + vbNewLine + vbNewLine + "Discord » ZioJoJo#0326" + vbNewLine + "Telegram » @nonesisteròmaipiu"
            Author.SendMessageAsync(Messaj)
        ElseIf FirstArg = "rules" Then
            Author.SendMessageAsync(System.IO.File.ReadAllText(Application.StartupPath & "\bot\rules.txt") + vbNewLine + vbNewLine + "Discord » ZioJoJo#0326" + vbNewLine + "Telegram » @nonesisteròmaipiu")
        ElseIf FirstArg = "start" Then
            Author.SendMessageAsync("We are matching you with a random stranger...")
            QueueManager.GetWaitingUsers().Add(Message.Author)
        ElseIf FirstArg = "setlang" Then
            Try
                If Arguments(1).ToLower().Equals("ita") Or Arguments(1).ToLower().Equals("eng") Then
                    UserVariables.SetLanguage(Message.Author.Id, Arguments(1))
                    Author.SendMessageAsync("Succesfully set your language to " + UserVariables.GetLanguage(Message.Author.Id).ToUpper())
                Else
                    Author.SendMessageAsync("Invalid language. Please use ITA or ENG.")
                End If
            Catch ex As Exception
                Author.SendMessageAsync("Failed to execute the command.")
            End Try
        ElseIf FirstArg = "stop" Then
            Author.SendMessageAsync("You are not currently connected with a stranger.")
        ElseIf FirstArg = "vip" Then
            Author.SendMessageAsync(System.IO.File.ReadAllText(Application.StartupPath & "\bot\vip.txt") + vbNewLine + vbNewLine + "Discord » ZioJoJo#0326" + vbNewLine + "Telegram » @nonesisteròmaipiu")
        ElseIf FirstArg = "onlygirls" Then
            If UserVariables.GetRole(Author.Id) = "User" Or UserVariables.GetRole(Author.Id) = "Bronze" Then
                Author.SendMessageAsync("You are not VIP.")
            Else
                If UserVariables.GetOnlyGirls(Author.Id) Then
                    UserVariables.SetOnlyGirls(Author.Id, False)
                    Author.SendMessageAsync("Now you disabled the only girls option.")
                Else
                    UserVariables.SetOnlyGirls(Author.Id, True)
                    Author.SendMessageAsync("Now you enabled the only girls option.")
                End If
            End If
        ElseIf FirstArg = "ban" Then
            If UserVariables.GetRole(Author.Id) = "Mod" Or UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim ID As ULong = ULong.Parse(Arguments(1))
                    Dim Reason As String = ""
                    Reason = Cmd.Replace("ban " + ID.ToString() + " ", "")
                    UserVariables.SetBanned(ID, True)
                    UserVariables.SetBanReason(ID, Reason)
                    Author.SendMessageAsync("This user is now banned.")
                    DiscordBot.GetDiscord().GetUser(ID).SendMessageAsync("You are now banned from the bot! Reason: " + Reason)
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "unban" Then
            If UserVariables.GetRole(Author.Id) = "Mod" Or UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim ID As ULong = ULong.Parse(Arguments(1))
                    UserVariables.SetBanned(ID, False)
                    UserVariables.SetBanReason(ID, "")
                    Author.SendMessageAsync("This user has been unbanned.")
                    DiscordBot.GetDiscord().GetUser(ID).SendMessageAsync("You have been succesfully unbanned from the bot.")
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "recent" Then
            Try
                Dim Coso As String = System.IO.File.ReadAllText(Application.StartupPath & "\bot\logs.txt")
                Dim Massaj As String = ""
                Dim textboxona As New TextBox With {.Text = Coso}
                For i = 0 To textboxona.Lines.Count - 1
                    Dim Line As String = textboxona.Lines(textboxona.Lines.Count - i - 1)
                    If Massaj = "" Then
                        Massaj = Line
                    Else
                        If Massaj.Length < 2000 Then
                            Massaj += vbNewLine + Line
                        End If
                    End If
                Next
                If Massaj.Length >= 2000 Then
                    Do Until Massaj.Length = 1999
                        Massaj = Massaj.Substring(0, Massaj.Length - 1)
                    Loop
                End If
                Author.SendMessageAsync(Massaj)
            Catch ex As Exception
                Author.SendMessageAsync("Failed to execute the command.")
            End Try
        ElseIf FirstArg = "logs" Then
            Try
                Dim Coso As String = System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + Arguments(1) + "\logs.txt")
                Dim Massaj As String = ""
                Dim textboxona As New TextBox With {.Text = Coso}
                For i = 0 To textboxona.Lines.Count - 1
                    Dim Line As String = textboxona.Lines(textboxona.Lines.Count - i - 1)
                    If Massaj = "" Then
                        Massaj = Line
                    Else
                        If Massaj.Length < 2000 Then
                            Massaj += vbNewLine + Line
                        End If
                    End If
                Next
                If Massaj.Length >= 2000 Then
                    Do Until Massaj.Length = 1999
                        Massaj = Massaj.Substring(0, Massaj.Length - 1)
                    Loop
                End If
                Author.SendMessageAsync(Massaj)
            Catch ex As Exception
                Author.SendMessageAsync("Failed to execute the command.")
            End Try
        ElseIf FirstArg = "profile" Then
            If UserVariables.GetRole(Author.Id) = "Mod" Or UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim ID As ULong = ULong.Parse(Arguments(1))
                    Author.SendMessageAsync("Profile of the ID " + ID.ToString() + ":" + vbNewLine + vbNewLine +
                                        "Age: " + UserVariables.GetAge(ID).ToString() + vbNewLine +
                                        "Banned: " + UserVariables.GetBanned(ID).ToString() + vbNewLine +
                                        "Ban reason: " + UserVariables.GetBanReason(ID) + vbNewLine +
                                        "Country: " + UserVariables.GetCountry(ID) + vbNewLine +
                                        "Gender: " + UserVariables.GetGender(ID) + vbNewLine +
                                        "Name: " + UserVariables.GetName(ID) + vbNewLine +
                                        "Only girls: " + UserVariables.GetOnlyGirls(ID).ToString() + vbNewLine +
                                        "Role: " + UserVariables.GetRole(ID) + vbNewLine +
                                        "Status: " + UserVariables.GetStatus(ID) + vbNewLine +
                                        "Setup status: " + UserSetup.GetSetupStatus(ID).ToString() + vbNewLine +
                                        "Username: " + System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + ID.ToString() + "\username.txt") + vbNewLine +
                                        "Mention: " + System.IO.File.ReadAllText(Application.StartupPath & "\setup\" + ID.ToString() + "\mention.txt") + vbNewLine +
                                        "Reports: " + UserVariables.GetReports(ID).ToString() + vbNewLine +
                                        "Language: " + UserVariables.GetLanguage(ID).ToString())
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "setname" Then
            If Not UserVariables.GetRole(Author.Id) = "User" Then
                Try
                    Dim Name As String = Arguments(1)
                    If Name.Length < 16 Then
                        UserVariables.SetName(Author.Id, Name)
                        Author.SendMessageAsync("Your name has been set to " + Name + ".")
                    Else
                        Author.SendMessageAsync("Name needs to be with a minimum length of 16 chars.")
                    End If
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("You are not a VIP.")
            End If
        ElseIf FirstArg = "setcountry" Then
            If Not UserVariables.GetRole(Author.Id) = "User" Then
                Try
                    Dim Country As String = Arguments(1)
                    If Country.Length < 10 Then
                        UserVariables.SetCountry(Author.Id, Country)
                        Author.SendMessageAsync("Your country has been set to " + Country + ".")
                    End If
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("You are not a VIP.")
            End If
        ElseIf FirstArg = "setstatus" Then
            If Not UserVariables.GetRole(Author.Id) = "User" Then
                Try
                    Dim Status As String = Arguments(1)
                    If Status.Length < 50 Then
                        UserVariables.SetStatus(Author.Id, Status)
                        Author.SendMessageAsync("Your status has been set to " + Status + ".")
                    End If
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("You are not a VIP.")
            End If
        ElseIf FirstArg = "setgender" Then
            If Not UserVariables.GetRole(Author.Id) = "User" Then
                Try
                    Dim Gender As String = Arguments(1)
                    If Gender.ToLower().Equals("male") Or Gender.ToLower().Equals("female") Then
                        UserVariables.SetGender(Author.Id, Gender)
                        Author.SendMessageAsync("Your gender has been set to " + Gender + ".")
                    Else
                        Author.SendMessageAsync("Invalid gender. Please, insert male or female.")
                    End If
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("You are not a VIP.")
            End If
        ElseIf FirstArg = "setage" Then
            If Not UserVariables.GetRole(Author.Id) = "User" Then
                Try
                    Dim Age As Integer = Int32.Parse(Arguments(1))
                    UserVariables.SetAge(Author.Id, Age)
                    Author.SendMessageAsync("Your age has been set to " + Age.ToString() + ".")
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("You are not a VIP.")
            End If
        ElseIf FirstArg = "banall" Then
            If UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim Reason As String = Cmd
                    Reason = Reason.Replace("banall ", "")
                    For Each s As String In System.IO.Directory.GetDirectories(Application.StartupPath & "\setup")
                        s = s.Replace(Application.StartupPath & "\setup\", "")
                        s = s.Replace("\", "")
                        s = s.Replace(" ", "")
                        UserVariables.SetBanned(ULong.Parse(s), True)
                        UserVariables.SetBanReason(ULong.Parse(s), Reason)
                    Next
                    Author.SendMessageAsync("All users have been banned. Reason: " + Reason)
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "unbanall" Then
            If UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    For Each s As String In System.IO.Directory.GetDirectories(Application.StartupPath & "\setup")
                        UserVariables.SetBanned(ULong.Parse(s), True)
                        UserVariables.SetBanReason(ULong.Parse(s), "")
                    Next
                    Author.SendMessageAsync("All users have been unbanned.")
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "setrole" Then
            If UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim ID As ULong = ULong.Parse(Arguments(1))
                    Dim Role As String = Arguments(2)
                    UserVariables.SetRole(ID, Role)
                    Author.SendMessageAsync("The role of the user ID " + Arguments(1) + " has been set to " + Arguments(2) + ".")
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "broadcast" Then
            If UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim Msg As String = Cmd
                    Msg = Msg.Replace("broadcast ", "")
                    For Each s As String In System.IO.Directory.GetDirectories(Application.StartupPath & "\setup")
                        s = s.Replace(Application.StartupPath & "\setup\", "")
                        s = s.Replace("\", "")
                        s = s.Replace(" ", "")
                        DiscordBot.GetDiscord().GetUser(ULong.Parse(s)).SendMessageAsync(":warning: **Warning** » " + Msg)
                    Next
                    Author.SendMessageAsync("The message has been succesfully sent to all users of the bot.")
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "sendmsg" Then
            If UserVariables.GetRole(Author.Id) = "Mod" Or UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim ID As ULong = ULong.Parse(Arguments(1))
                    Dim Msg As String = Cmd.Replace("sendmsg " + ID.ToString() + " ", "")
                    DiscordBot.GetDiscord().GetUser(ID).SendMessageAsync(Msg)
                    Author.SendMessageAsync("The message has been sent to the user.")
                Catch ex As Exception
                    Author.SendMessageAsync("Failed to execute the command.")
                End Try
            Else
                Author.SendMessageAsync("Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "report" Then
            Author.SendMessageAsync("You are not currently connected with a stranger.")
        Else
            Author.SendMessageAsync("Unrecognized command. Type " + Trigger + "help for the list of all commands.")
        End If
    End Sub
    Public Shared Sub ExecuteCommand(ByVal Cmd As String, ByVal Message As Telegram.Bot.Args.MessageEventArgs)
        Dim Arguments() As String = Nothing
        If Cmd.Contains(" ") Then
            Arguments = Cmd.Split(" ")
        End If
        Dim ACmd As String = Cmd.Replace(" ", "")
        Dim FirstArg As String = ""
        If Cmd.Contains(" ") Then
            FirstArg = Arguments(0)
        Else
            FirstArg = ACmd
        End If
        Dim Author As Telegram.Bot.Types.User = Message.Message.From
        FirstArg = FirstArg.ToLower()
        If FirstArg = "help" Then
            Dim Messaj As String = "Here is the list of all commands of Omehall (1/1):" + vbNewLine + vbNewLine +
                                   "[USER] " + Trigger + "help - See the list of all commands." + vbNewLine +
                                   "[USER] " + Trigger + "rules - See all the rules of the chat." + vbNewLine +
                                   "[USER] " + Trigger + "start - Match with a random stranger." + vbNewLine +
                                   "[USER] " + Trigger + "stop - End the chat with the stranger." + vbNewLine +
                                   "[USER] " + Trigger + "vip - See all the informations about VIP roles." + vbNewLine +
                                   "[USER] " + Trigger + "report - Report the current stranger." + vbNewLine +
                                   "[USER] " + Trigger + "setlang <ITA/ENG> - Set the language used by the bot." + vbNewLine +
                                   "[BRONZE] " + Trigger + "setname <name> - Set your name." + vbNewLine +
                                   "[BRONZE] " + Trigger + "setage <age> - Set your age." + vbNewLine +
                                   "[BRONZE] " + Trigger + "setcountry <country> - Set your country." + vbNewLine +
                                   "[BRONZE] " + Trigger + "setstatus <status> - Set your status." + vbNewLine +
                                   "[BRONZE] " + Trigger + "setgender <gender> - Set your gender." + vbNewLine +
                                   "[SILVER] " + Trigger + "onlygirls - You can meet only girls."
            If UserVariables.GetRole(Author.Id) = "Mod" Or UserVariables.GetRole(Author.Id) = "Admin" Then
                Messaj += vbNewLine +
                                   "[MOD] " + Trigger + "ban <id> <reason> - Ban an user with a specified reason." + vbNewLine +
                                   "[MOD] " + Trigger + "unban <id> - Unban an user." + vbNewLine +
                                   "[MOD] " + Trigger + "recent - Get the recent activity of the users." + vbNewLine +
                                   "[MOD] " + Trigger + "logs <id> - Get the recent activity of a specified user." + vbNewLine +
                                   "[MOD] " + Trigger + "profile <id> - Get the profile informations of a specified user." + vbNewLine +
                                   "[MOD] " + Trigger + "sendmsg <id> <message> - Send a message to a user through the bot."
            End If
            If UserVariables.GetRole(Author.Id) = "Admin" Then
                Messaj += vbNewLine +
                    "[ADMIN] " + Trigger + "banall <reason> - Ban all users in Omehall." + vbNewLine +
                                   "[ADMIN] " + Trigger + "unbanall - Unban all users in Omehall." + vbNewLine +
                                   "[ADMIN] " + Trigger + "setrole <id> <role> - Set the role of a user." + vbNewLine +
                                   "[ADMIN] " + Trigger + "broadcast <message> - Send a broadcast to all users."
            End If
            Messaj += vbNewLine + vbNewLine + "Bot created by: " + vbNewLine + vbNewLine + "Discord » ZioJoJo#0326" + vbNewLine + "Telegram » @nonesisteròmaipiu"
            TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, Messaj)
        ElseIf FirstArg = "rules" Then
            TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, System.IO.File.ReadAllText(Application.StartupPath & "\bot\rules.txt") + vbNewLine + vbNewLine + "Discord » ZioJoJo#0326" + vbNewLine + "Telegram » @nonesisteròmaipiu")
        ElseIf FirstArg = "start" Then
            TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "We are matching you with a random stranger...")
            QueueManager.GetWaitingUsers1().Add(Message.Message.From)
        ElseIf FirstArg = "setlang" Then
            Try
                If Arguments(1).ToLower().Equals("ita") Or Arguments(1).ToLower().Equals("eng") Then
                    UserVariables.SetLanguage(Author.Id, Arguments(1))
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Succesfully set your language to " + UserVariables.GetLanguage(Message.Message.From.Id).ToUpper())
                Else
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Invalid language. Please use ITA or ENG.")
                End If
            Catch ex As Exception
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
            End Try
        ElseIf FirstArg = "stop" Then
            TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "You are not currently connected with a stranger.")
        ElseIf FirstArg = "vip" Then
            TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, System.IO.File.ReadAllText(Application.StartupPath & "\bot\vip.txt") + vbNewLine + vbNewLine + "Discord » ZioJoJo#0326" + vbNewLine + "Telegram » @nonesisteròmaipiu")
        ElseIf FirstArg = "onlygirls" Then
            If UserVariables.GetRole(Author.Id) = "User" Or UserVariables.GetRole(Author.Id) = "Bronze" Then
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "You are not VIP.")
            Else
                If UserVariables.GetOnlyGirls(Author.Id) Then
                    UserVariables.SetOnlyGirls(Author.Id, False)
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Now you disabled the only girls option.")
                Else
                    UserVariables.SetOnlyGirls(Author.Id, True)
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Now you enabled the only girls option.")
                End If
            End If
        ElseIf FirstArg = "ban" Then
            If UserVariables.GetRole(Author.Id) = "Mod" Or UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim ID As Integer = ULong.Parse(Arguments(1))
                    Dim Reason As String = ""
                    Reason = Cmd.Replace("ban " + ID.ToString() + " ", "")
                    UserVariables.SetBanned(ID, True)
                    UserVariables.SetBanReason(ID, Reason)
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "This user is now banned.")
                    TelegramBot.GetTelegram().SendTextMessageAsync(ID, "You are now banned from the bot! Reason: " + Reason)
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "unban" Then
            If UserVariables.GetRole(Author.Id) = "Mod" Or UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim ID As Integer = ULong.Parse(Arguments(1))
                    UserVariables.SetBanned(ID, False)
                    UserVariables.SetBanReason(ID, "")
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "This user has been unbanned.")
                    TelegramBot.GetTelegram().SendTextMessageAsync(ID, "You have been succesfully unbanned from the bot.")
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "recent" Then
            Try
                Dim Coso As String = System.IO.File.ReadAllText(Application.StartupPath & "\bot\logs1.txt")
                Dim Massaj As String = ""
                Dim textboxona As New TextBox With {.Text = Coso}
                For i = 0 To textboxona.Lines.Count - 1
                    Dim Line As String = textboxona.Lines(textboxona.Lines.Count - i - 1)
                    If Massaj = "" Then
                        Massaj = Line
                    Else
                        If Massaj.Length < 2000 Then
                            Massaj += vbNewLine + Line
                        End If
                    End If
                Next
                If Massaj.Length >= 2000 Then
                    Do Until Massaj.Length = 1999
                        Massaj = Massaj.Substring(0, Massaj.Length - 1)
                    Loop
                End If
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, Massaj)
            Catch ex As Exception
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
            End Try
        ElseIf FirstArg = "logs" Then
            Try
                Dim Coso As String = System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + Arguments(1) + "\logs.txt")
                Dim Massaj As String = ""
                Dim textboxona As New TextBox With {.Text = Coso}
                For i = 0 To textboxona.Lines.Count - 1
                    Dim Line As String = textboxona.Lines(textboxona.Lines.Count - i - 1)
                    If Massaj = "" Then
                        Massaj = Line
                    Else
                        If Massaj.Length < 2000 Then
                            Massaj += vbNewLine + Line
                        End If
                    End If
                Next
                If Massaj.Length >= 2000 Then
                    Do Until Massaj.Length = 1999
                        Massaj = Massaj.Substring(0, Massaj.Length - 1)
                    Loop
                End If
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, Massaj)
            Catch ex As Exception
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
            End Try
        ElseIf FirstArg = "profile" Then
            If UserVariables.GetRole(Author.Id) = "Mod" Or UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim ID As Integer = Integer.Parse(Arguments(1))
                    TelegramBot.GetTelegram().SendTextMessageAsync(ID, "Profile of the ID " + ID.ToString() + ":" + vbNewLine + vbNewLine +
                                        "Age: " + UserVariables.GetAge(ID).ToString() + vbNewLine +
                                        "Banned: " + UserVariables.GetBanned(ID).ToString() + vbNewLine +
                                        "Ban reason: " + UserVariables.GetBanReason(ID) + vbNewLine +
                                        "Country: " + UserVariables.GetCountry(ID) + vbNewLine +
                                        "Gender: " + UserVariables.GetGender(ID) + vbNewLine +
                                        "Name: " + UserVariables.GetName(ID) + vbNewLine +
                                        "Only girls: " + UserVariables.GetOnlyGirls(ID).ToString() + vbNewLine +
                                        "Role: " + UserVariables.GetRole(ID) + vbNewLine +
                                        "Status: " + UserVariables.GetStatus(ID) + vbNewLine +
                                        "Setup status: " + UserSetup.GetSetupStatus(ID).ToString() + vbNewLine +
                                        "Username: " + System.IO.File.ReadAllText(Application.StartupPath & "\setup1\" + ID.ToString() + "\username.txt") + vbNewLine +
                                        "Reports: " + UserVariables.GetReports(ID).ToString() + vbNewLine +
                                        "Language: " + UserVariables.GetLanguage(ID).ToString())
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "setname" Then
            If Not UserVariables.GetRole(Author.Id) = "User" Then
                Try
                    Dim Name As String = Arguments(1)
                    If Name.Length < 16 Then
                        UserVariables.SetName(Author.Id, Name)
                        TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Your name has been set to " + Name + ".")
                    Else
                        TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Name needs to be with a minimum length of 16 chars.")
                    End If
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "You are not a VIP.")
            End If
        ElseIf FirstArg = "setcountry" Then
            If Not UserVariables.GetRole(Author.Id) = "User" Then
                Try
                    Dim Country As String = Arguments(1)
                    If Country.Length < 10 Then
                        UserVariables.SetCountry(Author.Id, Country)
                        TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Your country has been set to " + Country + ".")
                    End If
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "You are not a VIP.")
            End If
        ElseIf FirstArg = "setstatus" Then
            If Not UserVariables.GetRole(Author.Id) = "User" Then
                Try
                    Dim Status As String = Arguments(1)
                    If Status.Length < 50 Then
                        UserVariables.SetStatus(Author.Id, Status)
                        TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Your status has been set to " + Status + ".")
                    End If
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "You are not a VIP.")
            End If
        ElseIf FirstArg = "setgender" Then
            If Not UserVariables.GetRole(Author.Id) = "User" Then
                Try
                    Dim Gender As String = Arguments(1)
                    If Gender.ToLower().Equals("male") Or Gender.ToLower().Equals("female") Then
                        UserVariables.SetGender(Author.Id, Gender)
                        TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Your gender has been set to " + Gender + ".")
                    Else
                        TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Invalid gender. Please, insert male or female.")
                    End If
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "You are not a VIP.")
            End If
        ElseIf FirstArg = "setage" Then
            If Not UserVariables.GetRole(Author.Id) = "User" Then
                Try
                    Dim Age As Integer = Int32.Parse(Arguments(1))
                    UserVariables.SetAge(Author.Id, Age)
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Your age has been set to " + Age.ToString() + ".")
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "You are not a VIP.")
            End If
        ElseIf FirstArg = "banall" Then
            If UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim Reason As String = Cmd
                    Reason = Reason.Replace("banall ", "")
                    For Each s As String In System.IO.Directory.GetDirectories(Application.StartupPath & "\setup1")
                        s = s.Replace(Application.StartupPath & "\setup1\", "")
                        s = s.Replace("\", "")
                        s = s.Replace(" ", "")
                        UserVariables.SetBanned(Integer.Parse(s), True)
                        UserVariables.SetBanReason(Integer.Parse(s), Reason)
                    Next
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "All users have been banned. Reason: " + Reason)
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "unbanall" Then
            If UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    For Each s As String In System.IO.Directory.GetDirectories(Application.StartupPath & "\setup1")
                        s = s.Replace(Application.StartupPath & "\setup1\", "")
                        s = s.Replace("\", "")
                        s = s.Replace(" ", "")
                        UserVariables.SetBanned(Integer.Parse(s), False)
                        UserVariables.SetBanReason(Integer.Parse(s), "")
                    Next
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "All users have been unbanned.")
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "setrole" Then
            If UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim ID As Integer = Integer.Parse(Arguments(1))
                    Dim Role As String = Arguments(2)
                    UserVariables.SetRole(ID, Role)
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "The role of the user ID " + Arguments(1) + " has been set to " + Arguments(2) + ".")
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "broadcast" Then
            If UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim Msg As String = Cmd
                    Msg = Msg.Replace("broadcast ", "")
                    For Each s As String In System.IO.Directory.GetDirectories(Application.StartupPath & "\setup1")
                        s = s.Replace(Application.StartupPath & "\setup1\", "")
                        s = s.Replace("\", "")
                        s = s.Replace(" ", "")
                        TelegramBot.GetTelegram().SendTextMessageAsync(Integer.Parse(s), "⚠️ Warning » " + Msg)
                    Next
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "The message has been succesfully sent to all users of the bot.")
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "sendmsg" Then
            If UserVariables.GetRole(Author.Id) = "Mod" Or UserVariables.GetRole(Author.Id) = "Admin" Then
                Try
                    Dim ID As Integer = Integer.Parse(Arguments(1))
                    Dim Msg As String = Cmd.Replace("sendmsg " + ID.ToString() + " ", "")
                    TelegramBot.GetTelegram().SendTextMessageAsync(ID, Msg)
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "The message has been sent to the user.")
                Catch ex As Exception
                    TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Failed to execute the command.")
                End Try
            Else
                TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Unrecognized command. Type " + Trigger + "help for the list of all commands.")
            End If
        ElseIf FirstArg = "report" Then
            TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "You are not currently connected with a stranger.")
        Else
            TelegramBot.GetTelegram().SendTextMessageAsync(Author.Id, "Unrecognized command. Type " + Trigger + "help for the list of all commands.")
        End If
    End Sub
End Class