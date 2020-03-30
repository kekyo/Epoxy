////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
// Copyright (c) 2020 Kouji Matsui (@kozy_kekyo, @kekyo2)
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
using System.Diagnostics;
using System.Threading.Tasks;

namespace Epoxy
{
    public sealed class DelegatedAsyncCommand : Command
    {
        private static readonly Func<bool> defaultCanExecute =
            () => true;

        private readonly Func<ValueTask> executeAsync;
        private readonly Func<bool> canExecute;

        public DelegatedAsyncCommand(
            Func<ValueTask> executeAsync)
        {
            this.executeAsync = executeAsync;
            this.canExecute = defaultCanExecute;
        }

        public DelegatedAsyncCommand(
            Func<ValueTask> executeAsync,
            Func<bool> canExecute)
        {
            this.executeAsync = executeAsync;
            this.canExecute = canExecute;
        }

        protected override bool OnCanExecute(object parameter)
        {
            Debug.Assert(parameter == null);

            return (parameter == null) && canExecute.Invoke();
        }

        private protected override ValueTask OnExecuteAsync(object parameter) =>
            executeAsync();
    }

    public sealed class DelegatedAsyncCommand<TParameter> : Command
    {
        private static readonly Func<TParameter, bool> defaultCanExecute =
            _ => true;

        private readonly Func<TParameter, ValueTask> executeAsync;
        private readonly Func<TParameter, bool> canExecute;

        public DelegatedAsyncCommand(
            Func<TParameter, ValueTask> executeAsync)
        {
            this.executeAsync = executeAsync;
            this.canExecute = defaultCanExecute;
        }

        public DelegatedAsyncCommand(
            Func<TParameter, ValueTask> executeAsync,
            Func<TParameter, bool> canExecute)
        {
            this.executeAsync = executeAsync;
            this.canExecute = canExecute;
        }

        protected override bool OnCanExecute(object parameter)
        {
            Debug.Assert((parameter == null) || (parameter is TParameter));

            return parameter is TParameter p && canExecute.Invoke(p);
        }

        private protected override ValueTask OnExecuteAsync(object parameter) =>
            executeAsync((TParameter)parameter);
    }
}
