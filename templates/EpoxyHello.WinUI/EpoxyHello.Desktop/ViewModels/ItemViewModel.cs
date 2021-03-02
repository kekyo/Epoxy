#nullable enable

using Epoxy;
using Microsoft.UI.Xaml.Media;

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
