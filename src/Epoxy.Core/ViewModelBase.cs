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
using System.Linq;
using System.Threading.Tasks;

using Epoxy.Internal;
using Epoxy.Supplemental;

namespace Epoxy
{
    public abstract class ViewModelBase :
        ModelBase, INotifyPropertyChanging, INotifyPropertyChanged
    {
        private Dictionary<string, object?>? properties;

        [DebuggerStepThrough]
        private protected ViewModelBase()
        { }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;

        [DebuggerStepThrough]
        private Dictionary<string, object?> Prepare()
        {
            if (!(properties is Dictionary<string, object>))
            {
                properties = new Dictionary<string, object?>();
            }
            return properties;
        }

        private protected object? InternalGetValue(
            object? defaultValue,
            string? propertyName)
        {
            Debug.Assert(propertyName is string);
            if (!UIThread.UnsafeIsBound)
            {
                throw new InvalidOperationException(
                    "Couldn't use GetValue() on worker thread context.");
            }

            var properties = this.Prepare();
            if (properties.TryGetValue(propertyName!, out var value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        private protected TValue InternalGetValue<TValue>(
            TValue defaultValue,
            string? propertyName)
        {
            Debug.Assert(propertyName is string);
            if (!UIThread.UnsafeIsBound)
            {
                throw new InvalidOperationException(
                    "Couldn't use GetValue<TValue>() on worker thread context.");
            }

            var properties = this.Prepare();
            if (properties.TryGetValue(propertyName!, out var value) && value is TValue v)
            {
                return v;
            }
            else
            {
                return defaultValue!;
            }
        }

        private ValueTask InternalPrivateSetValueAsync<TValue>(
            TValue newValue,
            Func<TValue, ValueTask>? propertyChanged,
            string? propertyName = null)
        {
            Debug.Assert(propertyName is string);
            if (!UIThread.UnsafeIsBound)
            {
                throw new InvalidOperationException(
                    "Couldn't use SetValue() on worker thread context.");
            }

            var properties = this.Prepare();
            if (properties.TryGetValue(propertyName!, out var oldValue))
            {
                if (!DefaultValue<TValue>.ValueEquals(oldValue, newValue))
                {
                    this.InternalOnPropertyChanging(propertyName);

                    if (!DefaultValue.IsDefaultValue(newValue))
                    {
                        properties[propertyName!] = newValue!;
                    }
                    else
                    {
                        properties.Remove(propertyName!);
                    }

                    this.InternalOnPropertyChanged(propertyName);
                    if (propertyChanged is Func<TValue, ValueTask> pc)
                    {
                        return pc.Invoke(newValue);
                    }
                }
            }
            else
            {
                this.InternalOnPropertyChanging(propertyName);

                if (!DefaultValue.IsDefaultValue(newValue))
                {
                    properties.Add(propertyName!, newValue!);
                }
                else
                {
                    properties.Remove(propertyName!);
                }

                this.InternalOnPropertyChanged(propertyName);
                if (propertyChanged is Func<TValue, ValueTask> pc)
                {
                    return pc.Invoke(newValue);
                }
            }

            return default;
        }

        [DebuggerStepThrough]
        internal ValueTask InternalSetValueAsync<TValue>(
            TValue newValue,
            Func<TValue, ValueTask> propertyChanged,
            string? propertyName) =>
            this.InternalPrivateSetValueAsync(newValue, propertyChanged, propertyName);

        [DebuggerStepThrough]
        internal void InternalSetValue<TValue>(
            TValue newValue,
            string? propertyName) =>
            _ = this.InternalPrivateSetValueAsync(newValue, null, propertyName);

        private protected void InternalOnPropertyChanging(
            string? propertyName)
        {
            Debug.Assert(propertyName is string);
            if (!UIThread.UnsafeIsBound)
            {
                throw new InvalidOperationException(
                    "Couldn't use OnPropertyChanging() on worker thread context.");
            }

            this.PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        private protected void InternalOnPropertyChanged(
            string? propertyName)
        {
            Debug.Assert(propertyName is string);
            if (!UIThread.UnsafeIsBound)
            {
                throw new InvalidOperationException(
                    "Couldn't use OnPropertyChanged() on worker thread context.");
            }

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [DebuggerStepThrough]
        protected override string OnPrettyPrint() =>
            string.Join(
                ",",
                this.EnumerateProperties().
                OrderBy(entry => entry.Key).
                Select(entry => $"{entry.Key}={entry.Value ?? "(null)"}"));
    }
}
