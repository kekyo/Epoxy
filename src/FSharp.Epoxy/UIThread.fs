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

namespace Epoxy

open Epoxy.Internal

open System
open System.Diagnostics

[<DebuggerStepThrough>]
[<AbstractClass>]
[<Sealed>]
type public UIThread =
    /// <summary>
    /// Detects current thread context on the UI thread.
    /// </summary>
    static member isBound() : Async<bool> =
        InternalUIThread.IsBoundAsync().AsTask() |> Async.AwaitTask

    /// <summary>
    /// Binds current async workflow to the UI thread context manually.
    /// </summary>
    /// <returns>Async object for the UI thread continuation.</returns>
    /// <example>
    /// <code>
    /// async {
    ///   // (On the arbitrary thread context here)
    /// 
    ///   // Switch to UI thread context uses async-await.
    ///   do! UIThread.bind()
    /// 
    ///   // (On the UI thread context here)
    /// }
    /// </code>
    /// </example>
    static member bind() : Async<unit> =
        Async.FromContinuations(fun (resolve, reject, _) ->
            InternalUIThread.ContinueOnUIThread(
                new Action<bool>(fun isBound ->
                    if isBound then
                        resolve()
                    else
                        reject (InvalidOperationException "Epoxy: Could not bind to UI thread. UI thread is not found."))))

    /// <summary>
    /// Binds current async workflow to the UI thread context manually.
    /// </summary>
    /// <returns>Async object for the UI thread continuation.</returns>
    /// <example>
    /// <code>
    /// async {
    ///   // (On the arbitrary thread context here)
    /// 
    ///   // Switch to UI thread context uses async-await.
    ///   let! isBound = UIThread.tryBind()
    ///   if not isBound then
    ///     // Failed to bind (UI thread is not found, maybe reason is UI shutdown)
    ///   else
    ///     // (On the UI thread context here)
    /// }
    /// </code>
    /// </example>
    static member tryBind() : Async<bool> =
        Async.FromContinuations(fun (resolve, _, _) ->
            InternalUIThread.ContinueOnUIThread(new Action<bool>(resolve)))

    /// <summary>
    /// Unbinds current UI thread context to the worker thread context manually.
    /// </summary>
    /// <returns>Async object for the worker thread continuation.</returns>
    /// <example>
    /// <code>
    /// async {
    ///   // (On the UI thread context here)
    /// 
    ///   // Switch to worker thread context uses async-await.
    ///   do! UIThread.unbind()
    /// 
    ///   // (On the worker thread context here)
    /// }
    /// </code>
    /// </example>
    static member unbind() : Async<unit> =
        Async.FromContinuations(fun (resolve, _, _) ->
            InternalUIThread.ContinueOnWorkerThread(new Action(resolve)))
