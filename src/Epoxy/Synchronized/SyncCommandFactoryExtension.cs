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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Epoxy.Synchronized;

/// <summary>
/// Command factory methods for synchronous handler.
/// </summary>
/// <remarks>Notice: They handle with synchronous handler.
/// You can use asynchronous version instead.</remarks>
[DebuggerStepThrough]
public static class SyncCommandFactoryExtension
{
    /// <summary>
    /// Generate a Command instance with synchronous handler.
    /// </summary>
    /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
    /// <param name="execute">Synchronous handler</param>
    /// <returns>A Command instance</returns>
    /// <remarks>Notice: It handles with synchronous handler.
    /// You can use asynchronous version instead.</remarks>
    public static Command CreateSync(
        this CommandFactoryInstance factory,
        Action execute) =>
        new SyncDelegatedCommand(execute);

    /// <summary>
    /// Generate a Command instance with synchronous handler.
    /// </summary>
    /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
    /// <param name="execute">Synchronous handler</param>
    /// <param name="canExecute">Responder for be able to execute</param>
    /// <returns>A Command instance</returns>
    /// <remarks>Notice: It handles with synchronous handler.
    /// You can use asynchronous version instead.</remarks>
    public static Command CreateSync(
        this CommandFactoryInstance factory,
        Action execute,
        Func<bool> canExecute) =>
        new SyncDelegatedCommand(execute, canExecute);

    /// <summary>
    /// Generate a Command instance with synchronous handler.
    /// </summary>
    /// <typeparam name="TParameter">Handler parameter type</typeparam>
    /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
    /// <param name="execute">Synchronous handler</param>
    /// <returns>A Command instance</returns>
    /// <remarks>Notice: It handles with synchronous handler.
    /// You can use asynchronous version instead.</remarks>
    public static Command CreateSync<TParameter>(
        this CommandFactoryInstance factory,
        Action<TParameter> execute) =>
        new SyncDelegatedCommand<TParameter>(execute);

    /// <summary>
    /// Generate a Command instance with synchronous handler.
    /// </summary>
    /// <typeparam name="TParameter">Handler parameter type</typeparam>
    /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
    /// <param name="execute">Synchronous handler</param>
    /// <param name="canExecute">Responder for be able to execute</param>
    /// <returns>A Command instance</returns>
    /// <remarks>Notice: It handles with synchronous handler.
    /// You can use asynchronous version instead.</remarks>
    public static Command CreateSync<TParameter>(
        this CommandFactoryInstance factory,
        Action<TParameter> execute,
        Func<TParameter, bool> canExecute) =>
        new SyncDelegatedCommand<TParameter>(execute, canExecute);

    #region Dodge mistake choicing asynchronously overloads
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use Create instead.", true)]
    public static Command CreateSync(
        this CommandFactoryInstance factory,
        Func<ValueTask> execute) =>
        throw new InvalidOperationException("Use Create instead.");

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use Create instead.", true)]
    public static Command CreateSync(
        this CommandFactoryInstance factory,
        Func<ValueTask> execute,
        Func<bool> canExecute) =>
        throw new InvalidOperationException("Use Create instead.");

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use Create instead.", true)]
    public static Command CreateSync<TParameter>(
        this CommandFactoryInstance factory,
        Func<TParameter, ValueTask> execute) =>
        throw new InvalidOperationException("Use Create instead.");

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use Create instead.", true)]
    public static Command CreateSync<TParameter>(
        this CommandFactoryInstance factory,
        Func<TParameter, ValueTask> execute,
        Func<TParameter, bool> canExecute) =>
        throw new InvalidOperationException("Use Create instead.");
    #endregion
}
