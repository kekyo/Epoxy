﻿////////////////////////////////////////////////////////////////////////////
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

#nullable enable

using System;
using System.Threading.Tasks;

using Epoxy.Internal;

namespace Epoxy.Advanced
{
    /// <summary>
    /// GlobalService is a simple and lightweight dependency injection infrastructure.
    /// </summary>
    /// <example>
    /// <code>
    /// // Marks target interface type with 'GlobalService' attribute.
    /// [GlobalService]
    /// public interface IBluetooth
    /// {
    ///     // Declares manipulation method.
    ///     ValueTask EnableAsync(string parameter);
    /// }
    /// 
    /// // Platform depended implementation class.
    /// public sealed class AndroidBluetoothFacade : IBluetooth
    /// {
    ///     // Manipulation method.
    ///     public async ValueTask EnableAsync(string parameter)
    ///     {
    ///         // Your own platform depended implementations...
    ///     }
    /// }
    /// 
    /// // Register an instance.
    /// var facade = new AndroidBluetoothFacade();
    /// GlobalService.Register(facade);
    /// 
    /// // To use it on asynchronously.
    /// await GlobalService.ExecuteAsync&lt;IBluetooth&gt;(async bluetooth =>
    /// {
    ///     // 'bluetooth' argument is registered instance.
    ///     await bluetooth.EnableAsync("Primary");
    /// });
    /// </code>
    /// </example>
    public static class GlobalService
    {
        /// <summary>
        /// Get GlobalService accessor instance.
        /// </summary>
        public static readonly GlobalServiceAccessor Accessor =
            new GlobalServiceAccessor();

        /// <summary>
        /// Register an instance into GlobalService.
        /// </summary>
        /// <param name="instance">Target instance</param>
        /// <param name="validation">Registering validation method</param>
        /// <example>
        /// <code>
        /// // Marks the interface is target.
        /// [GlobalService]
        /// public interface IBluetooth
        /// {
        ///     // Declares manipulation method.
        ///     ValueTask EnableAsync(string parameter);
        /// }
        /// 
        /// // Platform depended implementation class.
        /// public sealed class AndroidBluetoothFacade
        /// {
        ///     // Manipulation method.
        ///     public async ValueTask EnableAsync(string parameter)
        ///     {
        ///         // Your own platform depended implementations...
        ///     }
        /// }
        /// 
        /// // Register instance.
        /// var facade = new AndroidBluetoothFacade();
        /// GlobalService.Register(facade);
        /// </code>
        /// </example>
        public static void Register(
            object instance, RegisteringValidations validation = RegisteringValidations.Strict) =>
            InternalGlobalService.Register(instance, validation);

        /// <summary>
        /// Unregister instance from GlobalService.
        /// </summary>
        /// <param name="instance">Target instance</param>
        public static void Unregister(object instance) =>
            InternalGlobalService.Unregister(instance);

        /// <summary>
        /// Execute target interface type asynchronously.
        /// </summary>
        /// <typeparam name="TService">Target interface type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="ignoreNotPresent">Ignore if didn't presend target instance.</param>
        /// <returns>ValueTask</returns>
        /// <example>
        /// <code>
        /// // Use the interface.
        /// await GlobalService.ExecuteAsync&lt;IBluetooth&gt;(async bluetooth =>
        /// {
        ///     // 'bluetooth' argument is registered instance.
        ///     await bluetooth.EnableAsync("Primary");
        /// });
        /// </code>
        /// </example>
        public static ValueTask ExecuteAsync<TService>(
            Func<TService, ValueTask> action, bool ignoreNotPresent = false) =>
            InternalGlobalService.ExecuteAsync<TService>(action, ignoreNotPresent);

        /// <summary>
        /// Execute target interface type asynchronously.
        /// </summary>
        /// <typeparam name="TService">Target interface type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <returns>ValueTask</returns>
        /// <example>
        /// <code>
        /// // Use the interface.
        /// var result = await GlobalService.ExecuteAsync&lt;IBluetooth, int&gt;(async bluetooth =>
        /// {
        ///     // 'bluetooth' argument is registered instance.
        ///     await bluetooth.EnableAsync("Primary");
        ///     return 100;
        /// });
        /// </code>
        /// </example>
        public static ValueTask<TResult> ExecuteAsync<TService, TResult>(
            Func<TService, ValueTask<TResult>> action) =>
            InternalGlobalService.ExecuteAsync<TService, TResult>(action);

        /// <summary>
        /// Execute target interface type asynchronously.
        /// </summary>
        /// <typeparam name="TService">Target interface type</typeparam>
        /// <param name="accessor">Accessor instance (will use only fixup by compiler)</param>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="ignoreNotPresent">Ignore if didn't presend target instance.</param>
        /// <returns>ValueTask</returns>
        /// <example>
        /// <code>
        /// // Use the interface.
        /// await GlobalService.Accessor.ExecuteAsync&lt;IBluetooth&gt;(async bluetooth =>
        /// {
        ///     // 'bluetooth' argument is registered instance.
        ///     await bluetooth.EnableAsync("Primary");
        /// });
        /// </code>
        /// </example>
        public static ValueTask ExecuteAsync<TService>(
            this GlobalServiceAccessor accessor,
            Func<TService, ValueTask> action,
            bool ignoreNotPresent = false) =>
            InternalGlobalService.ExecuteAsync(action, ignoreNotPresent);

        /// <summary>
        /// Execute target interface type asynchronously.
        /// </summary>
        /// <typeparam name="TService">Target interface type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="accessor">Accessor instance (will use only fixup by compiler)</param>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <returns>ValueTask</returns>
        /// <example>
        /// <code>
        /// // Use the interface.
        /// var result = await GlobalService.Accessor.ExecuteAsync&lt;IBluetooth, int&gt;(async bluetooth =>
        /// {
        ///     // 'bluetooth' argument is registered instance.
        ///     await bluetooth.EnableAsync("Primary");
        ///     return 100;
        /// });
        /// </code>
        /// </example>
        public static ValueTask<TResult> ExecuteAsync<TService, TResult>(
            this GlobalServiceAccessor accessor,
            Func<TService, ValueTask<TResult>> action) =>
            InternalGlobalService.ExecuteAsync(action);
    }
}
