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
using System;
using System.Threading.Tasks;

namespace Epoxy.Advanced;

/// <summary>
/// Marks target inferface type for using GlobalService.
/// </summary>\
/// <remarks>The GlobalService will find target interface types when applied this attribute.</remarks>
[AttributeUsage(AttributeTargets.Interface)]
public sealed class GlobalServiceAttribute : Attribute
{
    /// <summary>
    /// The constructor.
    /// </summary>
    public GlobalServiceAttribute()
    { }
}

/// <summary>
/// GlobalService registering validation methods.
/// </summary>
public enum RegisteringValidations
{
    /// <summary>
    /// Strict method is totally declined when one or more interface type already registered.
    /// </summary>
    Strict,

    /// <summary>
    /// Unsafe partial method is ignored when interface type already registered.
    /// </summary>
    UnsafePartial,

    /// <summary>
    /// Unsafe override method is force overrided when interface type already registered.
    /// </summary>
    UnsafeOverride
}

/// <summary>
/// GlobalService accessor class.
/// </summary>
public sealed class GlobalServiceAccessor
{
    /// <summary>
    /// The constructor.
    /// </summary>
    internal GlobalServiceAccessor()
    {
    }
}

/// <summary>
/// GlobalService is a simple and lightweight dependency injection infrastructure.
/// </summary>
public static class GlobalService
{
    /// <summary>
    /// Get GlobalService accessor instance.
    /// </summary>
    public static readonly GlobalServiceAccessor Accessor =
        InternalGlobalService.Accessor;

    #region Obsoleted
    /// <summary>
    /// Static Register() method is obsoleted, will remove in future release. Use Accessor.Register() instead.
    /// </summary>
    [Obsolete("Static Register() method is obsoleted, will remove in future release. Use Accessor.Register() instead.")]
    public static void Register(
        object instance, RegisteringValidations validation = RegisteringValidations.Strict) =>
        InternalGlobalService.Register(instance, validation);

    /// <summary>
    /// Static Unregister() method is obsoleted, will remove in future release. Use Accessor.Unregister() instead.
    /// </summary>
    [Obsolete("Static Unregister() method is obsoleted, will remove in future release. Use Accessor.Unregister() instead.")]
    public static void Unregister(object instance) =>
        InternalGlobalService.Unregister(instance);

    /// <summary>
    /// Static ExecuteAsync() method is obsoleted, will remove future release. Use Accessor.ExecuteAsync() instead.
    /// </summary>
    [Obsolete("Static ExecuteAsync() method is obsoleted, will remove future release. Use Accessor.ExecuteAsync() instead.")]
    public static ValueTask ExecuteAsync<TService>(
        Func<TService, ValueTask> action, bool ignoreNotPresent = false) =>
        InternalGlobalService.ExecuteAsync<TService>(action, ignoreNotPresent);
    [Obsolete("Static ExecuteAsync() method is obsoleted, will remove future release. Use Accessor.ExecuteAsync() instead.")]
    public static ValueTask<TResult> ExecuteAsync<TService, TResult>(
        Func<TService, ValueTask<TResult>> action) =>
        InternalGlobalService.ExecuteAsync<TService, TResult>(action);
    #endregion
}
