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

using Epoxy.Advanced;

namespace Epoxy.Synchronized
{
    public static class GlobalServiceAccessorExtension
    {
        public static void ExecuteSync<TService>(
            this GlobalServiceAccessor accessor, Action<TService> action, bool ignoreNotPresent = false) =>
            GlobalService.ExecuteSync(action, ignoreNotPresent);

        public static TResult ExecuteSync<TService, TResult>(
            this GlobalServiceAccessor accessor, Func<TService, TResult> action) =>
            GlobalService.ExecuteSync(action);
    }
}
