////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

using Epoxy;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace EpoxyHello.Views.Converters
{
    public sealed class ScoreToBrushConverter : ValueConverter<int, Brush>
    {
        private static readonly Brush yellow = new SolidColorBrush(Color.FromRgba(96, 96, 0, 255));
        private static readonly Brush gray = new SolidColorBrush(Color.FromRgba(96, 96, 96, 255));

        public override bool TryConvert(int from, out Brush result)
        {
            result = from >= 5 ? yellow : gray;
            return true;
        }
    }
}
