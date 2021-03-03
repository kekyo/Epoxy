////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

using Epoxy;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace EpoxyHello.Views.Converters
{
    public sealed class ScoreToBrushConverter : ValueConverter<int, Brush>
    {
        private static readonly Brush yellow = new SolidColorBrush(Color.FromArgb(255, 96, 96, 0));
        private static readonly Brush gray = new SolidColorBrush(Color.FromArgb(255, 96, 96, 96));

        public override bool TryConvert(int from, out Brush result)
        {
            result = from >= 5 ? yellow : gray;
            return true;
        }
    }
}
