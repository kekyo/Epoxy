////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

using Epoxy;
using Xamarin.Forms;

namespace EpoxyHello.ViewModels
{
    public sealed class ItemViewModel : ViewModel
    {
        public string? Title
        {
            get => GetValue();
            set => SetValue(value);
        }

        public ImageSource? Image
        {
            get => GetValue();
            set => SetValue(value);
        }

        public int Score
        {
            get => GetValue();
            set => SetValue(value);
        }
    }
}
