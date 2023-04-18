open System
open System.Windows

[<STAThread>]
[<EntryPoint>]
let main _ =
    let application = 
        Application.LoadComponent(Uri("App.xaml", UriKind.Relative)) :?> Application
    application.Run()
