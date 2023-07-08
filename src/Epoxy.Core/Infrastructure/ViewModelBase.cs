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
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Epoxy.Infrastructure;

/// <summary>
/// The ViewModel base class using internal only.
/// </summary>
/// <remarks>You have to use Epoxy.ViewModel class instead this class directly.</remarks>
[DebuggerDisplay("{PrettyPrint}")]
public abstract class ViewModelBase :
    IViewModelImplementer
{
    internal InternalPropertyBag? epoxy_properties__;

    /// <summary>
    /// The constructor.
    /// </summary>
    [DebuggerStepThrough]
    private protected ViewModelBase()
    { }

    /// <summary>
    /// Occurs when a property value is changing.
    /// </summary>
    public event PropertyChangingEventHandler? PropertyChanging
    {
        [DebuggerStepThrough]
        add => InternalModelHelper.AddPropertyChanging(
            value, ref this.epoxy_properties__);
        [DebuggerStepThrough]
        remove => InternalModelHelper.RemovePropertyChanging(
            value, ref this.epoxy_properties__);
    }

    /// <summary>
    /// Occurs when a property value is changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged
    {
        [DebuggerStepThrough]
        add => InternalModelHelper.AddPropertyChanged(
            value, ref this.epoxy_properties__);
        [DebuggerStepThrough]
        remove => InternalModelHelper.RemovePropertyChanged(
            value, ref this.epoxy_properties__);
    }

    [DebuggerStepThrough]
    private protected object? InternalGetValue(
        object? defaultValue,
        string? propertyName) =>
        InternalModelHelper.GetValue(defaultValue, propertyName, ref epoxy_properties__);

    [DebuggerStepThrough]
    private protected TValue InternalGetValue<TValue>(
        TValue defaultValue,
        string? propertyName) =>
        InternalModelHelper.GetValueT(defaultValue, propertyName, ref epoxy_properties__);

    [DebuggerStepThrough]
    internal ValueTask<Unit> InternalSetValueAsync<TValue>(
        TValue newValue,
        Func<TValue, ValueTask<Unit>> propertyChanged,
        string? propertyName) =>
        InternalModelHelper.SetValueAsyncT(
            newValue, propertyChanged, propertyName,
            this, ref epoxy_properties__);

    [DebuggerStepThrough]
    internal void InternalSetValue<TValue>(
        TValue newValue,
        string? propertyName) =>
        _ = InternalModelHelper.SetValueAsyncT(
            newValue, null, propertyName,
            this, ref epoxy_properties__);

    [DebuggerStepThrough]
    private protected void InternalOnPropertyChanging(
        string? propertyName) =>
        InternalModelHelper.OnPropertyChanging(
            propertyName, this, this.epoxy_properties__);

    [DebuggerStepThrough]
    private protected void InternalOnPropertyChanged(
        string? propertyName) =>
        InternalModelHelper.OnPropertyChanged(
            propertyName, this, this.epoxy_properties__);

    /// <summary>
    /// Pretty printer for this class.
    /// </summary>
    public string PrettyPrint
    {
        [DebuggerStepThrough]
        get => InternalModelHelper.PrettyPrint(this, false);
    }

    /// <summary>
    /// Get pretty printing string.
    /// </summary>
    /// <returns>Pretty printing string</returns>
    [DebuggerStepThrough]
    public override string ToString() =>
        $"{this.GetPrettyTypeName()}: {this.PrettyPrint}";
}
