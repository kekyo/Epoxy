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

#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Epoxy.Internal;

namespace Epoxy.Infrastructure
{
    [DebuggerDisplay("{PrettyPrint}")]
    public abstract class ViewModelBase :
        INotifyPropertyChanging, INotifyPropertyChanged
    {
        private Dictionary<string, object?>? properties;

        [DebuggerStepThrough]
        private protected ViewModelBase()
        { }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;

        [DebuggerStepThrough]
        private protected object? InternalGetValue(
            object? defaultValue,
            string? propertyName) =>
            InternalModelHelper.GetValue(defaultValue, propertyName, ref properties);

        [DebuggerStepThrough]
        private protected TValue InternalGetValue<TValue>(
            TValue defaultValue,
            string? propertyName) =>
            InternalModelHelper.GetValue(defaultValue, propertyName, ref properties);

        [DebuggerStepThrough]
        internal ValueTask<Unit> InternalSetValueAsync<TValue>(
            TValue newValue,
            Func<TValue, ValueTask<Unit>> propertyChanged,
            string? propertyName) =>
            InternalModelHelper.SetValueAsync(
                newValue, propertyChanged, propertyName,
                this, this.PropertyChanging, this.PropertyChanged,
                ref properties);

        [DebuggerStepThrough]
        internal void InternalSetValue<TValue>(
            TValue newValue,
            string? propertyName) =>
            InternalModelHelper.SetValueAsync(
                newValue, null, propertyName,
                this, this.PropertyChanging, this.PropertyChanged,
                ref properties);

        [DebuggerStepThrough]
        private protected void InternalOnPropertyChanging(
            string? propertyName) =>
            InternalModelHelper.OnPropertyChanging(this, this.PropertyChanging, propertyName);

        [DebuggerStepThrough]
        private protected void InternalOnPropertyChanged(
            string? propertyName) =>
            InternalModelHelper.OnPropertyChanged(this, this.PropertyChanged, propertyName);

        public string PrettyPrint =>
            InternalModelHelper.PrettyPrint(this, false);

        [DebuggerStepThrough]
        public override string ToString() =>
            $"{this.GetPrettyTypeName()}: {this.PrettyPrint}";
    }
}
