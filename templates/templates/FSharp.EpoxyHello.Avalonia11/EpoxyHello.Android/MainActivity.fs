namespace EpoxyHello.Android

open Android.App
open Android.Content.PM
open Avalonia
open Avalonia.Android
open EpoxyHello

[<Activity(
    Label = "EpoxyHello.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = (ConfigChanges.Orientation ||| ConfigChanges.ScreenSize ||| ConfigChanges.UiMode))>]
type MainActivity() =
    inherit AvaloniaMainActivity<App>()

    override _.CustomizeAppBuilder(builder) =
        base.CustomizeAppBuilder(builder).WithInterFont()
