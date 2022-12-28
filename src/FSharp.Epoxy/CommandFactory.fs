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

/// <summary>
/// CommandFactory class is obsoleted. Use Command.Factory instead.
/// </summary>
[<Obsolete("CommandFactory class is obsoleted. Use Command.Factory instead.")>]
[<DebuggerStepThrough>]
[<AbstractClass>]
[<Sealed>]
type public CommandFactory =
    /// <summary>
    /// CommandFactory class is obsoleted. Use Command.Factory instead.
    /// </summary>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <returns>A Command instance</returns>
    [<Obsolete("CommandFactory class is obsoleted. Use Command.Factory instead.")>]
    static member create executeAsync =
        create0 (executeAsync >> asyncUnitAsValueTaskUnit |> asFunc0)

    /// <summary>
    /// CommandFactory class is obsoleted. Use Command.Factory instead.
    /// </summary>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <param name="canExecute">Responder for be able to execute</param>
    /// <returns>A Command instance</returns>
    [<Obsolete("CommandFactory class is obsoleted. Use Command.Factory instead.")>]
    static member create (executeAsync, canExecute) =
        create1 (executeAsync >> asyncUnitAsValueTaskUnit |> asFunc0) (canExecute |> asFunc0)

    /// <summary>
    /// CommandFactory class is obsoleted. Use Command.Factory instead.
    /// </summary>
    /// <typeparam name="'TParameter">Handler parameter type</typeparam>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <returns>A Command instance</returns>
    [<Obsolete("CommandFactory class is obsoleted. Use Command.Factory instead.")>]
    static member create (executeAsync: 'TParameter -> Async<unit>) =
        createP0 (executeAsync >> asyncUnitAsValueTaskUnit |> asFunc1)

    /// <summary>
    /// CommandFactory class is obsoleted. Use Command.Factory instead.
    /// </summary>
    /// <typeparam name="'TParameter">Handler parameter type</typeparam>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <param name="canExecute">Responder for be able to execute</param>
    /// <returns>A Command instance</returns>
    [<Obsolete("CommandFactory class is obsoleted. Use Command.Factory instead.")>]
    static member create (executeAsync: 'TParameter -> Async<unit>, canExecute) =
        createP1 (executeAsync >> asyncUnitAsValueTaskUnit |> asFunc1) (canExecute |> asFunc1)

/// <summary>
/// Command factory functions for Async&lt;unit&gt; based asynchronous handler.
/// </summary>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public CommandFactoryExtension =

    type public CommandFactoryInstance with
        /// <summary>
        /// Generate a Command instance with Async&lt;unit&gt; based asynchronous handler.
        /// </summary>
        /// <param name="executeAsync">Asynchronous handler</param>
        /// <returns>A Command instance</returns>
        member __.create executeAsync =
            create0 (executeAsync >> asyncUnitAsValueTaskUnit |> asFunc0)

        /// <summary>
        /// Generate a Command instance with Async&lt;unit&gt; based asynchronous handler.
        /// </summary>
        /// <param name="executeAsync">Asynchronous handler</param>
        /// <param name="canExecute">Responder for be able to execute</param>
        /// <returns>A Command instance</returns>
        member __.create (executeAsync, canExecute) =
            create1 (executeAsync >> asyncUnitAsValueTaskUnit |> asFunc0) (canExecute |> asFunc0)

        /// <summary>
        /// Generate a Command instance with Async&lt;unit&gt; based asynchronous handler.
        /// </summary>
        /// <typeparam name="'TParameter">Handler parameter type</typeparam>
        /// <param name="executeAsync">Asynchronous handler</param>
        /// <returns>A Command instance</returns>
        member __.create (executeAsync: 'TParameter -> Async<unit>) =
            createP0 (executeAsync >> asyncUnitAsValueTaskUnit |> asFunc1)

        /// <summary>
        /// Generate a Command instance with Async&lt;unit&gt; based asynchronous handler.
        /// </summary>
        /// <typeparam name="'TParameter">Handler parameter type</typeparam>
        /// <param name="executeAsync">Asynchronous handler</param>
        /// <param name="canExecute">Responder for be able to execute</param>
        /// <returns>A Command instance</returns>
        member __.create (executeAsync: 'TParameter -> Async<unit>, canExecute) =
            createP1 (executeAsync >> asyncUnitAsValueTaskUnit |> asFunc1) (canExecute |> asFunc1)
