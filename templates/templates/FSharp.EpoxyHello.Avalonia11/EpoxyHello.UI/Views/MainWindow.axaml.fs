namespace EpoxyHello.Views

open Avalonia.Controls
open Avalonia.Markup.Xaml

[<Sealed>]
type MainWindow() as self =
    inherit Window()

    do
        self.InitializeComponent()

    member private self.InitializeComponent() =
        AvaloniaXamlLoader.Load(self)
