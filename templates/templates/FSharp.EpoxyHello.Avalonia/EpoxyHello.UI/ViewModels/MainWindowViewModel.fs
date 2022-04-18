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

open System
open System.Collections.ObjectModel
open System.IO

open Avalonia.Media.Imaging

open EpoxyHello.Models

[<Sealed; ViewModel>]
type MainWindowViewModel() as self =
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
                    let! image = Reddit.fetchImageAsync url
                    return new Bitmap(new MemoryStream(image))
                }

                for reddit in reddits do
                    let! image = fetchImageAsync reddit.Url
                    let item = new ItemViewModel()
                    do item.Title <- reddit.Title
                    do item.Score <- reddit.Score
                    do item.Image <- image
                    do self.Items.Add(item)
            finally
                do self.IsEnabled <- true
        })

    member val Ready: Command = null with get, set
    member val IsEnabled: bool = false with get, set
    member val Fetch: Command = null with get, set
    member val Items: ObservableCollection<ItemViewModel> = null with get, set
