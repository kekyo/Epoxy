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

open System
open System.ComponentModel
open System.Diagnostics
open System.Threading.Tasks

open Epoxy

[<DebuggerStepThrough>]
[<AutoOpen>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module public CommandFactoryExtension =
    type public CommandFactoryInstance with
        member __.createSync (execute: unit -> unit) =
            new SyncDelegatedCommand(new Action(execute))
        member __.createSync (execute: unit -> unit, canExecute: unit -> bool) =
            new SyncDelegatedCommand(new Action(execute), new Func<bool>(canExecute))
        member __.createSync<'TParameter> (execute: 'TParameter -> unit) =
            new SyncDelegatedCommand<'TParameter>(new Action<'TParameter>(execute))
        member __.createSync<'TParameter> (execute: 'TParameter -> unit, canExecute: 'TParameter -> bool) =
            new SyncDelegatedCommand<'TParameter>(new Action<'TParameter>(execute), new Func<'TParameter, bool>(canExecute))

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync (execute: unit -> ValueTask) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync (execute: unit -> ValueTask, canExecute: unit -> bool) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync<'TParameter> (execute: 'TParameter -> ValueTask) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync<'TParameter> (execute: 'TParameter -> ValueTask, canExecute: 'TParameter -> bool) =
            raise(InvalidOperationException("Use setValueAsync instead."))

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync (execute: unit -> ValueTask<unit>) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync (execute: unit -> ValueTask<unit>, canExecute: unit -> bool) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync<'TParameter> (execute: 'TParameter -> ValueTask<unit>) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync<'TParameter> (execute: 'TParameter -> ValueTask<unit>, canExecute: 'TParameter -> bool) =
            raise(InvalidOperationException("Use setValueAsync instead."))

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync (execute: unit -> Task) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync (execute: unit -> Task, canExecute: unit -> bool) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync<'TParameter> (execute: 'TParameter -> Task) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync<'TParameter> (execute: 'TParameter -> Task, canExecute: 'TParameter -> bool) =
            raise(InvalidOperationException("Use setValueAsync instead."))

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync (execute: unit -> Task<unit>) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync (execute: unit -> Task<unit>, canExecute: unit -> bool) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync<'TParameter> (execute: 'TParameter -> Task<unit>) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync<'TParameter> (execute: 'TParameter -> Task<unit>, canExecute: 'TParameter -> bool) =
            raise(InvalidOperationException("Use setValueAsync instead."))

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync (execute: unit -> Async<unit>) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync (execute: unit -> Async<unit>, canExecute: unit -> bool) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync<'TParameter> (execute: 'TParameter -> Async<unit>) =
            raise(InvalidOperationException("Use setValueAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use setValueAsync instead.", true)>]
        member __.createSync<'TParameter> (execute: 'TParameter -> Async<unit>, canExecute: 'TParameter -> bool) =
            raise(InvalidOperationException("Use setValueAsync instead."))
