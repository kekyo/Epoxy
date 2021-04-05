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

open Epoxy.Advanced
open Epoxy.Internal

open System
open System.ComponentModel
open System.Diagnostics
open System.Runtime.InteropServices

/// <summary>
/// GlobalService is a simple and lightweight dependency injection infrastructure.
/// </summary>
/// <remarks>Notice: They handle with synchronous handler.
/// You can use asynchronous version instead.</remarks>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public SyncGlobalServiceAccessorExtension =

    type public GlobalServiceAccessor with

        /// <summary>
        /// Execute target interface type synchronously.
        /// </summary>
        /// <typeparam name="'TService">Target interface type</typeparam>
        /// <param name="accessor">Accessor instance (will use only fixup by compiler)</param>
        /// <param name="action">Synchronous continuation delegate</param>
        /// <param name="ignoreNotPresent">Ignore if didn't presend target instance.</param>
        /// <remarks>Notice: They handle with synchronous handler.
        /// You can use asynchronous version instead.</remarks>
        member __.executeSync (action: 'TService -> unit, [<Optional; DefaultParameterValue(false)>] ignoreNotPresent) =
            InternalGlobalService.ExecuteSync(action |> asAction1, ignoreNotPresent)

        /// <summary>
        /// Execute target interface type synchronously.
        /// </summary>
        /// <typeparam name="'TService">Target interface type</typeparam>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="accessor">Accessor instance (will use only fixup by compiler)</param>
        /// <param name="action">Synchronous continuation delegate</param>
        /// <returns>Result value</returns>
        /// <remarks>Notice: They handle with synchronous handler.
        /// You can use asynchronous version instead.</remarks>
        member __.executeSync (action: 'TService -> 'TResult) =
            InternalGlobalService.ExecuteSync(action |> asFunc1)

        // Dodge mistake choicing asynchronously overloads
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use executeAsync instead.", true)>]
        member __.executeSync (action: 'TService -> Async<unit>, [<Optional; DefaultParameterValue(false)>] ignoreNotPresent) =
            raise(InvalidOperationException("Use executeAsync instead."))
        [<EditorBrowsable(EditorBrowsableState.Never)>]
        [<Obsolete("Use executeAsync instead.", true)>]
        member __.executeSync (action: 'TService -> Async<'TResult>) =
            raise(InvalidOperationException("Use executeAsync instead."))
