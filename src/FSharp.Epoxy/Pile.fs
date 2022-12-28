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

#if WINDOWS_WPF || OPENSILVER
open System.Windows
#endif

/// <summary>
/// PileFactory class is obsoleted. Use Pile.Factory instead.
/// </summary>
[<Obsolete("PileFactory class is obsoleted. Use Pile.Factory instead.")>]
[<DebuggerStepThrough>]
[<AbstractClass; Sealed>]
type public PileFactory =

    /// <summary>
    /// PileFactory class is obsoleted. Use Pile.Factory instead.
    /// </summary>
    /// <returns>Pile instance</returns>
    [<Obsolete("PileFactory class is obsoleted. Use Pile.Factory instead.")>]
    static member create() =
        new Pile<UIElement>()

    /// <summary>
    /// PileFactory class is obsoleted. Use Pile.Factory instead.
    /// </summary>
    /// <typeparam name="'TUIElement">Target control type</typeparam>
    /// <returns>Pile instance</returns>
    [<Obsolete("PileFactory class is obsoleted. Use Pile.Factory instead.")>]
    static member create<'TUIElement when 'TUIElement :> UIElement>() =
        new Pile<'TUIElement>()

/// <summary>
/// The Pile factory.
/// </summary>
/// <remarks>You can manipulate XAML controls directly inside ViewModels
/// when places and binds both an Anchor (in XAML) and a Pile.</remarks>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public PileFactoryExtension =

    type public PileFactoryInstance with

        /// <summary>
        /// Create an anonymous control typed Pile.
        /// </summary>
        /// <returns>Pile instance</returns>
        member _.create() =
            new Pile<UIElement>()

        /// <summary>
        /// Create a Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">Target control type</typeparam>
        /// <returns>Pile instance</returns>
        member _.create<'TUIElement when 'TUIElement :> UIElement>() =
            new Pile<'TUIElement>()

/// <summary>
/// Pile manipulator class.
/// </summary>
/// <remarks>You can manipulate XAML controls directly inside ViewModels
/// when places and binds both an Anchor (in XAML) and a Pile.</remarks>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public PileExtension =

    type public Pile<'TUIElement when 'TUIElement :> UIElement> with

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">Target control type</typeparam>
        /// <param name="action">Predicts when rents control instance</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>Async&lt;unit&gt; instance</returns>
        member self.rentAsync (action: 'TUIElement -> Async<unit>, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalRentAsync(action >> asyncUnitAsValueTaskUnit |> asFunc1, canIgnore) |> valueTaskUnitAsAsyncResult

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">Target control type</typeparam>
        /// <typeparam name="'TResult">Action result type</typeparam>
        /// <param name="action">Predicts when rents control instance</param>
        /// <returns>Async result for action</returns>
        /// <remarks>This overload has to complete XAML data-binding.</remarks>
        member self.rentAsync (action: 'TUIElement -> Async<'TResult>) =
            self.InternalRentAsync<'TResult>(action >> asyncAsValueTask |> asFunc1) |> valueTaskAsAsyncResult
