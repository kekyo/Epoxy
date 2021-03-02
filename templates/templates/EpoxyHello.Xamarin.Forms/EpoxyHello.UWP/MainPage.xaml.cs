////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

namespace EpoxyHello.UWP
{
    public partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new EpoxyHello.App());
        }
    }
}
