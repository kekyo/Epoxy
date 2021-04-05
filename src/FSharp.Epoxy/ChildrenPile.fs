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

open System.Collections.Generic
open System.Diagnostics
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

/// <summary>
/// The ChildrenPile factory.
/// </summary>
/// <remarks>You can manipulate XAML controls directly inside ViewModels
/// when places and binds both a ChildrenAnchor (in XAML) and a ChildrenPile.</remarks>
[<DebuggerStepThrough>]
[<AbstractClass; Sealed>]
type public ChildrenPileFactory =

    /// <summary>
    /// Create an anonymous child control typed ChildrenPile.
    /// </summary>
    /// <returns>ChildrenPile instance</returns>
    static member create() =
        new ChildrenPile<UIElement>()

    /// <summary>
    /// Create a ChildrenPile.
    /// </summary>
    /// <typeparam name="'TUIElement">Target child control type</typeparam>
    /// <returns>ChildrenPile instance</returns>
    static member create<'TUIElement when 'TUIElement :> UIElement>() =
        new ChildrenPile<'TUIElement>()

/// <summary>
/// ChildrenPile manipulator class.
/// </summary>
/// <remarks>You can manipulate XAML controls directly inside ViewModels
/// when places and binds both an Anchor (in XAML) and a ChildrenPile.</remarks>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public ChildrenPileExtension =

    type public ChildrenPile<'TUIElement when 'TUIElement :> UIElement> with
        /// <summary>
        /// Manipulate anchoring element.
        /// </summary>
        /// <typeparam name="'TUIElement">Target child control type</typeparam>
        /// <param name="pile">ChildrenPile instance</param>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>Async&lt;unit&gt; instance</returns>
        member self.manipulateAsync (action: IList<'TUIElement> -> Async<unit>, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalManipulateAsync(action >> asyncUnitAsValueTaskUnit |> asFunc1, canIgnore) |> valueTaskUnitAsAsyncResult

        /// <summary>
        /// Manipulate anchoring element.
        /// </summary>
        /// <typeparam name="'TUIElement">Target child control type</typeparam>
        /// <typeparam name="'TResult">Action result type</typeparam>
        /// <param name="pile">ChildrenPile instance</param>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <returns>Async result for action</returns>
        /// <remarks>This overload has to complete XAML data-binding.</remarks>
        member self.manipulateAsync (action: IList<'TUIElement> -> Async<'TResult>) =
            self.InternalManipulateAsync<'TResult>(action >> asyncAsValueTask |> asFunc1) |> valueTaskAsAsyncResult
