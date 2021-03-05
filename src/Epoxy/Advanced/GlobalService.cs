﻿////////////////////////////////////////////////////////////////////////////
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
using System.Threading.Tasks;

using Epoxy.Internal;

namespace Epoxy.Advanced
{
    public static class GlobalService
    {
        public static readonly GlobalServiceAccessor Accessor =
            new GlobalServiceAccessor();

        public static void Register(
            object instance, RegisteringValidations validation = RegisteringValidations.Strict) =>
            InternalGlobalService.Register(instance, validation);

        public static ValueTask ExecuteAsync<TService>(Func<TService, ValueTask> action, bool ignoreNotPresent = false) =>
            InternalGlobalService.ExecuteAsync<TService>(action, ignoreNotPresent);

        public static ValueTask<TResult> ExecuteAsync<TService, TResult>(Func<TService, ValueTask<TResult>> action) =>
            InternalGlobalService.ExecuteAsync<TService, TResult>(action);

        public static ValueTask ExecuteAsync<TService>(
            this GlobalServiceAccessor accessor,
            Func<TService, ValueTask> action,
            bool ignoreNotPresent = false) =>
            InternalGlobalService.ExecuteAsync(action, ignoreNotPresent);

        public static ValueTask<TResult> ExecuteAsync<TService, TResult>(
            this GlobalServiceAccessor accessor,
            Func<TService, ValueTask<TResult>> action) =>
            InternalGlobalService.ExecuteAsync(action);
    }
}