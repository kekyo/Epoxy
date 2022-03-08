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

using System;

using Epoxy.Internal;

namespace Epoxy.Synchronized
{
    public sealed class SyncDelegatedCommand : Command
    {
        private static readonly Func<bool> defaultCanExecute =
            () => true;

        private readonly Action execute;
        private readonly Func<bool> canExecute;

        internal SyncDelegatedCommand(
            Action execute)
        {
            this.execute = execute;
            this.canExecute = defaultCanExecute;
        }

        internal SyncDelegatedCommand(
            Action execute,
            Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        protected override bool OnCanExecute(object? parameter)
        {
            if (parameter != null)
            {
                throw new ArgumentException(
                    $"SyncDelegatedCommand.OnCanExecute: Invalid parameter given in {this.GetPrettyTypeName()}: Parameter={parameter.GetPrettyTypeName()}");
            }

            return (parameter == null) && canExecute.Invoke();
        }

        private protected override void OnExecute(object? parameter) =>
            execute();
    }

    public sealed class SyncDelegatedCommand<TParameter> : Command
    {
        private static readonly Func<TParameter, bool> defaultCanExecute =
            _ => true;

        private readonly Action<TParameter> execute;
        private readonly Func<TParameter, bool> canExecute;

        internal SyncDelegatedCommand(
            Action<TParameter> execute)
        {
            this.execute = execute;
            this.canExecute = defaultCanExecute;
        }

        internal SyncDelegatedCommand(
            Action<TParameter> execute,
            Func<TParameter, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        protected override bool OnCanExecute(object? parameter)
        {
            if (parameter is not TParameter &&
                !DefaultValue.IsDefault<TParameter>(parameter))
            {
                throw new ArgumentException(
                    $"SyncDelegatedCommand.OnCanExecute: Invalid parameter given in {this.GetPrettyTypeName()}: Parameter={parameter.GetPrettyTypeName()}");
            }

            return parameter is TParameter p && canExecute.Invoke(p);
        }

        private protected override void OnExecute(object? parameter) =>
            execute((TParameter)parameter!);
    }
}
