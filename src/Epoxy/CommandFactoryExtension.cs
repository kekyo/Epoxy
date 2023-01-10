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

#nullable enable

using Epoxy.Internal;
using Epoxy.Supplemental;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Epoxy;

/// <summary>
/// CommandFactory class is obsoleted. Use Command.Factory instead.
/// </summary>
[Obsolete("CommandFactory class is obsoleted. Use Command.Factory instead.")]
[DebuggerStepThrough]
public static class CommandFactory
{
    /// <summary>
    /// CommandFactory class is obsoleted. Use Command.Factory instead.
    /// </summary>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <returns>A Command instance</returns>
    [Obsolete("CommandFactory class is obsoleted. Use Command.Factory instead.")]
    public static Command Create(
        Func<ValueTask> executeAsync) =>
        new DelegatedCommand(() => executeAsync().AsValueTaskUnit());

    /// <summary>
    /// CommandFactory class is obsoleted. Use Command.Factory instead.
    /// </summary>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <param name="canExecute">Responder for be able to execute</param>
    /// <returns>A Command instance</returns>
    [Obsolete("CommandFactory class is obsoleted. Use Command.Factory instead.")]
    public static Command Create(
        Func<ValueTask> executeAsync,
        Func<bool> canExecute) =>
        new DelegatedCommand(() => executeAsync().AsValueTaskUnit(), canExecute);

    /// <summary>
    /// CommandFactory class is obsoleted. Use Command.Factory instead.
    /// </summary>
    /// <typeparam name="TParameter">Handler parameter type</typeparam>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <returns>A Command instance</returns>
    [Obsolete("CommandFactory class is obsoleted. Use Command.Factory instead.")]
    public static Command Create<TParameter>(
        Func<TParameter, ValueTask> executeAsync) =>
        new DelegatedCommand<TParameter>(p => executeAsync(p).AsValueTaskUnit());

    /// <summary>
    /// CommandFactory class is obsoleted. Use Command.Factory instead.
    /// </summary>
    /// <typeparam name="TParameter">Handler parameter type</typeparam>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <param name="canExecute">Responder for be able to execute</param>
    /// <returns>A Command instance</returns>
    [Obsolete("CommandFactory class is obsoleted. Use Command.Factory instead.")]
    public static Command Create<TParameter>(
        Func<TParameter, ValueTask> executeAsync,
        Func<TParameter, bool> canExecute) =>
        new DelegatedCommand<TParameter>(p => executeAsync(p).AsValueTaskUnit(), canExecute);
}

/// <summary>
/// Command factory methods for ValueTask based asynchronous handler.
/// </summary>
[DebuggerStepThrough]
public static class CommandFactoryExtension
{
    /// <summary>
    /// Generate a Command instance with ValueTask based asynchronous handler.
    /// </summary>
    /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <returns>A Command instance</returns>
    public static Command Create(
        this CommandFactoryInstance factory,
        Func<ValueTask> executeAsync) =>
        new DelegatedCommand(() => executeAsync().AsValueTaskUnit());

    /// <summary>
    /// Generate a Command instance with ValueTask based asynchronous handler.
    /// </summary>
    /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <param name="canExecute">Responder for be able to execute</param>
    /// <returns>A Command instance</returns>
    public static Command Create(
        this CommandFactoryInstance factory,
        Func<ValueTask> executeAsync,
        Func<bool> canExecute) =>
        new DelegatedCommand(() => executeAsync().AsValueTaskUnit(), canExecute);

    /// <summary>
    /// Generate a Command instance with ValueTask based asynchronous handler.
    /// </summary>
    /// <typeparam name="TParameter">Handler parameter type</typeparam>
    /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <returns>A Command instance</returns>
    public static Command Create<TParameter>(
        this CommandFactoryInstance factory,
        Func<TParameter, ValueTask> executeAsync) =>
        new DelegatedCommand<TParameter>(p => executeAsync(p).AsValueTaskUnit());

    /// <summary>
    /// Generate a Command instance with ValueTask based asynchronous handler.
    /// </summary>
    /// <typeparam name="TParameter">Handler parameter type</typeparam>
    /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
    /// <param name="executeAsync">Asynchronous handler</param>
    /// <param name="canExecute">Responder for be able to execute</param>
    /// <returns>A Command instance</returns>
    public static Command Create<TParameter>(
        this CommandFactoryInstance factory,
        Func<TParameter, ValueTask> executeAsync,
        Func<TParameter, bool> canExecute) =>
        new DelegatedCommand<TParameter>(p => executeAsync(p).AsValueTaskUnit(), canExecute);
}
