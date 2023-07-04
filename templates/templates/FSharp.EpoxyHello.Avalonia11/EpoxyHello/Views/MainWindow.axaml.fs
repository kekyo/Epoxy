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
