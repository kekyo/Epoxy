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

open Epoxy.Internal
open Epoxy.Infrastructure

open System.Runtime.CompilerServices
open System.Runtime.InteropServices

/// <summary>
/// The ViewModel base class implementation.
/// </summary>
/// <remarks>You can choose for using ViewModel implementation:
/// * Applies ViewModel attribute (ViewModelAttribute) onto your pure ViewModel type and places auto-implemented properties.
/// * Your ViewModel type derives from this base class and use getValue/setValue functions.
/// </remarks>
[<AbstractClass>]
type public ViewModel() =
    inherit ViewModelBase()

    /// <summary>
    /// Get a value from ViewModel with an explicit type.
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
    /// <returns>Value</returns>
    /// <remarks>Default value will come from Unchecked.defaultof operator if not present.</remarks>
    member self.getValue<'TValue> ([<Optional; CallerMemberName; DefaultParameterValue("")>] propertyName) =
        self.InternalGetValue<'TValue>(Unchecked.defaultof<_>, propertyName)
    /// <summary>
    /// Get a value from ViewModel.
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="defaultValue">Default value if not present</param>
    /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
    /// <returns>Value</returns>
    member self.getValue (defaultValue: 'TValue, [<Optional; CallerMemberName; DefaultParameterValue("")>] propertyName) =
        self.InternalGetValue<'TValue>(defaultValue, propertyName)

    /// <summary>
    /// Set a value to ViewModel with callback.
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="newValue">A value</param>
    /// <param name="propertyChanged">Callback delegate when value has changed</param>
    /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
    /// <returns>Async&lt;unit&gt; instance</returns>
    /// <remarks>This function will return with Async&lt;unit&gt;, you may have to bind this on async workflow.</remarks>
    member self.setValueAsync (newValue: 'TValue, propertyChanged: 'TValue -> Async<unit>, [<Optional; CallerMemberName; DefaultParameterValue("")>] propertyName) =
        self.InternalSetValueAsync<'TValue>(newValue, propertyChanged >> asyncUnitAsValueTaskUnit |> asFunc1, propertyName) |> valueTaskUnitAsAsyncResult

    /// <summary>
    /// Set a value to ViewModel.
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
    /// <param name="newValue">A value</param>
    /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
    member self.setValue (newValue: 'TValue, [<Optional; CallerMemberName; DefaultParameterValue("")>] propertyName) =
        self.InternalSetValue<'TValue>(newValue, propertyName)

    /// <summary>
    /// Property changing callback.
    /// </summary>
    /// <param name="propertyName">Property name</param>
    member self.onPropertyChanging ([<Optional; CallerMemberName; DefaultParameterValue("")>] propertyName) =
        self.InternalOnPropertyChanging(propertyName)
    /// <summary>
    /// Property changed callback.
    /// </summary>
    /// <param name="propertyName">Property name</param>
    member self.onPropertyChanged ([<Optional; CallerMemberName; DefaultParameterValue("")>] propertyName) =
        self.InternalOnPropertyChanged(propertyName)
