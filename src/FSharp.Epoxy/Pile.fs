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

namespace Epoxy

open Epoxy
open Epoxy.Internal

open System
open System.Diagnostics
open System.Threading.Tasks
open System.Runtime.InteropServices

#if WINDOWS_UWP || UNO
open Windows.UI.Xaml
#endif

#if WINUI
open Microsoft.UI.Xaml
#endif

#if WINDOWS_WPF
open System.Windows

#endif

#if XAMARIN_FORMS
open Xamarin.Forms
type DependencyObject = Xamarin.Forms.BindableObject
type UIElement = Xamarin.Forms.Element
#endif

#if AVALONIA
open Avalonia
type DependencyObject = Avalonia.IAvaloniaObject
type UIElement = Avalonia.IStyledElement
#endif

[<AbstractClass; Sealed>]
type public PileFactory =
    static member create() =
        new Pile<UIElement>()
    static member create<'TUIElement when 'TUIElement :> UIElement>() =
        new Pile<'TUIElement>()

[<DebuggerStepThrough>]
[<AutoOpen>]
module public PileExtension =
    type public Pile<'TUIElement when 'TUIElement :> UIElement> with
        member self.executeAsync (action: 'TUIElement -> Async<unit>, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalExecuteAsync(action >> asyncUnitAsValueTaskUnit |> asFunc1, canIgnore) |> valueTaskUnitAsAsyncResult
        member self.executeAsync (action: 'TUIElement -> Async<'TResult>) =
            self.InternalExecuteAsync<'TResult>(action >> asyncAsValueTask |> asFunc1) |> valueTaskAsAsyncResult
