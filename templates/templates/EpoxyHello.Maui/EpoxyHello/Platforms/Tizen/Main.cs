using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using System;

namespace EpoxyHello;

internal class Program : MauiApplication
{
    protected override MauiApp CreateMauiApp() =>
        MauiProgram.CreateMauiApp();

    public static void Main(string[] args)
    {
        var app = new Program();
        app.Run(args);
    }
}
