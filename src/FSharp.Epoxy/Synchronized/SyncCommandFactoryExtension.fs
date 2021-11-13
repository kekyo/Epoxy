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
open Epoxy.Internal

/// <summary>
/// Command factory functions for synchronous handler.
/// </summary>
/// <remarks>Notice: They handle with synchronous handler. You can use asynchronous version instead.</remarks>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public SyncCommandFactoryExtension =

    type public CommandFactoryInstance with
        /// <summary>
        /// Generate a Command instance with synchronous handler.
        /// </summary>
        /// <param name="execute">Synchronous handler</param>
        /// <returns>A Command instance</returns>
        /// <remarks>Notice: It handles with synchronous handler. You can use asynchronous version instead.</remarks>
        member __.createSync (execute: unit -> unit) =
            new SyncDelegatedCommand(execute |> asActionVoid) :> Command

        /// <summary>
        /// Generate a Command instance with synchronous handler.
        /// </summary>
        /// <param name="execute">Synchronous handler</param>
        /// <param name="canExecute">Responder for be able to execute</param>
        /// <returns>A Command instance</returns>
        /// <remarks>Notice: It handles with synchronous handler. You can use asynchronous version instead.</remarks>
        member __.createSync (execute: unit -> unit, canExecute: unit -> bool) =
            new SyncDelegatedCommand(execute |> asActionVoid, canExecute |> asFunc0) :> Command

        /// <summary>
        /// Generate a Command instance with synchronous handler.
        /// </summary>
        /// <typeparam name="'TParameter">Handler parameter type</typeparam>
        /// <param name="execute">Synchronous handler</param>
        /// <returns>A Command instance</returns>
        /// <remarks>Notice: It handles with synchronous handler. You can use asynchronous version instead.</remarks>
        member __.createSync (execute: 'TParameter -> unit) =
            new SyncDelegatedCommand<'TParameter>(execute |> asAction1) :> Command

        /// <summary>
        /// Generate a Command instance with synchronous handler.
        /// </summary>
        /// <typeparam name="'TParameter">Handler parameter type</typeparam>
        /// <param name="execute">Synchronous handler</param>
        /// <param name="canExecute">Responder for be able to execute</param>
        /// <returns>A Command instance</returns>
        /// <remarks>Notice: It handles with synchronous handler. You can use asynchronous version instead.</remarks>
        member __.createSync (execute: 'TParameter -> unit, canExecute: 'TParameter -> bool) =
            new SyncDelegatedCommand<'TParameter>(execute |> asAction1, canExecute |> asFunc1) :> Command

        // Dodge mistake choicing asynchronously overloads
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: unit -> ValueTask) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: unit -> ValueTask, canExecute: unit -> bool) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: 'TParameter -> ValueTask) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: 'TParameter -> ValueTask, canExecute: 'TParameter -> bool) =
            raise(InvalidOperationException("Use createAsync instead."))

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: unit -> ValueTask<unit>) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: unit -> ValueTask<unit>, canExecute: unit -> bool) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: 'TParameter -> ValueTask<unit>) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: 'TParameter -> ValueTask<unit>, canExecute: 'TParameter -> bool) =
            raise(InvalidOperationException("Use createAsync instead."))

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: unit -> Task) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: unit -> Task, canExecute: unit -> bool) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: 'TParameter -> Task) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: 'TParameter -> Task, canExecute: 'TParameter -> bool) =
            raise(InvalidOperationException("Use createAsync instead."))

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: unit -> Task<unit>) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: unit -> Task<unit>, canExecute: unit -> bool) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: 'TParameter -> Task<unit>) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: 'TParameter -> Task<unit>, canExecute: 'TParameter -> bool) =
            raise(InvalidOperationException("Use createAsync instead."))

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: unit -> Async<unit>) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: unit -> Async<unit>, canExecute: unit -> bool) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: 'TParameter -> Async<unit>) =
            raise(InvalidOperationException("Use createAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use createAsync instead.", true)>]
        member __.createSync (execute: 'TParameter -> Async<unit>, canExecute: 'TParameter -> bool) =
            raise(InvalidOperationException("Use createAsync instead."))
