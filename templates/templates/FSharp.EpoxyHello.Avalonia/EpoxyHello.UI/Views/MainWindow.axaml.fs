////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

namespace EpoxyHello.Views

open Avalonia
open Avalonia.Controls
open Avalonia.Markup.Xaml

[<Sealed>]
type MainWindow() as self =
    inherit Window()

    do
        self.InitializeComponent()
#if DEBUG
        self.AttachDevTools()
#endif

    member private self.InitializeComponent() =
        AvaloniaXamlLoader.Load(self)
