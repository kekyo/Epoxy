using System.Windows;

using EpoxyHello.Views;

namespace EpoxyHello;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();

        Window.Current.Content = new MainPage();
    }
}
