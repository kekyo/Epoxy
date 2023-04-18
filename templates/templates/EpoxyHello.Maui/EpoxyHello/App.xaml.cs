using Microsoft.Maui.Controls;

namespace EpoxyHello;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
