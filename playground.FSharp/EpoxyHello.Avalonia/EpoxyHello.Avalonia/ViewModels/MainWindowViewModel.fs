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

[<AutoOpen>]
module private MainWindowViewModelModule =
    let inline valueNullableTo (defaultValue: 'T) (vn: Nullable<'T>) =
        match vn.HasValue with
        | true -> vn.Value
        | false -> defaultValue
    let inline refNullableTo (defaultValue: 'T) (vn: 'T) =
        match vn with
        | null -> defaultValue
        | _ -> vn

[<Sealed; ViewModel>]
type public MainWindowViewModel() as self =
    do
        self.Items <- new ObservableCollection<ItemViewModel>()

        // A handler for window loaded
        self.Ready <- Command.Factory.createSync(fun () ->
            self.IsEnabled <- true)

        // A handler for fetch button
        self.Fetch <- Command.Factory.create(fun () -> async {
            do self.IsEnabled <- false
            try
                // Uses The Cat API
                let! cats = TheCatAPI.FetchTheCatsAsync 10
                do self.Items.Clear()

                let fetchImageAsync url = async {
                    let! image = Reddit.FetchImageAsync url
                    return new Bitmap(new MemoryStream(image))
                }

                for cat in cats do
                    let! image = fetchImageAsync cat.Url
                    match image, (cat.Bleeds |> Seq.tryHead) with
                    | bitmap, Some bleed ->
                        let item = new ItemViewModel()
                        do item.Title <- bleed.Description |> refNullableTo bleed.Temperament |> refNullableTo "(No comment)"
                        do item.Score <- bleed.Intelligence |> valueNullableTo 5
                        do item.Image <- bitmap
                        do self.Items.Add(item)
                    | bitmap, _ ->
                        let item = new ItemViewModel()
                        do item.Title <- "(No comment)"
                        do item.Score <- 5
                        do item.Image <- bitmap
                        do self.Items.Add(item)
            finally
                do self.IsEnabled <- true
        })
        
    member val Ready: Command = null with get, set
    member val IsEnabled: bool = false with get, set
    member val Fetch: Command = null with get, set
    member val Items: ObservableCollection<ItemViewModel> = null with get, set
