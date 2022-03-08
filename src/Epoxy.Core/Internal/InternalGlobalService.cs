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

using Epoxy.Advanced;

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Epoxy.Internal
{
    internal static class InternalGlobalService
    {
        private sealed class ServiceHolder<TService>
        {
            public static TService? Instance = default;
        }

        private static object? GetInstance(Type targetType)
        {
            var holderType = typeof(ServiceHolder<>).MakeGenericType(targetType);
            return holderType.
                GetField("Instance", BindingFlags.Public | BindingFlags.Static)!.
                GetValue(null);
        }

        private static void SetInstance(Type targetType, object? instance, bool ignoreIfPresent)
        {
            var holderType = typeof(ServiceHolder<>).MakeGenericType(targetType);
            var fi = holderType.
                GetField("Instance", BindingFlags.Public | BindingFlags.Static)!;

            if (!ignoreIfPresent || (fi.GetValue(null) == null))
            {
                fi.SetValue(null, instance);
            }
        }

        public static void Register(object instance, RegisteringValidations validation)
        {
            var targetType = instance.GetType();
            var interfaces = targetType.GetInterfaces().
                Where(it => it.IsDefined<GlobalServiceAttribute>()).
                ToArray()!;
            if (validation == RegisteringValidations.Strict)
            {
                var assigned = interfaces.
                    Where(it => GetInstance(it) != null).
                    ToArray();
                if (assigned.Length >= 1)
                {
                    throw new InvalidOperationException(
                        $"GlobalService: Service already assigned: Types=[{string.Join(",", assigned.Select(t => t.FullName))}]");
                }
            }

            foreach (var it in interfaces)
            {
                SetInstance(it, instance, validation == RegisteringValidations.UnsafePartial);
            }
        }

        public static void Unregister(object instance)
        {
            var targetType = instance.GetType();
            var interfaces = targetType.GetInterfaces().
               Where(it => it.IsDefined<GlobalServiceAttribute>()).
               ToArray()!;
            foreach (var it in interfaces)
            {
                SetInstance(it, null, false);
            }
        }

        public static readonly GlobalServiceAccessor Accessor =
            new GlobalServiceAccessor();

        public static ValueTask ExecuteAsync<TService>(
            Func<TService, ValueTask> action, bool ignoreNotPresent) =>
            ServiceHolder<TService>.Instance is { } instance ?
                action(instance) :
                (ignoreNotPresent ? default :
                    throw new InvalidOperationException($"GlobalService: Service didn't assign: Type={typeof(TService).FullName}"));

        public static ValueTask<TResult> ExecuteAsync<TService, TResult>(
            Func<TService, ValueTask<TResult>> action) =>
            ServiceHolder<TService>.Instance is { } instance ?
                action(instance) :
                throw new InvalidOperationException($"GlobalService: Service didn't assign: Type={typeof(TService).FullName}");

        public static void ExecuteSync<TService>(
            Action<TService> action, bool ignoreNotPresent)
        {
            if (ServiceHolder<TService>.Instance is { } instance)
            {
                action(instance);
            }
            else if (!ignoreNotPresent)
            {
                throw new InvalidOperationException($"GlobalService: Service didn't assign: Type={typeof(TService).FullName}");
            }
        }

        public static TResult ExecuteSync<TService, TResult>(
            Func<TService, TResult> action) =>
            ServiceHolder<TService>.Instance is { } instance?
                action(instance) :
                throw new InvalidOperationException($"GlobalService: Service didn't assign: Type={typeof(TService).FullName}");
    }
}
