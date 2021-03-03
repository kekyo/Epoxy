////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

namespace EpoxyHello.ViewModels

open Epoxy
open System.Windows.Media

[<Sealed>]
type ItemViewModel() =
    inherit ViewModel()

    member __.Title
        with get(): string = __.getValue()
        and set (value: string) = __.setValue value

    member __.Image
        with get(): ImageSource = __.getValue()
        and set (value: ImageSource) = __.setValue value

    member __.Score
        with get(): int = __.getValue()
        and set (value: int) = __.setValue value
