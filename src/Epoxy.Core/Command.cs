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
using System.ComponentModel;
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
            return default;
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
                    Debug.Fail($"Epoxy.Command: unobserved {ex.GetType().FullName}: {ex.Message}");
                }
            }
        }

        public void Execute(object? parameter) =>
            this.OnExecute(parameter);

        public static readonly CommandFactoryInstance Factory =
            new CommandFactoryInstance();

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Command.Create is obsoleted. Use CommandFactory.Create instead.", true)]
        public static Command Create(
            Func<ValueTask> executeAsync) =>
            throw new InvalidOperationException("Command.Create is obsoleted. Use CommandFactory.Create instead.");

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Command.Create is obsoleted. Use CommandFactory.Create instead.", true)]
        public static Command Create(
            Func<ValueTask> executeAsync,
            Func<bool> canExecute) =>
            throw new InvalidOperationException("Command.Create is obsoleted. Use CommandFactory.Create instead.");

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Command.Create is obsoleted. Use CommandFactory.Create instead.", true)]
        public static Command Create<TParameter>(
            Func<TParameter, ValueTask> executeAsync) =>
            throw new InvalidOperationException("Command.Create is obsoleted. Use CommandFactory.Create instead.");

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Command.Create is obsoleted. Use CommandFactory.Create instead.", true)]
        public static Command Create<TParameter>(
            Func<TParameter, ValueTask> executeAsync,
            Func<TParameter, bool> canExecute) =>
            throw new InvalidOperationException("Command.Create is obsoleted. Use CommandFactory.Create instead.");
    }

    public sealed class CommandFactoryInstance
    {
        internal CommandFactoryInstance()
        {
        }
    }
}
