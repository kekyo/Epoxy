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

[<Sealed>]
type public MainWindowViewModel() as self =
    inherit ViewModel()
    do
        self.Items <- new ObservableCollection<ItemViewModel>()

        // A handler for window loaded
        self.Ready <- Command.Factory.createSync<EventArgs>(fun e ->
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

    member __.Ready
        with get(): Command = __.getValue()
        and private set (value: Command) = __.setValue value

    member __.IsEnabled
        with get(): bool = __.getValue()
        and private set (value: bool) = __.setValue value
        
    member __.Fetch
        with get(): Command = __.getValue()
        and private set (value: Command) = __.setValue value

    member __.Items
        with get(): ObservableCollection<ItemViewModel> = __.getValue()
        and private set (value: ObservableCollection<ItemViewModel>) = __.setValue value
