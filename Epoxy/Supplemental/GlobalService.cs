////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Epoxy.Supplemental
{
    public enum RegisteringValidations
    {
        Strict,
        UnsafePartial,
        UnsafeOverride
    }

    public static class GlobalService
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

        private static void SetInstance(Type targetType, object? instance)
        {
            var holderType = typeof(ServiceHolder<>).MakeGenericType(targetType);
            holderType.
                GetField("Instance", BindingFlags.Public | BindingFlags.Static)!.
                SetValue(null, instance);
        }

        public static void Register(
            object instance, RegisteringValidations validation = RegisteringValidations.Strict)
        {
            var targetType = instance.GetType();
            if (validation != RegisteringValidations.UnsafeOverride)
            {
                if (GetInstance(targetType) != null)
                {
                    throw new InvalidOperationException(
                        $"GlobalService: Service already assigned: Type={targetType.FullName}");
                }
            }
            if (validation == RegisteringValidations.Strict)
            {
                var assigned = targetType.GetInterfaces().
                    Where(it => GetInstance(it) != null).
                    ToArray();
                if (assigned.Length >= 1)
                {
                    throw new InvalidOperationException(
                        $"GlobalService: Service already assigned: Types=[{string.Join(",", assigned.Select(t => t.FullName))}]");
                }
            }

            SetInstance(targetType, instance);

            switch (validation)
            {
                case RegisteringValidations.UnsafePartial:
                    foreach (var it in targetType.GetInterfaces().
                        Where(it => GetInstance(it) == null))
                    {
                        SetInstance(it, instance);
                    }
                    break;
                default:
                    foreach (var it in targetType.GetInterfaces())
                    {
                        SetInstance(it, instance);
                    }
                    break;
            }
        }

        public static void UnRegister(object instance)
        {
            var targetType = instance.GetType();
            SetInstance(targetType, null);

            foreach (var it in instance.GetType().GetInterfaces())
            {
                SetInstance(it, null);
            }
        }

        public static ValueTask ExecuteAsync<TService>(Func<TService, ValueTask> action, bool ignoreNotPresent = false) =>
            ServiceHolder<TService>.Instance is { } instance ?
                action(instance) :
                (ignoreNotPresent ? default :
                    throw new InvalidOperationException($"GlobalService: Service didn't assign: Type={typeof(TService).FullName}"));

        public static ValueTask<TResult> ExecuteAsync<TService, TResult>(Func<TService, ValueTask<TResult>> action) =>
            ServiceHolder<TService>.Instance is { } instance ?
                action(instance) :
                throw new InvalidOperationException($"GlobalService: Service didn't assign: Type={typeof(TService).FullName}");
    }
}
