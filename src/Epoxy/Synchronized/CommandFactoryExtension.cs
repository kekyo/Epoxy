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

namespace Epoxy.Synchronized
{
    public static class CommandFactoryExtension
    {
        public static Command CreateSync(
            this CommandFactoryInstance factory,
            Action execute) =>
            new SyncDelegatedCommand(execute);

        public static Command CreateSync(
            this CommandFactoryInstance factory,
            Action execute,
            Func<bool> canExecute) =>
            new SyncDelegatedCommand(execute, canExecute);

        public static Command CreateSync<TParameter>(
            this CommandFactoryInstance factory,
            Action<TParameter> execute) =>
            new SyncDelegatedCommand<TParameter>(execute);

        public static Command CreateSync<TParameter>(
            this CommandFactoryInstance factory,
            Action<TParameter> execute,
            Func<TParameter, bool> canExecute) =>
            new SyncDelegatedCommand<TParameter>(execute, canExecute);

        [Obsolete("Use Create instead.", true)]
        public static Command CreateSync(
            this CommandFactoryInstance factory,
            Func<ValueTask> execute) =>
            throw new InvalidOperationException("Use Create instead.");

        [Obsolete("Use Create instead.", true)]
        public static Command CreateSync(
            this CommandFactoryInstance factory,
            Func<ValueTask> execute,
            Func<bool> canExecute) =>
            throw new InvalidOperationException("Use Create instead.");

        [Obsolete("Use Create instead.", true)]
        public static Command CreateSync<TParameter>(
            this CommandFactoryInstance factory,
            Func<TParameter, ValueTask> execute) =>
            throw new InvalidOperationException("Use Create instead.");

        [Obsolete("Use Create instead.", true)]
        public static Command CreateSync<TParameter>(
            this CommandFactoryInstance factory,
            Func<TParameter, ValueTask> execute,
            Func<TParameter, bool> canExecute) =>
            throw new InvalidOperationException("Use Create instead.");
    }
}
