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
using System.Threading.Tasks;
using Epoxy.Advanced;
using Epoxy.Internal;

namespace Epoxy.Synchronized;

/// <summary>
/// GlobalService is a simple and lightweight dependency injection infrastructure.
/// </summary>
/// <remarks>Notice: They handle with synchronous handler.
/// You can use asynchronous version instead.</remarks>
public static class SyncGlobalServiceAccessorExtension
{
    /// <summary>
    /// Execute target interface type synchronously.
    /// </summary>
    /// <typeparam name="TService">Target interface type</typeparam>
    /// <param name="accessor">Accessor instance (will use only fixup by compiler)</param>
    /// <param name="action">Synchronous continuation delegate</param>
    /// <param name="ignoreNotPresent">Ignore if didn't presend target instance.</param>
    /// <remarks>Notice: They handle with synchronous handler.
    /// You can use asynchronous version instead.</remarks>
    public static void ExecuteSync<TService>(
        this GlobalServiceAccessor accessor,
        Action<TService> action,
        bool ignoreNotPresent = false) =>
        InternalGlobalService.ExecuteSync(action, ignoreNotPresent);

    /// <summary>
    /// Execute target interface type synchronously.
    /// </summary>
    /// <typeparam name="TService">Target interface type</typeparam>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="accessor">Accessor instance (will use only fixup by compiler)</param>
    /// <param name="action">Synchronous continuation delegate</param>
    /// <returns>Result value</returns>
    /// <remarks>Notice: They handle with synchronous handler.
    /// You can use asynchronous version instead.</remarks>
    public static TResult ExecuteSync<TService, TResult>(
        this GlobalServiceAccessor accessor,
        Func<TService, TResult> action) =>
        InternalGlobalService.ExecuteSync(action);

    #region Dodge mistake choicing asynchronously overloads
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use ExecuteAsync instead.", true)]
    public static void ExecuteSync<TService>(
        this GlobalServiceAccessor accessor,
        Func<TService, ValueTask> action,
        bool ignoreNotPresent = false) =>
        throw new InvalidOperationException("Use ExecuteAsync instead.");

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use ExecuteAsync instead.", true)]
    public static TResult ExecuteSync<TService, TResult>(
        this GlobalServiceAccessor accessor,
        Func<TService, ValueTask<TResult>> action) =>
        throw new InvalidOperationException("Use ExecuteAsync instead.");
    #endregion
}
