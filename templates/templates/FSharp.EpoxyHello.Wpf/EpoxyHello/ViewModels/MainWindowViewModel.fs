////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

namespace EpoxyHello.ViewModels

open Epoxy
open Epoxy.Synchronized

open System.Collections.ObjectModel
open System.IO
open System.Windows
open System.Windows.Media.Imaging

open EpoxyHello.Models

[<Sealed; ViewModel>]
type public MainWindowViewModel() as self =
    do
        self.Items <- new ObservableCollection<ItemViewModel>()

        // A handler for window loaded
        self.Ready <- Command.Factory.createSync(fun () ->
            self.IsEnabled <- true)

        // A handler for fetch button
        self.Fetch <- CommandFactory.create(fun () -> async {
            do self.IsEnabled <- false
            try
                // Uses Reddit API
                let! reddits = Reddit.fetchNewPostsAsync "r/aww"
                do self.Items.Clear()

                let fetchImageAsync url = async {
                    try
                        let! imageData = Reddit.fetchImageAsync url
                        let bitmap = new WriteableBitmap(BitmapFrame.Create(new MemoryStream(imageData)))
                        do bitmap.Freeze()
                        return Some bitmap
                    // Some images will cause decoding error by WPF's BitmapFrame, so ignoring it.
                    with
                    | :? FileFormatException -> return None
                }

                for reddit in reddits do
                    let! image = fetchImageAsync reddit.Url
                    match image with
                    | Some bitmap ->
                        let item = new ItemViewModel()
                        do item.Title <- reddit.Title
                        do item.Score <- reddit.Score
                        do item.Image <- bitmap
                        do self.Items.Add(item)
                    | None -> ()
            finally
                do self.IsEnabled <- true
        })

    member val Ready: Command = null with get, set
    member val IsEnabled: bool = false with get, set
    member val Fetch: Command = null with get, set
    member val Items: ObservableCollection<ItemViewModel> = null with get, set
