////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

namespace EpoxyHello.Models

open System

[<Struct>]
type RedditPost(title: string, url: Uri, score: int) =
    member __.Title = title
    member __.Url = url
    member __.Score = score
