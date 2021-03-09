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

namespace Epoxy.Internal
{
    //[DebuggerStepThrough]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InternalModelHelper
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static Dictionary<string, object?> Prepare(ref Dictionary<string, object?>? properties)
        {
            if (!(properties is Dictionary<string, object>))
            {
                properties = new Dictionary<string, object?>();
            }
            return properties;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static object? GetValue(
            object? defaultValue,
            string? propertyName,
            ref Dictionary<string, object?>? properties)
        {
            Debug.Assert(propertyName is string);
            if (!InternalUIThread.UnsafeIsBound())
            {
                throw new InvalidOperationException(
                    "Couldn't use GetValue() on worker thread context.");
            }

            var p = Prepare(ref properties);
            if (p.TryGetValue(propertyName!, out var value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static TValue GetValue<TValue>(
            TValue defaultValue,
            string? propertyName,
            ref Dictionary<string, object?>? properties)
        {
            Debug.Assert(propertyName is string);
            if (!InternalUIThread.UnsafeIsBound())
            {
                throw new InvalidOperationException(
                    "Couldn't use GetValue<TValue>() on worker thread context.");
            }

            var p = Prepare(ref properties);
            if (p.TryGetValue(propertyName!, out var value) && value is TValue v)
            {
                return v;
            }
            else
            {
                return defaultValue!;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static ValueTask<Unit> SetValueAsync<TValue>(
            TValue newValue,
            Func<TValue, ValueTask<Unit>>? propertyChanged,
            string? propertyName,
            object sender,
            PropertyChangingEventHandler? propertyChangingHandler,
            PropertyChangedEventHandler? propertyChangedHandler,
            ref Dictionary<string, object?>? properties)
        {
            Debug.Assert(propertyName is string);
            if (!InternalUIThread.UnsafeIsBound())
            {
                throw new InvalidOperationException(
                    "Couldn't use SetValue() on worker thread context.");
            }

            var p = Prepare(ref properties);
            if (p.TryGetValue(propertyName!, out var oldValue))
            {
                if (!DefaultValue<TValue>.ValueEquals(oldValue, newValue))
                {
                    propertyChangingHandler?.Invoke(sender, new PropertyChangingEventArgs(propertyName));

                    if (!DefaultValue.IsDefaultValue(newValue))
                    {
                        p[propertyName!] = newValue!;
                    }
                    else
                    {
                        p.Remove(propertyName!);
                    }

                    propertyChangedHandler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
                    if (propertyChanged is { } pc)
                    {
                        return pc.Invoke(newValue);
                    }
                }
            }
            else
            {
                propertyChangingHandler?.Invoke(sender, new PropertyChangingEventArgs(propertyName));

                if (!DefaultValue.IsDefaultValue(newValue))
                {
                    p.Add(propertyName!, newValue!);
                }
                else
                {
                    p.Remove(propertyName!);
                }

                propertyChangedHandler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
                if (propertyChanged is { } pc)
                {
                    return pc.Invoke(newValue);
                }
            }

            return default;
        }

        [DebuggerStepThrough]
        public static void OnPropertyChanging(
            object sender,
            PropertyChangingEventHandler? propertyChangingHandler,
            string? propertyName)
        {
            Debug.Assert(propertyName is string);
            if (!InternalUIThread.UnsafeIsBound())
            {
                throw new InvalidOperationException(
                    "Couldn't use OnPropertyChanging() on worker thread context.");
            }

            propertyChangingHandler?.Invoke(sender, new PropertyChangingEventArgs(propertyName));
        }

        [DebuggerStepThrough]
        public static void OnPropertyChanged(
            object sender,
            PropertyChangedEventHandler? propertyChangedHandler,
            string? propertyName)
        {
            Debug.Assert(propertyName is string);
            if (!InternalUIThread.UnsafeIsBound())
            {
                throw new InvalidOperationException(
                    "Couldn't use OnPropertyChanged() on worker thread context.");
            }

            propertyChangedHandler?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string PrettyPrint(object self, bool includeFields) =>
            string.Join(
                ",",
                (includeFields ?
                    self.EnumerateFields().Concat(self.EnumerateProperties()) :
                    self.EnumerateProperties()).
                OrderBy(entry => entry.Key).
                Select(entry => $"{entry.Key}={entry.Value ?? "(null)"}"));
    }
}
