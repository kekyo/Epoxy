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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Epoxy.Internal
{
    [DebuggerStepThrough]
    internal static class InternalHelpers
    {
        public static readonly object[] EmptyArgs = new object[0];

#if WINDOWS_UWP || UNO
        public static bool IsPrimitive(this Type type) =>
            type.GetTypeInfo().IsPrimitive;
        public static bool IsClass(this Type type) =>
            type.GetTypeInfo().IsClass;
        public static bool IsDefined<TAttribute>(this Type type) =>
            type.GetTypeInfo().IsDefined(typeof(TAttribute));
#else
        public static bool IsPrimitive(this Type type) =>
            type.IsPrimitive;
        public static bool IsClass(this Type type) =>
            type.IsClass;
        public static bool IsDefined<TAttribute>(this Type type) =>
            type.IsDefined(typeof(TAttribute));
#endif

        public static string GetPrettyTypeName(this object? value) =>
            value?.GetType().FullName ?? "(null)";

        public static KeyValuePair<TKey, TValue> Pair<TKey, TValue>(TKey key, TValue value) =>
            new KeyValuePair<TKey, TValue>(key, value);

        public static ValueTask<T> FromResult<T>(T value) =>
            new ValueTask<T>(value);
        public static ValueTask<T> FromTask<T>(Task<T> task) =>
            new ValueTask<T>(task);
        public static ValueTask FromTask(Task task) =>
            new ValueTask(task);

        public static IEnumerable<KeyValuePair<string, object?>> EnumerateFields(this object self) =>
            self.GetType().GetFields().
            Where(f => f.IsPublic && !f.IsStatic).
            Select(f => Pair(f.Name, (object?)f.GetValue(self)));

        public static IEnumerable<KeyValuePair<string, object?>> EnumerateProperties(this object self) =>
            self.GetType().GetProperties().
            Where(p =>
                p.CanRead &&
                (p.Name != "PrettyPrint") &&
                (p.GetIndexParameters().Length == 0) &&
                (p.PropertyType.IsPrimitive() || p.PropertyType == typeof(string)) &&
                p.GetGetMethod() is MethodInfo m &&
                m.IsPublic && !m.IsStatic).
            Select(p => Pair(p.Name, (object?)p.GetValue(self, EmptyArgs)));

        public static IEnumerable<T> Traverse<T>(this T instance, Func<T, T?> traverser)
        {
            T? current = instance;
            while (current != null)
            {
                yield return current;
                current = traverser(current);
            }
        }
    }
}
