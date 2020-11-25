﻿////////////////////////////////////////////////////////////////////////////
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

namespace Epoxy
{
    public sealed class DelegatedCommand : Command
    {
        private static readonly Func<bool> defaultCanExecute =
            () => true;

        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public DelegatedCommand(
            Action execute)
        {
            this.execute = execute;
            this.canExecute = defaultCanExecute;
        }

        public DelegatedCommand(
            Action execute,
            Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        protected override bool OnCanExecute(object? parameter)
        {
            Debug.Assert(parameter == null);

            return (parameter == null) && canExecute.Invoke();
        }

        private protected override void OnExecute(object? parameter) =>
            execute();
    }

    public sealed class DelegatedCommand<TParameter> : Command
    {
        private static readonly Func<TParameter, bool> defaultCanExecute =
            _ => true;

        private readonly Action<TParameter> execute;
        private readonly Func<TParameter, bool> canExecute;

        public DelegatedCommand(
            Action<TParameter> execute)
        {
            this.execute = execute;
            this.canExecute = defaultCanExecute;
        }

        public DelegatedCommand(
            Action<TParameter> execute,
            Func<TParameter, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        protected override bool OnCanExecute(object? parameter)
        {
            Debug.Assert((parameter == null) || (parameter is TParameter));

            return parameter is TParameter p && canExecute.Invoke(p);
        }

        private protected override void OnExecute(object? parameter) =>
            execute((TParameter)parameter!);
    }
}
