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

using Epoxy.Infrastructure;
using Epoxy.Internal;
using Epoxy.Supplemental;

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Epoxy
{
    /// <summary>
    /// The ViewModel base class implementation.
    /// </summary>
    /// <remarks>You can choose for using ViewModel implementation:
    /// * Applies ViewModel attribute (ViewModelAttribute) onto your pure ViewModel type and places auto-implemented properties.
    /// * Your ViewModel type derives from this base class and use GetValue/SetValue methods.
    /// </remarks>
    public abstract class ViewModel :
        ViewModelBase
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        protected ViewModel()
        { }

        /// <summary>
        /// Get a value from ViewModel with helper.
        /// </summary>
        /// <param name="defaultValue">Default value if not present</param>
        /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
        /// <returns>Helper instance (ValueHolder)</returns>
        /// <remarks>Mostly useful for standard usage, this method will return with ValueHolder struct.
        /// It will automatically convert some standard types (CLR and XAML related).
        /// You can use other GetValue overload with generic arguments if have to use non-standard types.</remarks>
        protected ValueHolder GetValue(
            object? defaultValue = default,
            [CallerMemberName] string? propertyName = null) =>
            new ValueHolder(base.InternalGetValue(defaultValue, propertyName));

        /// <summary>
        /// Get a value from ViewModel with an explicit type.
        /// </summary>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="defaultValue">Default value if not present</param>
        /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
        /// <returns>Value</returns>
        protected TValue GetValue<TValue>(
            TValue defaultValue = default!,
            [CallerMemberName] string? propertyName = null) =>
            base.InternalGetValue<TValue>(defaultValue!, propertyName);

        /// <summary>
        /// Set a value to ViewModel with callback.
        /// </summary>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="newValue">A value</param>
        /// <param name="propertyChanged">Callback delegate when value has changed</param>
        /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
        /// <returns>ValueTask instance</returns>
        /// <remarks>This method will return with ValueTask, you may have to await this.</remarks>
        public ValueTask SetValueAsync<TValue>(
            TValue newValue,
            Func<TValue, ValueTask> propertyChanged,
            [CallerMemberName] string? propertyName = null) =>
            base.InternalSetValueAsync<TValue>(newValue, v => propertyChanged(v).AsValueTaskUnit(), propertyName).AsValueTaskVoid();

        /// <summary>
        /// Set a value to ViewModel.
        /// </summary>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="newValue">A value</param>
        /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
        protected void SetValue<TValue>(
            TValue newValue,
            [CallerMemberName] string? propertyName = null) =>
            base.InternalSetValue<TValue>(newValue, propertyName);

        /// <summary>
        /// Property changing callback.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanging(
            [CallerMemberName] string? propertyName = null) =>
            base.InternalOnPropertyChanging(propertyName);

        /// <summary>
        /// Property changed callback.
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null) =>
            base.InternalOnPropertyChanged(propertyName);
    }
}
