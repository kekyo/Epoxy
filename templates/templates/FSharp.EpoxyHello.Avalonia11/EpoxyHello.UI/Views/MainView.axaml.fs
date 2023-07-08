namespace EpoxyHello.Views

open Avalonia.Controls
open Avalonia.Markup.Xaml

type MainView() as self =
    inherit UserControl()

    do
        self.InitializeComponent()

    member private self.InitializeComponent() =
        AvaloniaXamlLoader.Load(self)
