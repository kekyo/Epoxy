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

namespace Epoxy.Supplemental

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

#if NOESIS
open Noesis
#endif

open Epoxy
open Epoxy.Internal

/// <summary>
/// Pile functions for ValueTask/Task based asynchronous execution.
/// </summary>
/// <remarks>You can manipulate XAML controls directly inside ViewModels
/// when places and binds both an Anchor (in XAML) and a Pile.
/// 
/// Notice: They handle with synchronous handler.
/// You can use asynchronous version instead.</remarks>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public PileExtension =
    type public Pile<'TUIElement when 'TUIElement :> UIElement> with

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>Async instance</returns>
        member self.rentAsync (action: 'TUIElement -> ValueTask<unit>, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalRentAsync(action >> valueTaskUnitAsValueTaskUnit |> asFunc1, canIgnore) |> valueTaskUnitAsAsyncResult

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>Async instance</returns>
        member self.rentAsync (action: 'TUIElement -> Task<unit>, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalRentAsync(action >> taskUnitAsValueTaskUnit |> asFunc1, canIgnore) |> valueTaskUnitAsAsyncResult

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <returns>Async instance</returns>
        member self.rentAsync (action: 'TUIElement -> ValueTask<'TResult>) =
            self.InternalRentAsync<'TResult>(action |> asFunc1) |> valueTaskAsAsyncResult

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <returns>Async instance</returns>
        member self.rentAsync (action: 'TUIElement -> Task<'TResult>) =
            self.InternalRentAsync<'TResult>(action >> taskAsValueTask |> asFunc1) |> valueTaskAsAsyncResult

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>Async instance</returns>
        member self.rentAsync (action: 'TUIElement -> ValueTask, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalRentAsync(action >> valueTaskVoidAsValueTaskUnit |> asFunc1, canIgnore)

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>Async instance</returns>
        member self.rentAsync (action: 'TUIElement -> Task, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalRentAsync(action >> taskVoidAsValueTaskUnit |> asFunc1, canIgnore)

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>Async instance</returns>
        [<Obsolete("Use rentAsync instead.")>]
        member self.executeAsync (action: 'TUIElement -> ValueTask<unit>, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalRentAsync(action >> valueTaskUnitAsValueTaskUnit |> asFunc1, canIgnore) |> valueTaskUnitAsAsyncResult

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>Async instance</returns>
        [<Obsolete("Use rentAsync instead.")>]
        member self.executeAsync (action: 'TUIElement -> Task<unit>, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalRentAsync(action >> taskUnitAsValueTaskUnit |> asFunc1, canIgnore) |> valueTaskUnitAsAsyncResult

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <returns>Async instance</returns>
        [<Obsolete("Use rentAsync instead.")>]
        member self.executeAsync (action: 'TUIElement -> ValueTask<'TResult>) =
            self.InternalRentAsync<'TResult>(action |> asFunc1) |> valueTaskAsAsyncResult

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <returns>Async instance</returns>
        [<Obsolete("Use rentAsync instead.")>]
        member self.executeAsync (action: 'TUIElement -> Task<'TResult>) =
            self.InternalRentAsync<'TResult>(action >> taskAsValueTask |> asFunc1) |> valueTaskAsAsyncResult

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>Async instance</returns>
        [<Obsolete("Use rentAsync instead.")>]
        member self.executeAsync (action: 'TUIElement -> ValueTask, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalRentAsync(action >> valueTaskVoidAsValueTaskUnit |> asFunc1, canIgnore)

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>Async instance</returns>
        [<Obsolete("Use rentAsync instead.")>]
        member self.executeAsync (action: 'TUIElement -> Task, [<Optional; DefaultParameterValue(false)>] canIgnore: bool) =
            self.InternalRentAsync(action >> taskVoidAsValueTaskUnit |> asFunc1, canIgnore)
