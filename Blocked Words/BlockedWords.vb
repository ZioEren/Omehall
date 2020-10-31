Public Class BlockedWords
    Public Shared Function ContainsBlockedWord(ByVal StringIn As String) As Boolean
        Dim Confirm As Boolean = False
        Dim s As String = ""
        For Each c As Char In StringIn.ToLower()
            Dim F As String = c
            Dim chars As String = "abcdefghijklmnopqrstuvwxyz0123456789"
            For Each t As Char In chars
                Dim M As String = t
                If F = M Then
                    s += M
                End If
            Next
        Next
        Dim textboxona As New TextBox With {.Text = System.IO.File.ReadAllText(Application.StartupPath & "\bot\blocked_words.txt")}
        For i = 0 To textboxona.Lines.Count - 1
            Dim Line As String = textboxona.Lines(i).ToLower()
            If s.Contains(Line) Then
                Confirm = True
            End If
        Next
        Return Confirm
    End Function
End Class