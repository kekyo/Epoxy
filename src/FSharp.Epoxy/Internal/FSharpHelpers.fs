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

namespace Epoxy.Internal

open System
open System.Diagnostics
open System.Threading.Tasks

[<DebuggerStepThrough>]
[<AutoOpen>]
module internal FSharpHelpers =
    let inline asFunc0 (f: unit -> 'TResult) = new Func<'TResult>(f)
    let inline asFunc1 (f: 'T0 -> 'TResult) = new Func<'T0, 'TResult>(f)
    let inline asFunc2 (f: 'T0 -> 'T1 -> 'TResult) = new Func<'T0, 'T1, 'TResult>(f)
    let inline asAction0 (f: unit -> unit) = new Action(f)
    let inline asAction1 (f: 'T0 -> unit) = new Action<'T0>(f)
    let inline asAction2 (f: 'T0 -> 'T1 -> unit) = new Action<'T0, 'T1>(f)

    let inline taskVoidAsValueTask (t: Task) = InternalHelpers.FromTask t
    let inline taskAsValueTask (t: Task<'TResult>) = InternalHelpers.FromTask t
    let inline taskUnitAsValueTask (t: Task<unit>) = InternalHelpers.FromTask(t :> Task)
    let inline valueTaskUnitAsValueTask (t: ValueTask<unit>) = InternalHelpers.FromTask(t.AsTask() :> Task)

    let inline asyncUnitAsValueTask (a: Async<unit>) = InternalHelpers.FromTask(a |> Async.StartImmediateAsTask :> Task)
    let inline asyncAsValueTask (a: Async<'TResult>) = InternalHelpers.FromTask(a |> Async.StartImmediateAsTask)

