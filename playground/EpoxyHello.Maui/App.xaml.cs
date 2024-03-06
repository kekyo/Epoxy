using Microsoft.Maui.Controls;

namespace EpoxyHello.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
