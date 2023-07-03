open System

open Avalonia
open Avalonia.Logging

open EpoxyHello

// Avalonia configuration, don't remove; also used by visual designer.
[<CompiledName "BuildAvaloniaApp">] 
let buildAvaloniaApp() = 
    AppBuilder.Configure<App>().
        UsePlatformDetect().
        LogToTrace(LogEventLevel.Warning)

// Initialization code. Don't use any Avalonia, third-party APIs or any
// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
// yet and stuff might break.
[<STAThread>]
[<EntryPoint>]
let main args =
    buildAvaloniaApp().
        StartWithClassicDesktopLifetime(args)
