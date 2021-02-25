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

using Epoxy.Supplemental;

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Epoxy
{
    public abstract class ViewModel :
        ViewModelBase
    {
        protected ViewModel()
        { }

        protected ValueHolder GetValue(
            object? defaultValue = default,
            [CallerMemberName] string? propertyName = null) =>
            new ValueHolder(base.InternalGetValue(defaultValue, propertyName));

        protected TValue GetValue<TValue>(
            TValue defaultValue = default,
            [CallerMemberName] string? propertyName = null) =>
            base.InternalGetValue<TValue>(defaultValue!, propertyName);

        public ValueTask SetValueAsync<TValue>(
            TValue newValue,
            Func<TValue, ValueTask> propertyChanged,
            [CallerMemberName] string? propertyName = null) =>
            base.InternalSetValueAsync<TValue>(newValue, propertyChanged, propertyName);

        protected void SetValue<TValue>(
            TValue newValue,
            [CallerMemberName] string? propertyName = null) =>
            base.InternalSetValue<TValue>(newValue, propertyName);

        protected void OnPropertyChanging(
            [CallerMemberName] string? propertyName = null) =>
            base.InternalOnPropertyChanging(propertyName);

        protected void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null) =>
            base.InternalOnPropertyChanged(propertyName);
    }
}
