Imports System
Imports System.Windows

Friend NotInheritable Class Program
    <STAThread()>
    Public Shared Sub Main()
        Dim app As New Application()
        Dim window As New MainWindow()
        app.Run(window)
    End Sub
End Class
