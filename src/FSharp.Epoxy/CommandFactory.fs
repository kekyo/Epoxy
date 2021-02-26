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

open Epoxy.Internal
open Epoxy.Supplemental

open System
open System.Diagnostics
open System.Threading.Tasks

[<DebuggerStepThrough>]
[<AutoOpen>]
module private CommandFactoryGenerator =
    let inline create0 executeAsync = new DelegatedCommand(executeAsync) :> Command
    let inline create1 executeAsync canExecute = new DelegatedCommand(executeAsync, canExecute) :> Command
    let inline createP0 executeAsync = new DelegatedCommand<'TParameter>(executeAsync) :> Command
    let inline createP1 executeAsync canExecute = new DelegatedCommand<'TParameter>(executeAsync, canExecute) :> Command

[<DebuggerStepThrough>]
[<AbstractClass; Sealed>]
type public CommandFactory =
    static member create executeAsync =
        create0 (executeAsync >> valueTaskUnitAsValueTask |> asFunc0)
    static member create (executeAsync, canExecute) =
        create1 (executeAsync >> valueTaskUnitAsValueTask |> asFunc0) (canExecute |> asFunc0)

    static member create (executeAsync: 'TParameter -> ValueTask<unit>) =
        createP0 (executeAsync >> valueTaskUnitAsValueTask |> asFunc1)
    static member create (executeAsync: 'TParameter -> ValueTask<unit>, canExecute) =
        createP1 (executeAsync >> valueTaskUnitAsValueTask |> asFunc1) (canExecute |> asFunc1)

    static member create (executeAsync: unit -> Task<unit>) =
        create0 (executeAsync >> taskUnitAsValueTask |> asFunc0)
    static member create (executeAsync: unit -> Task<unit>, canExecute) =
        create1 (executeAsync >> taskUnitAsValueTask |> asFunc0) (canExecute |> asFunc0)

    static member create (executeAsync: 'TParameter -> Task<unit>) =
        createP0 (executeAsync >> taskUnitAsValueTask |> asFunc1)
    static member create (executeAsync: 'TParameter -> Task<unit>, canExecute) =
        createP1 (executeAsync >> taskUnitAsValueTask |> asFunc1) (canExecute |> asFunc1)

    static member create executeAsync =
        create0 (executeAsync >> asyncUnitAsValueTask |> asFunc0)
    static member create (executeAsync, canExecute) =
        create1 (executeAsync >> asyncUnitAsValueTask |> asFunc0) (canExecute |> asFunc0)

    static member create (executeAsync: 'TParameter -> Async<unit>) =
        createP0 (executeAsync >> asyncUnitAsValueTask |> asFunc1)
    static member create (executeAsync: 'TParameter -> Async<unit>, canExecute) =
        createP1 (executeAsync >> asyncUnitAsValueTask |> asFunc1) (canExecute |> asFunc1)

[<DebuggerStepThrough>]
[<AutoOpen>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module public CommandFactoryExtension =
    type public CommandFactoryInstance with
        member __.create executeAsync =
            create0 (executeAsync >> valueTaskUnitAsValueTask |> asFunc0)
        member __.create (executeAsync, canExecute) =
            create1 (executeAsync >> valueTaskUnitAsValueTask |> asFunc0) (canExecute |> asFunc0)

        member __.create (executeAsync: 'TParameter -> ValueTask<unit>) =
            createP0 (executeAsync >> valueTaskUnitAsValueTask |> asFunc1)
        member __.create (executeAsync: 'TParameter -> ValueTask<unit>, canExecute) =
            createP1 (executeAsync >> valueTaskUnitAsValueTask |> asFunc1) (canExecute |> asFunc1)

        member __.create (executeAsync: unit -> Task<unit>) =
            create0 (executeAsync >> taskUnitAsValueTask |> asFunc0)
        member __.create (executeAsync: unit -> Task<unit>, canExecute) =
            create1 (executeAsync >> taskUnitAsValueTask |> asFunc0) (canExecute |> asFunc0)

        member __.create (executeAsync: 'TParameter -> Task<unit>) =
            createP0 (executeAsync >> taskUnitAsValueTask |> asFunc1)
        member __.create (executeAsync: 'TParameter -> Task<unit>, canExecute: 'TParameter -> bool) =
            createP1 (executeAsync >> taskUnitAsValueTask |> asFunc1) (canExecute |> asFunc1)

        member __.create executeAsync =
            create0 (executeAsync >> asyncUnitAsValueTask |> asFunc0)
        member __.create (executeAsync, canExecute) =
            create1 (executeAsync >> asyncUnitAsValueTask |> asFunc0) (canExecute |> asFunc0)

        member __.create (executeAsync: 'TParameter -> Async<unit>) =
            createP0 (executeAsync >> asyncUnitAsValueTask |> asFunc1)
        member __.create (executeAsync: 'TParameter -> Async<unit>, canExecute) =
            createP1 (executeAsync >> asyncUnitAsValueTask |> asFunc1) (canExecute |> asFunc1)
