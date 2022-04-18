////////////////////////////////////////////////////////////////////////////
//
// Epoxy - An independent flexible XAML MVVM library for .NET
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
////////////////////////////////////////////////////////////////////////////

namespace EpoxyHello.Avalonia.ViewModels

open Epoxy
open Epoxy.Synchronized

open System
open System.Collections.ObjectModel
open System.IO

open Avalonia.Media.Imaging

open EpoxyHello.Models

type public MainWindowViewModel() as self =
    inherit ViewModel()
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
                let! reddits = Reddit.FetchNewPostsAsync "r/aww"
                do self.Items.Clear()

                let fetchImageAsync url = async {
                    let! image = Reddit.FetchImageAsync url
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

