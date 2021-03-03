////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

namespace EpoxyHello.Views.Converters

open Epoxy
open Avalonia.Media

[<Sealed>]
type public ScoreToBrushConverter() =
    inherit ValueConverter<int, Brush>()

    let yellow = new SolidColorBrush(Color.FromArgb(255uy, 96uy, 96uy, 0uy)) :> Brush
    let gray = new SolidColorBrush(Color.FromArgb(255uy, 96uy, 96uy, 96uy)) :> Brush

    override __.convert from =
        if from >= 5 then Some yellow else Some gray
