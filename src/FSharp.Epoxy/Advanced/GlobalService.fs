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

namespace Epoxy.Advanced

open System.Diagnostics
open System.Runtime.InteropServices

open Epoxy.Advanced
open Epoxy.Internal

/// <summary>
/// GlobalService is a simple and lightweight dependency injection infrastructure.
/// </summary>
/// <example>
/// <code>
/// // Marks target interface type with 'GlobalService' attribute.
/// [&lt;GlobalService&gt;]
/// type IBluetooth =
///     interface
///     // Declares manipulation function.
///     abstract enableAsync: string -> Async<unit>
/// 
/// // Platform depended implementation class.
/// type AndroidBluetoothFacade = 
///     interface IBluetooth with
///         // Manipulation function.
///         member __.enableAsync parameter = async {
///             // Your own platform depended implementations...
///         }
/// 
/// // Register an instance.
/// let facade = new AndroidBluetoothFacade()
/// GlobalService.register facade
/// 
/// // To use it on asynchronously.
/// do! GlobalService.executeAsync (fun (bluetooth: IBluetooth) -> async {
///     // 'bluetooth' argument is registered instance.
///     do! bluetooth.enableAsync "Primary"
/// })
/// </code>
/// </example>
[<DebuggerStepThrough>]
[<AbstractClass>]
[<Sealed>]
type public GlobalService =
    /// <summary>
    /// Get GlobalService accessor instance.
    /// </summary>
    static member GlobalServiceAccessor accessor =
        new GlobalServiceAccessor()

    /// <summary>
    /// Register an instance into GlobalService.
    /// </summary>
    /// <param name="instance">Target instance</param>
    /// <param name="validation">Registering validation method</param>
    /// <example>
    /// <code>
    /// // Marks target interface type with 'GlobalService' attribute.
    /// [&lt;GlobalService&gt;]
    /// type IBluetooth =
    ///     interface
    ///     // Declares manipulation function.
    ///     abstract enableAsync: string -> Async<unit>
    /// 
    /// // Platform depended implementation class.
    /// type AndroidBluetoothFacade = 
    ///     interface IBluetooth with
    ///         // Manipulation function.
    ///         member __.enableAsync parameter = async {
    ///             // Your own platform depended implementations...
    ///         }
    /// 
    /// // Register an instance.
    /// let facade = new AndroidBluetoothFacade()
    /// GlobalService.register facade
    /// </code>
    /// </example>
    static member register(instance: obj, [<Optional; DefaultParameterValue(RegisteringValidations.Strict)>] validation: RegisteringValidations) =
        InternalGlobalService.Register(instance, validation)

    /// <summary>
    /// Unregister instance from GlobalService.
    /// </summary>
    /// <param name="instance">Target instance</param>
    static member unregister(instance: obj) =
        InternalGlobalService.Unregister(instance)

    /// <summary>
    /// Execute target interface type asynchronously.
    /// </summary>
    /// <typeparam name="'TService">Target interface type</typeparam>
    /// <param name="action">Asynchronous continuation delegate</param>
    /// <param name="ignoreNotPresent">Ignore if didn't presend target instance.</param>
    /// <returns>Async&lt;unit&gt; instance</returns>
    /// <example>
    /// <code>
    /// // Use the interface.
    /// do! GlobalService.executeAsync (fun (bluetooth: IBluetooth) -> async {
    ///     // 'bluetooth' argument is registered instance.
    ///     do! bluetooth.enableAsync "Primary"
    /// })
    /// </code>
    /// </example>
    static member executeAsync(action: 'TService -> Async<unit>, [<Optional; DefaultParameterValue(false)>] ignoreNotPresent: bool) =
        InternalGlobalService.ExecuteAsync<'TService>(action >> asyncUnitAsValueTaskVoid |> asFunc1, ignoreNotPresent)
        |> valueTaskVoidAsAsyncResult

    /// <summary>
    /// Execute target interface type asynchronously.
    /// </summary>
    /// <typeparam name="'TService">Target interface type</typeparam>
    /// <typeparam name="'TResult">Result type</typeparam>
    /// <param name="action">Asynchronous continuation delegate</param>
    /// <returns>Async&lt;'TResult&gt; instance</returns>
    /// <example>
    /// <code>
    /// // Use the interface.
    /// let! result = GlobalService.executeAsync (fun (bluetooth: IBluetooth) -> async {
    ///     // 'bluetooth' argument is registered instance.
    ///     do! bluetooth.enableAsync "Primary"
    ///     return 100
    /// })
    /// </code>
    /// </example>
    static member executeAsync(action: 'TService -> Async<'TResult>) =
        InternalGlobalService.ExecuteAsync<'TService, 'TResult>(action >> asyncAsValueTask |> asFunc1)
        |> valueTaskAsAsyncResult

/// <summary>
/// GlobalService is a simple and lightweight dependency injection infrastructure.
/// </summary>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public GlobalServiceAccessorExtension =

    type GlobalServiceAccessor with
        /// <summary>
        /// Execute target interface type asynchronously.
        /// </summary>
        /// <typeparam name="'TService">Target interface type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="ignoreNotPresent">Ignore if didn't presend target instance.</param>
        /// <returns>Async&lt;unit&gt; instance</returns>
        /// <example>
        /// <code>
        /// // Use the interface.
        /// do! GlobalService.executeAsync (fun (bluetooth: IBluetooth) -> async {
        ///     // 'bluetooth' argument is registered instance.
        ///     do! bluetooth.enableAsync "Primary"
        /// })
        /// </code>
        /// </example>
        member __.executeAsync(action: 'TService -> Async<unit>, [<Optional; DefaultParameterValue(false)>] ignoreNotPresent: bool) =
            InternalGlobalService.ExecuteAsync<'TService>(action >> asyncUnitAsValueTaskVoid |> asFunc1, ignoreNotPresent)
            |> valueTaskVoidAsAsyncResult

        /// <summary>
        /// Execute target interface type asynchronously.
        /// </summary>
        /// <typeparam name="'TService">Target interface type</typeparam>
        /// <typeparam name="'TResult">Result type</typeparam>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <returns>Async&lt;'TResult&gt; instance</returns>
        /// <example>
        /// <code>
        /// // Use the interface.
        /// let! result = GlobalService.executeAsync (fun (bluetooth: IBluetooth) -> async {
        ///     // 'bluetooth' argument is registered instance.
        ///     do! bluetooth.enableAsync "Primary"
        ///     return 100
        /// })
        /// </code>
        /// </example>
        member __.executeAsync(action: 'TService -> Async<'TResult>) =
            InternalGlobalService.ExecuteAsync<'TService, 'TResult>(action >> asyncAsValueTask |> asFunc1)
            |> valueTaskAsAsyncResult
