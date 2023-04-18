namespace EpoxyHello

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Markup.Xaml

open EpoxyHello.Views

[<Sealed>]
type public App() =
    inherit Application()

    override self.Initialize() =
        AvaloniaXamlLoader.Load self

    override self.OnFrameworkInitializationCompleted() =
        match self.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktop ->
            desktop.MainWindow <- new MainWindow()
        | _ -> ()

        base.OnFrameworkInitializationCompleted()
