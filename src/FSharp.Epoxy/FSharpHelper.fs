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

open Epoxy
open Epoxy.Internal

open System
open System.Threading.Tasks
open System.Diagnostics

#if WINDOWS_UWP || UNO
open Windows.UI.Xaml
#endif

#if WINUI
open Microsoft.UI.Xaml
#endif

#if WINDOWS_WPF || OPENSILVER
open System.Windows
open System.Windows.Threading

#endif

#if XAMARIN_FORMS
open Xamarin.Forms
#endif

#if AVALONIA
open Avalonia
open Avalonia.Threading
#endif

/// <summary>
/// Usable functions for F#.
/// </summary>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public FSharpHelper =

#if WINDOWS_WPF
    type Dispatcher with
    
        /// <summary>
        /// Execute function under the Dispatcher.
        /// </summary>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Function</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <remarks>Recommends using UIThread.bind function instead.</remarks>
        member dispatcher.invokeAsync (action: unit -> 'TResult) =
            dispatcher.InvokeAsync<'TResult>(action |> asFunc0).Task |> taskAsAsyncResult

        /// <summary>
        /// Execute function under the Dispatcher.
        /// </summary>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Function</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <remarks>Recommends using UIThread.bind function instead.</remarks>
        member dispatcher.invokeAsync (action: unit -> ValueTask<'TResult>) =
            dispatcher.Invoke(action >> valueTaskAsAsyncResult)

        /// <summary>
        /// Execute function under the Dispatcher.
        /// </summary>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Function</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <remarks>Recommends using UIThread.bind function instead.</remarks>
        member dispatcher.invokeAsync (action: unit -> Task<'TResult>) =
            dispatcher.Invoke(action) |> taskAsAsyncResult

        /// <summary>
        /// Execute function under the Dispatcher.
        /// </summary>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Function</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <remarks>Recommends using UIThread.bind function instead.</remarks>
        member dispatcher.invokeAsync (action: unit -> Async<'TResult>) =
            dispatcher.Invoke(action) |> asyncAsAsyncResult
#endif
#if OPENSILVER
    type Dispatcher with
    
        /// <summary>
        /// Execute function under the Dispatcher.
        /// </summary>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Function</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <remarks>Recommends using UIThread.bind function instead.</remarks>
        member dispatcher.invokeAsync (action: unit -> 'TResult) =
            invokeAsync dispatcher action |> taskAsAsyncResult

        /// <summary>
        /// Execute function under the Dispatcher.
        /// </summary>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Function</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <remarks>Recommends using UIThread.bind function instead.</remarks>
        member dispatcher.invokeAsync (action: unit -> ValueTask<'TResult>) =
            invokeAsync dispatcher action |> unwrapTaskAsValueTask

        /// <summary>
        /// Execute function under the Dispatcher.
        /// </summary>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Function</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <remarks>Recommends using UIThread.bind function instead.</remarks>
        member dispatcher.invokeAsync (action: unit -> Task<'TResult>) =
            invokeAsync dispatcher action |> unwrapTaskAsTask

        /// <summary>
        /// Execute function under the Dispatcher.
        /// </summary>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Function</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <remarks>Recommends using UIThread.bind function instead.</remarks>
        member dispatcher.invokeAsync (action: unit -> Async<'TResult>) =
            invokeAsync dispatcher action |> unwrapTaskAsAsyncResult
#endif
#if AVALONIA
    type Dispatcher with

        /// <summary>
        /// Execute function under the Dispatcher.
        /// </summary>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Function</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <remarks>Recommends using UIThread.bind function instead.</remarks>
        member dispatcher.invokeAsync (action: unit -> ValueTask<'TResult>) =
            dispatcher.InvokeAsync<'TResult>(action >> valueTaskAsTask) |> taskAsAsyncResult

        /// <summary>
        /// Execute function under the Dispatcher.
        /// </summary>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Function</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <remarks>Recommends using UIThread.bind function instead.</remarks>
        member dispatcher.invokeAsync (action: unit -> Task<'TResult>) =
            dispatcher.InvokeAsync<'TResult>(action) |> taskAsAsyncResult

        /// <summary>
        /// Execute function under the Dispatcher.
        /// </summary>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Function</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <remarks>Recommends using UIThread.bind function instead.</remarks>
        member dispatcher.invokeAsync (action: unit -> Async<'TResult>) =
            dispatcher.InvokeAsync<'TResult>(action >> Async.StartImmediateAsTask) |> taskAsAsyncResult
#endif
