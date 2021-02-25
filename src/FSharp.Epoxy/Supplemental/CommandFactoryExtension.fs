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

open Epoxy
open Epoxy.Internal

[<DebuggerStepThrough>]
[<AutoOpen>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module public CommandFactoryExtension =
    let fromAsync (propertyChanged:'TValue -> Async<unit>) =
        new Func<'TValue, ValueTask>(fun value ->
            InternalHelpers.FromTask(Async.StartImmediateAsTask(propertyChanged value) :> Task))

    type public CommandFactoryInstance with
        member __.create (executeAsync: unit -> ValueTask<unit>) =
            new DelegatedCommand(new Func<ValueTask>(fun () -> InternalHelpers.FromTask(executeAsync().AsTask() :> Task)))
        member __.create (executeAsync: unit -> ValueTask<unit>, canExecute: unit -> bool) =
            new DelegatedCommand(new Func<ValueTask>(fun () -> InternalHelpers.FromTask(executeAsync().AsTask() :> Task)), new Func<bool>(canExecute))

        member __.create<'TParameter> (executeAsync: 'TParameter -> ValueTask<unit>) =
            new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(fun parameter -> InternalHelpers.FromTask(executeAsync(parameter).AsTask() :> Task)))
        member __.create<'TParameter> (executeAsync: 'TParameter -> ValueTask<unit>, canExecute: 'TParameter -> bool) =
            new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(fun parameter -> InternalHelpers.FromTask(executeAsync(parameter).AsTask() :> Task)), new Func<'TParameter, bool>(canExecute))
        
        member __.create (executeAsync: unit -> Task) =
            new DelegatedCommand(new Func<ValueTask>(fun () -> InternalHelpers.FromTask(executeAsync())))
        member __.create (executeAsync: unit -> Task, canExecute: unit -> bool) =
            new DelegatedCommand(new Func<ValueTask>(fun () -> InternalHelpers.FromTask(executeAsync())), new Func<bool>(canExecute))

        member __.create<'TParameter> (executeAsync: 'TParameter -> Task) =
            new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(fun parameter -> InternalHelpers.FromTask(executeAsync parameter)))
        member __.create<'TParameter> (executeAsync: 'TParameter -> Task, canExecute: 'TParameter -> bool) =
            new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(fun parameter -> InternalHelpers.FromTask(executeAsync parameter)), new Func<'TParameter, bool>(canExecute))

        member __.create (executeAsync: unit -> Task<unit>) =
            new DelegatedCommand(new Func<ValueTask>(fun () -> InternalHelpers.FromTask(executeAsync() :> Task)))
        member __.create (executeAsync: unit -> Task<unit>, canExecute: unit -> bool) =
            new DelegatedCommand(new Func<ValueTask>(fun () -> InternalHelpers.FromTask(executeAsync() :> Task)), new Func<bool>(canExecute))

        member __.create<'TParameter> (executeAsync: 'TParameter -> Task<unit>) =
            new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(fun parameter -> InternalHelpers.FromTask(executeAsync parameter :> Task)))
        member __.create<'TParameter> (executeAsync: 'TParameter -> Task<unit>, canExecute: 'TParameter -> bool) =
            new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(fun parameter -> InternalHelpers.FromTask(executeAsync parameter :> Task)), new Func<'TParameter, bool>(canExecute))

        member __.create (executeAsync: unit -> Async<unit>) =
            new DelegatedCommand(new Func<ValueTask>(fun () -> InternalHelpers.FromTask(Async.StartImmediateAsTask(executeAsync()) :> Task)))
        member __.create (executeAsync: unit -> Async<unit>, canExecute: unit -> bool) =
            new DelegatedCommand(new Func<ValueTask>(fun () -> InternalHelpers.FromTask(Async.StartImmediateAsTask(executeAsync()) :> Task)), new Func<bool>(canExecute))

        member __.create<'TParameter> (executeAsync: 'TParameter -> Async<unit>) =
            new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(fun parameter -> InternalHelpers.FromTask(Async.StartImmediateAsTask(executeAsync parameter) :> Task)))
        member __.create<'TParameter> (executeAsync: 'TParameter -> Async<unit>, canExecute: 'TParameter -> bool) =
            new DelegatedCommand<'TParameter>(new Func<'TParameter, ValueTask>(fun parameter -> InternalHelpers.FromTask(Async.StartImmediateAsTask(executeAsync parameter) :> Task)), new Func<'TParameter, bool>(canExecute))
