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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Epoxy
{
    public interface IAsyncCommand
    {
        ValueTask ExecuteAsync(object? parameter);
    }

    public sealed class UnobservedExceptionEventArgs : EventArgs
    {
        public UnobservedExceptionEventArgs(Exception ex) =>
            this.Exception = ex;

        public readonly Exception Exception;
    }

    public abstract class Command : ICommand, IAsyncCommand
    {
        public static EventHandler<UnobservedExceptionEventArgs>? UnobservedException;

        protected Command()
        { }

        public event EventHandler? CanExecuteChanged;

        public void ChangeCanExecute() =>
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        protected abstract bool OnCanExecute(object? parameter);

        private protected virtual void OnExecute(object? parameter) =>
            _ = this.OnExecuteAsync(parameter);

        private protected virtual ValueTask OnExecuteAsync(object? parameter)
        {
            this.OnExecute(parameter);
            return new ValueTask();
        }

        public bool CanExecute(object? parameter) =>
            this.OnCanExecute(parameter);

        public async ValueTask ExecuteAsync(object? parameter)
        {
            var task = this.OnExecuteAsync(parameter);

            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (UnobservedException is EventHandler<UnobservedExceptionEventArgs> ue)
                {
                    ue.Invoke(this, new UnobservedExceptionEventArgs(ex));
                }
                else
                {
                    Debug.Fail($"Epoxy: unobserved {ex.GetType().FullName}: {ex.Message}");
                }
            }
        }

        public void Execute(object? parameter) =>
            this.OnExecute(parameter);

        public static readonly CommandFactory Factory =
            CommandFactory.Instance;

        public static Command Create(
            Func<ValueTask> executeAsync) =>
            new DelegatedAsyncCommand(executeAsync);

        public static Command Create(
            Func<ValueTask> executeAsync,
            Func<bool> canExecute) =>
            new DelegatedAsyncCommand(executeAsync, canExecute);

        public static Command Create<TParameter>(
            Func<TParameter, ValueTask> executeAsync) =>
            new DelegatedAsyncCommand<TParameter>(executeAsync);

        public static Command Create<TParameter>(
            Func<TParameter, ValueTask> executeAsync,
            Func<TParameter, bool> canExecute) =>
            new DelegatedAsyncCommand<TParameter>(executeAsync, canExecute);
    }

    public sealed class CommandFactory
    {
        private CommandFactory()
        {
        }

        public Command Create(
            Func<ValueTask> executeAsync) =>
            new DelegatedAsyncCommand(executeAsync);

        public Command Create(
            Func<ValueTask> executeAsync,
            Func<bool> canExecute) =>
            new DelegatedAsyncCommand(executeAsync, canExecute);

        public Command Create<TParameter>(
            Func<TParameter, ValueTask> executeAsync) =>
            new DelegatedAsyncCommand<TParameter>(executeAsync);

        public Command Create<TParameter>(
            Func<TParameter, ValueTask> executeAsync,
            Func<TParameter, bool> canExecute) =>
            new DelegatedAsyncCommand<TParameter>(executeAsync, canExecute);

        public static readonly CommandFactory Instance =
            new CommandFactory();
    }

    public static class CommandFactoryExtension
    {
        public static Command Create(
            this CommandFactory factory,
            Func<Task> executeAsync) =>
            new DelegatedAsyncCommand(() => new ValueTask(executeAsync()));

        public static Command Create(
            this CommandFactory factory,
            Func<Task> executeAsync,
            Func<bool> canExecute) =>
            new DelegatedAsyncCommand(() => new ValueTask(executeAsync()), canExecute);

        public static Command Create<TParameter>(
            this CommandFactory factory,
            Func<TParameter, Task> executeAsync) =>
            new DelegatedAsyncCommand<TParameter>(parameter => new ValueTask(executeAsync(parameter)));

        public static Command Create<TParameter>(
            this CommandFactory factory,
            Func<TParameter, Task> executeAsync,
            Func<TParameter, bool> canExecute) =>
            new DelegatedAsyncCommand<TParameter>(parameter => new ValueTask(executeAsync(parameter)), canExecute);
    }
}
