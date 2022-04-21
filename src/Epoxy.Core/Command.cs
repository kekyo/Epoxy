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

using Epoxy.Internal;

using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
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
        protected Command()
        { }

        public event EventHandler? CanExecuteChanged;

        public void ChangeCanExecute() =>
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        protected abstract bool OnCanExecute(object? parameter);

        private protected virtual async void OnExecute(object? parameter)
        {
            try
            {
                var _ = await this.OnExecuteAsync(parameter).
                    ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // HACK: Because the exception will ignore by 'async void' bottom stack,
                //   (And will reraise delaying UnobservedException on finalizer thread.)
                //   This captures logical task context and reraise on UI thread pumps immediately.
                var edi = ExceptionDispatchInfo.Capture(ex);
                InternalUIThread.ContinueOnUIThread(_ => edi.Throw());
            }
        }

        private protected virtual ValueTask<Unit> OnExecuteAsync(object? parameter)
        {
            this.OnExecute(parameter);
            return default;
        }

        public bool CanExecute(object? parameter) =>
            this.OnCanExecute(parameter);

        public ValueTask ExecuteAsync(object? parameter) =>
            this.OnExecuteAsync(parameter).AsValueTaskVoid();

        public void Execute(object? parameter) =>
            this.OnExecute(parameter);

        public static readonly CommandFactoryInstance Factory =
            CommandFactoryInstance.Instance;
    }

    [DebuggerStepThrough]
    public sealed class CommandFactoryInstance
    {
        private CommandFactoryInstance()
        {
        }

        internal static readonly CommandFactoryInstance Instance = 
            new CommandFactoryInstance();
    }
}
