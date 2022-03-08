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

namespace Epoxy.Supplemental

open System
open System.Diagnostics
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open System.Threading.Tasks

open Epoxy
open Epoxy.Internal

/// <summary>
/// The ViewModel functions.
/// </summary>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public ViewModelExtension =

    type public ViewModel with

        /// <summary>
        /// Set value directly.
        /// </summary>
        /// <typeparam name="'TValue">Value type</typeparam>
        /// <param name="newValue">Value</param>
        /// <param name="propertyChanged">Callback delegate when value has changed</param>
        /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
        /// <returns>Async&lt;unit&gt; instance</returns>
        member self.setValueAsync (newValue: 'TValue, propertyChanged: 'TValue -> ValueTask<unit>, [<Optional; CallerMemberName; DefaultParameterValue("")>] propertyName) =
            self.InternalSetValueAsync<'TValue>(newValue, propertyChanged >> valueTaskUnitAsValueTaskUnit |> asFunc1, propertyName)
            |> valueTaskUnitAsAsyncResult

        /// <summary>
        /// Set value directly.
        /// </summary>
        /// <typeparam name="'TValue">Value type</typeparam>
        /// <param name="newValue">Value</param>
        /// <param name="propertyChanged">Callback delegate when value has changed</param>
        /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
        /// <returns>Async&lt;unit&gt; instance</returns>
        member self.setValueAsync (newValue: 'TValue, propertyChanged: 'TValue -> Task<unit>, [<Optional; CallerMemberName; DefaultParameterValue("")>] propertyName) =
            self.InternalSetValueAsync<'TValue>(newValue, propertyChanged >> taskUnitAsValueTaskUnit |> asFunc1, propertyName)
            |> valueTaskUnitAsAsyncResult

        /// <summary>
        /// Set value directly.
        /// </summary>
        /// <typeparam name="'TValue">Value type</typeparam>
        /// <param name="newValue">Value</param>
        /// <param name="propertyChanged">Callback delegate when value has changed</param>
        /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
        /// <returns>Async&lt;unit&gt; instance</returns>
        member self.setValueAsync (newValue, propertyChanged: 'TValue -> ValueTask, [<Optional; CallerMemberName>] propertyName) =
            self.InternalSetValueAsync<'TValue>(newValue, propertyChanged >> valueTaskVoidAsValueTaskUnit |> asFunc1, propertyName)
            |> valueTaskUnitAsAsyncResult

        /// <summary>
        /// Set value directly.
        /// </summary>
        /// <typeparam name="'TValue">Value type</typeparam>
        /// <param name="newValue">Value</param>
        /// <param name="propertyChanged">Callback delegate when value has changed</param>
        /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
        /// <returns>Async&lt;unit&gt; instance</returns>
        member self.setValueAsync (newValue, propertyChanged: 'TValue -> Task, [<Optional; CallerMemberName>] propertyName) =
            self.InternalSetValueAsync<'TValue>(newValue, propertyChanged >> taskVoidAsValueTaskUnit |> asFunc1, propertyName)
            |> valueTaskUnitAsAsyncResult
