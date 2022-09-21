////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

using System.Windows;

using EpoxyHello.Views;

namespace EpoxyHello
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();

            Window.Current.Content = new MainPage();
        }
    }
}
