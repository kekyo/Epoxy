﻿////////////////////////////////////////////////////////////////////////////
//
// Epoxy - An independent flexible XAML MVVM library for .NET
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
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
module internal InternalFSharpHelper =
    let inline asFunc0 (f: unit -> 'TResult) = new Func<'TResult>(f)
    let inline asFunc1 (f: 'T0 -> 'TResult) = new Func<'T0, 'TResult>(f)
    let inline asFunc2 (f: 'T0 -> 'T1 -> 'TResult) = new Func<'T0, 'T1, 'TResult>(f)
    let inline asActionVoid (f: unit -> unit) = new Action(f)
    let inline asAction1 (f: 'T0 -> unit) = new Action<'T0>(f)
    let inline asAction2 (f: 'T0 -> 'T1 -> unit) = new Action<'T0, 'T1>(f)

    let inline valueTaskVoidAsValueTaskUnit (t: ValueTask) = t |> InternalHelpers.AsValueTaskUnit
    let inline valueTaskUnitAsValueTaskUnit (t: ValueTask<unit>) = t |> InternalHelpers.AsValueTaskUnit
    let inline valueTaskUnitAsValueTaskVoid (t: ValueTask<unit>) = t |> InternalHelpers.AsValueTaskVoid
    let inline taskVoidAsValueTaskUnit (t: Task) = t |> InternalHelpers.AsValueTaskUnit
    let inline taskVoidAsValueTaskVoid (t: Task) = t |> InternalHelpers.AsValueTaskVoid
    let inline taskUnitAsValueTaskUnit (t: Task<unit>) = t |> InternalHelpers.AsValueTaskUnit
    let inline taskUnitAsValueTaskVoid (t: Task<unit>) = t |> InternalHelpers.AsValueTaskVoid
    let inline taskAsValueTask (t: Task<'TResult>) = t |> InternalHelpers.AsValueTask

    let inline asyncUnitAsValueTaskUnit (a: Async<unit>) = Async.StartImmediateAsTask(a) |> InternalHelpers.AsValueTaskUnit
    let inline asyncUnitAsValueTaskVoid (a: Async<unit>) = Async.StartImmediateAsTask(a) |> InternalHelpers.AsValueTaskVoid
    let inline asyncAsValueTask (a: Async<'TResult>) = Async.StartImmediateAsTask(a) |> InternalHelpers.AsValueTask
    
    let inline valueTaskAsTask (t: ValueTask<'TResult>) = t.AsTask()
    let inline unitAsValueTaskUnit (_: unit) = InternalHelpers.FromResult<Unit>(new Unit())

#if INTEROP_VALUE_TASK
    let inline taskAsAsyncResult (t: Task<'TResult>) = t |> InternalHelpers.AsValueTask
    let inline valueTaskVoidAsAsyncResult (t: ValueTask) = InternalHelpers.MapAsValueTask(t, fun () -> ())
    let inline valueTaskAsAsyncResult (t: ValueTask<'TResult>) = t
    let inline valueTaskUnitAsAsyncResult (t: ValueTask<Unit>) = InternalHelpers.MapAsValueTask(t, fun _ -> ())
    let inline asyncAsAsyncResult (a: Async<'TResult>) = Async.StartImmediateAsTask(a) |> InternalHelpers.AsValueTask

    let inline unwrap (a: Async<Async<'TResult>>) =
        async {
            let! r = a
            return! r
        } |> Async.StartImmediateAsTask |> InternalHelpers.AsValueTask
    let inline unwrapTaskAsAsyncResult (a: Task<Async<'TResult>>) =
        async {
            let! r = a |> Async.AwaitTask
            return! r
        } |> Async.StartImmediateAsTask |> InternalHelpers.AsValueTask
    let inline unwrapTaskAsTask (a: Task<Task<'TResult>>) =
        async {
            let! r = a |> Async.AwaitTask
            return! r |> Async.AwaitTask
        } |> Async.StartImmediateAsTask |> InternalHelpers.AsValueTask
    let inline unwrapTaskAsValueTask (a: Task<ValueTask<'TResult>>) =
        async {
            let! r = a |> Async.AwaitTask
            return! r.AsTask() |> Async.AwaitTask
        } |> Async.StartImmediateAsTask |> InternalHelpers.AsValueTask
    let inline unwrapValueTaskAsValueTask (a: ValueTask<ValueTask<'TResult>>) =
        async {
            let! r = a.AsTask() |> Async.AwaitTask
            return! r.AsTask() |> Async.AwaitTask
        } |> Async.StartImmediateAsTask |> InternalHelpers.AsValueTask
#else
    let inline taskAsAsyncResult (t: Task<'TResult>) = t |> Async.AwaitTask
    let inline valueTaskVoidAsAsyncResult (t: ValueTask) = t.AsTask() |> Async.AwaitTask
    let inline valueTaskAsAsyncResult (t: ValueTask<'TResult>) = t.AsTask() |> Async.AwaitTask
    let inline valueTaskUnitAsAsyncResult (t: ValueTask<Unit>) = InternalHelpers.MapAsTask(t, fun _ -> ()) |> Async.AwaitTask
    let inline asyncAsAsyncResult (a: Async<'TResult>) = a

    let inline unwrap (a: Async<Async<'TResult>>) =
        async {
            let! r = a
            return! r
        }
    let inline unwrapTaskAsAsyncResult (a: Task<Async<'TResult>>) =
        async {
            let! r = a |> Async.AwaitTask
            return! r
        }
    let inline unwrapTaskAsTask (a: Task<Task<'TResult>>) =
        async {
            let! r = a |> Async.AwaitTask
            return! r |> Async.AwaitTask
        }
    let inline unwrapTaskAsValueTask (a: Task<ValueTask<'TResult>>) =
        async {
            let! r = a |> Async.AwaitTask
            return! r.AsTask() |> Async.AwaitTask
        }
    let inline unwrapValueTaskAsValueTask (a: ValueTask<ValueTask<'TResult>>) =
        async {
            let! r = a.AsTask() |> Async.AwaitTask
            return! r.AsTask() |> Async.AwaitTask
        }
#endif

#if OPENSILVER
    let inline invokeAsync (dispatcher: System.Windows.Threading.Dispatcher) (action: unit -> 'TResult) =
        let tcs = new TaskCompletionSource<'TResult>();
        dispatcher.BeginInvoke(fun () ->
            try
                tcs.TrySetResult(action()) |> ignore
            with
            | ex -> tcs.TrySetException(ex) |> ignore
        ) |> ignore
        tcs.Task
#endif
