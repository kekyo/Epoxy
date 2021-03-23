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

/// <summary>
/// The Pile factory.
/// </summary>
/// <remarks>You can manipulate XAML controls directly inside ViewModels
/// when places and binds both an Anchor (in XAML) and a Pile.</remarks>
[<AbstractClass; Sealed>]
type public PileFactory =
    /// <summary>
    /// Create an anonymous control typed Pile.
    /// </summary>
    /// <returns>Pile instance</returns>
    static member create() =
        new Pile<UIElement>()
    /// <summary>
    /// Create a Pile.
    /// </summary>
    /// <typeparam name="'TUIElement">Target control type</typeparam>
    /// <returns>Pile instance</returns>
    static member create<'TUIElement when 'TUIElement :> UIElement>() =
        new Pile<'TUIElement>()

[<DebuggerStepThrough>]
[<AutoOpen>]
module public PileExtension =
    /// <summary>
    /// Pile manipulator class.
    /// </summary>
    /// <remarks>You can manipulate XAML controls directly inside ViewModels
    /// when places and binds both an Anchor (in XAML) and a Pile.</remarks>
    type public Pile<'TUIElement when 'TUIElement :> UIElement> with
        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="TUIElement">Target control type</typeparam>
        /// <param name="pile">Pile instance</param>
        /// <param name="action">Predicts when rents control instance</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>Async&lt;unit&gt; instance</returns>
        member self.executeAsync (action: 'TUIElement -> Async<unit>, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalExecuteAsync(action >> asyncUnitAsValueTaskUnit |> asFunc1, canIgnore) |> valueTaskUnitAsAsyncResult
        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">Target control type</typeparam>
        /// <typeparam name="'TResult">Action result type</typeparam>
        /// <param name="pile">Pile instance</param>
        /// <param name="action">Predicts when rents control instance</param>
        /// <returns>Async result for action</returns>
        /// <remarks>This overload has to complete XAML data-binding.</remarks>
        member self.executeAsync (action: 'TUIElement -> Async<'TResult>) =
            self.InternalExecuteAsync<'TResult>(action >> asyncAsValueTask |> asFunc1) |> valueTaskAsAsyncResult
