Public Class Omehall
    Private Sub Omehall_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CheckForIllegalCrossThreadCalls = False
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime
        DiscordBot.SetToken("DISCORD_TOKEN_HERE")
        DiscordBot.SetupDiscordBot()
        TelegramBot.SetToken("TELEGRAM_TOKEN_HERE")
        TelegramBot.SetupTelegramBot()
        Timer1.Start()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        GC.Collect()
    End Sub
End Class