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

open Epoxy

open System.Diagnostics

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
        member __.invokeAsync (action:unit -> Async<'T>) = async {
            do! UIThread.bind()
            return! action()
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="accessor">UIThread accessor</param>
        /// <param name="action">Action on UI thread context</param>
        /// <returns>True if executed.</returns>
        member __.tryInvokeAsync (action:unit -> Async<'T>) = async {
            let! isBound = UIThread.tryBind()
            if isBound then
                let! r = action()
                return (true, r)
            else
                return (false, Unchecked.defaultof<'T>)
        }
