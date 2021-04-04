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

namespace Epoxy.Synchronized

open Epoxy
open Epoxy.Internal

open System
open System.ComponentModel
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
open System.Runtime.InteropServices
#endif

/// <summary>
/// Pile functions for synchronous execution.
/// </summary>
/// <remarks>You can manipulate XAML controls directly inside ViewModels
/// when places and binds both an Anchor (in XAML) and a Pile.
/// 
/// Notice: They handle with synchronous handler.
/// You can use asynchronous version instead.</remarks>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public SyncPileExtension =

    type public Pile<'TUIElement when 'TUIElement :> UIElement> with
        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <param name="action">Synchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <remarks>Notice: It handles with synchronous handler. You can use asynchronous version instead.</remarks>
        member pile.executeSync(action: 'TUIElement -> unit, [<Optional; DefaultParameterValue(false)>] canIgnore) =
            pile.InternalExecuteSync(action |> asAction1, canIgnore)

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="'TUIElement">UI element type</typeparam>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Synchronous continuation delegate</param>
        /// <returns>Result value</returns>
        /// <remarks>Notice: It handles with synchronous handler. You can use asynchronous version instead.</remarks>
        member pile.executeSync(action: 'TUIElement -> 'TResult) =
            pile.InternalExecuteSync(action |> asFunc1)

        // Dodge mistake choicing asynchronously overloads
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use executeAsync instead.", true)>]
        member __.executeSync(action: 'TUIElement -> Async<'TResult>) =
            raise (InvalidOperationException("Use executeAsync instead."))

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use executeAsync instead.", true)>]
        member __.executeSync(action: 'TUIElement -> Task, [<Optional; DefaultParameterValue(false)>] canIgnore) =
            raise (InvalidOperationException("Use executeAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use executeAsync instead.", true)>]
        member __.executeSync(action: 'TUIElement -> Task<'TResult>) =
            raise (InvalidOperationException("Use executeAsync instead."))

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use executeAsync instead.", true)>]
        member __.executeSync(action: 'TUIElement -> ValueTask, [<Optional; DefaultParameterValue(false)>] canIgnore) =
            raise (InvalidOperationException("Use executeAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use executeAsync instead.", true)>]
        member __.executeSync(action: 'TUIElement -> ValueTask<'TResult>) =
            raise (InvalidOperationException("Use executeAsync instead."))
