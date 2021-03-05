////////////////////////////////////////////////////////////////////////////
//
// Epoxy - An independent flexible XAML MVVM library for .NET
// Copyright (c) 2019-2021 Kouji Matsui (@kozy_kekyo, @kekyo2)
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

namespace EpoxyHello.Wpf.ViewModels

open Epoxy
open Epoxy.Synchronized

open System.Collections.ObjectModel
open System.IO
open System.Windows
open System.Windows.Media.Imaging

open EpoxyHello.Models

type public MainWindowViewModel() as self =
    inherit ViewModel()
    do
        self.Items <- new ObservableCollection<ItemViewModel>()

        // A handler for window loaded
        self.Ready <- Command.Factory.createSync(fun (e:RoutedEventArgs) ->
            self.IsEnabled <- true)

        // A handler for fetch button
        self.Fetch <- CommandFactory.create(fun () -> async {
            do self.IsEnabled <- false
            try
                // Uses Reddit API
                let! reddits = Reddit.FetchNewPostsAsync "r/aww"
                do self.Items.Clear()

                let fetchImageAsync url = async {
                    try
                        let! imageData = Reddit.FetchImageAsync url
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
