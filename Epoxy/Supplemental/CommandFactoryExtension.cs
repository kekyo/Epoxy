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
using System.Threading.Tasks;

using Epoxy.Internal;

namespace Epoxy.Supplemental
{
    public static class CommandFactoryExtension
    {
        public static Command Create(
            this CommandFactory factory,
            Func<Task> executeAsync) =>
            new DelegatedCommand(() => InternalHelpers.FromTask(executeAsync()));

        public static Command Create(
            this CommandFactory factory,
            Func<Task> executeAsync,
            Func<bool> canExecute) =>
            new DelegatedCommand(() => InternalHelpers.FromTask(executeAsync()), canExecute);

        public static Command Create<TParameter>(
            this CommandFactory factory,
            Func<TParameter, Task> executeAsync) =>
            new DelegatedCommand<TParameter>(parameter => InternalHelpers.FromTask(executeAsync(parameter)));

        public static Command Create<TParameter>(
            this CommandFactory factory,
            Func<TParameter, Task> executeAsync,
            Func<TParameter, bool> canExecute) =>
            new DelegatedCommand<TParameter>(parameter => InternalHelpers.FromTask(executeAsync(parameter)), canExecute);
    }
}
