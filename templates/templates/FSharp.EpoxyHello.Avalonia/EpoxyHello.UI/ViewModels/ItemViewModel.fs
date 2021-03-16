////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

namespace EpoxyHello.ViewModels

open Epoxy
open Avalonia.Media.Imaging

[<Sealed; ViewModel>]
type ItemViewModel() =

    member val Title : string = "" with get, set
    member val Image : Bitmap = null with get, set
    member val Score : int = 0 with get, set
