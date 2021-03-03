////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

open System
open System.Windows

[<STAThread>]
[<EntryPoint>]
let main _ =
    let application = 
        Application.LoadComponent(Uri("App.xaml", UriKind.Relative)) :?> Application
    application.Run()
