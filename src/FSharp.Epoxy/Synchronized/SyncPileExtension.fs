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

#if XAMARIN_FORMS
type UIElement = Xamarin.Forms.Element
#endif

#if AVALONIA
type UIElement = Avalonia.IStyledElement
#endif

[<DebuggerStepThrough>]
[<AutoOpen>]
module public SyncPileExtension =
    type public Pile<'TUIElement when 'TUIElement :> UIElement> with
        member pile.executeSync(action: 'TUIElement -> unit, [<Optional; DefaultParameterValue(false)>] canIgnore) =
            pile.InternalExecuteSync(action |> asAction1, canIgnore)

        member pile.executeSync(action: 'TUIElement -> 'TResult) =
            pile.InternalExecuteSync(action |> asFunc1)

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
