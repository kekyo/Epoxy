////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

using Epoxy;
using Microsoft.UI.Xaml.Media;

namespace EpoxyHello.ViewModels
{
    [ViewModel]
    public sealed class ItemViewModel
    {
        public string? Title { get; set; }

        public ImageSource? Image { get; set; }

        public int Score { get; set; }
    }
}
