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

open System
open System.ComponentModel
open System.Diagnostics

[<DebuggerStepThrough>]
[<AbstractClass>]
[<Sealed>]
type public UIThread =
    /// <summary>
    /// Detects current thread context on the UI thread.
    /// </summary>
    static member isBound =
        InternalUIThread.IsBound

    /// <summary>
    /// Detects current thread context on the UI thread.
    /// </summary>
    /// <remarks>This function is used internal only.
    /// You may have to use isBound property instead.</remarks>
    [<EditorBrowsable(EditorBrowsableState.Never)>]
    static member unsafeIsBound() =
        InternalUIThread.UnsafeIsBound()

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
        Async.FromContinuations(fun (resolve, _, _) ->
            InternalUIThread.ContinueOnUIThread(new Action(resolve)))
