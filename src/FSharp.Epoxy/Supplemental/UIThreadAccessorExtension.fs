////////////////////////////////////////////////////////////////////////////
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

namespace Epoxy.Supplemental

open Epoxy

open System.Diagnostics
open System.Threading.Tasks

/// <summary>
/// UI thread commonly manipulator.
/// </summary>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public UIThreadAccessorExtension =

    type public UIThreadAccessorInstance with

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="action">Action on UI thread context</param>
        member __.invokeAsync (action:unit -> Task) = async {
            do! UIThread.bind()
            return! action() |> Async.AwaitTask
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="accessor">UIThread accessor</param>
        /// <param name="action">Action on UI thread context</param>
        /// <returns>True if executed.</returns>
        member __.tryInvokeAsync (action:unit -> Task) = async {
            let! isBound = UIThread.tryBind()
            if isBound then
                do! action() |> Async.AwaitTask
                return true
            else
                return false
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="action">Action on UI thread context</param>
        member __.invokeAsync (action:unit -> Task<'T>) = async {
            do! UIThread.bind()
            return! action() |> Async.AwaitTask
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="accessor">UIThread accessor</param>
        /// <param name="action">Action on UI thread context</param>
        /// <returns>True if executed.</returns>
        member __.tryInvokeAsync (action:unit -> Task<'T>) = async {
            let! isBound = UIThread.tryBind()
            if isBound then
                let! r = action() |> Async.AwaitTask
                return (true, r)
            else
                return (false, Unchecked.defaultof<'T>)
        }

        ////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="action">Action on UI thread context</param>
        member __.invokeAsync (action:unit -> ValueTask) = async {
            do! UIThread.bind()
            return! action().AsTask() |> Async.AwaitTask
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="accessor">UIThread accessor</param>
        /// <param name="action">Action on UI thread context</param>
        /// <returns>True if executed.</returns>
        member __.tryInvokeAsync (action:unit -> ValueTask) = async {
            let! isBound = UIThread.tryBind()
            if isBound then
                do! action().AsTask() |> Async.AwaitTask
                return true
            else
                return false
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="action">Action on UI thread context</param>
        member __.invokeAsync (action:unit -> ValueTask<'T>) = async {
            do! UIThread.bind()
            return! action().AsTask() |> Async.AwaitTask
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="accessor">UIThread accessor</param>
        /// <param name="action">Action on UI thread context</param>
        /// <returns>True if executed.</returns>
        member __.tryInvokeAsync (action:unit -> ValueTask<'T>) = async {
            let! isBound = UIThread.tryBind()
            if isBound then
                let! r = action().AsTask() |> Async.AwaitTask
                return (true, r)
            else
                return (false, Unchecked.defaultof<'T>)
        }
