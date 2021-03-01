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

open Epoxy.Internal

open System
open System.Diagnostics
open System.Threading.Tasks

[<DebuggerStepThrough>]
[<AutoOpen>]
module internal InternalFSharpHelpers =
    let inline asFunc0 (f: unit -> 'TResult) = new Func<'TResult>(f)
    let inline asFunc1 (f: 'T0 -> 'TResult) = new Func<'T0, 'TResult>(f)
    let inline asFunc2 (f: 'T0 -> 'T1 -> 'TResult) = new Func<'T0, 'T1, 'TResult>(f)
    let inline asAction0 (f: unit -> unit) = new Action(f)
    let inline asAction1 (f: 'T0 -> unit) = new Action<'T0>(f)
    let inline asAction2 (f: 'T0 -> 'T1 -> unit) = new Action<'T0, 'T1>(f)

    let inline valueTaskVoidAsValueTaskUnit (t: ValueTask) = t |> InternalHelpers.AsValueTaskUnit
    let inline valueTaskUnitAsValueTaskUnit (t: ValueTask<unit>) = t |> InternalHelpers.AsValueTaskUnit
    let inline taskVoidAsValueTaskUnit (t: Task) = t |> InternalHelpers.AsValueTaskUnit
    let inline taskUnitAsValueTaskUnit (t: Task<unit>) = t |> InternalHelpers.AsValueTaskUnit
    let inline taskAsValueTask (t: Task<'TResult>) = t |> InternalHelpers.AsValueTask

    let inline asyncUnitAsValueTaskUnit (a: Async<unit>) = Async.StartImmediateAsTask(a) |> InternalHelpers.AsValueTaskUnit
    let inline asyncAsValueTask (a: Async<'TResult>) = Async.StartImmediateAsTask(a) |> InternalHelpers.AsValueTask
    
    let inline valueTaskAsTask (t: ValueTask<'TResult>) = t.AsTask()
    let inline unitAsValueTaskUnit (_: unit) = InternalHelpers.FromResult<Unit>(new Unit())

#if INTEROP_VALUE_TASK
    let inline taskAsAsyncResult (t: Task<'TResult>) = t |> InternalHelpers.AsValueTask
    let inline valueTaskAsAsyncResult (t: ValueTask<'TResult>) = t
    let inline valueTaskUnitAsAsyncResult (t: ValueTask<Unit>) = InternalHelpers.MapAsValueTask(t, fun _ -> ())
    let inline asyncAsAsyncResult (a: Async<'TResult>) = Async.StartImmediateAsTask(a) |> InternalHelpers.AsValueTask

    let inline unwrap (a: ValueTask<ValueTask<'TResult>>) =
        async {
            let! r = a.AsTask() |> Async.AwaitTask
            return! r.AsTask() |> Async.AwaitTask
        } |> Async.StartImmediateAsTask |> InternalHelpers.AsValueTask
#else
    let inline taskAsAsyncResult (t: Task<'TResult>) = t |> Async.AwaitTask
    let inline valueTaskAsAsyncResult (t: ValueTask<'TResult>) = t.AsTask() |> Async.AwaitTask
    let inline valueTaskUnitAsAsyncResult (t: ValueTask<Unit>) = InternalHelpers.MapAsTask(t, fun _ -> ()) |> Async.AwaitTask
    let inline asyncAsAsyncResult (a: Async<'TResult>) = a

    let inline unwrap (a: ValueTask<ValueTask<'TResult>>) =
        async {
            let! r = a.AsTask() |> Async.AwaitTask
            return! r.AsTask() |> Async.AwaitTask
        }
#endif
