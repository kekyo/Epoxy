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

namespace Epoxy.Supplemental
{
    public static class CommandFactoryExtension
    {
        public static Command CreateSync(
            this CommandFactory factory,
            Action execute) =>
            new DelegatedCommand(execute);

        public static Command CreateSync(
            this CommandFactory factory,
            Action execute,
            Func<bool> canExecute) =>
            new DelegatedCommand(execute, canExecute);

        public static Command CreateSync<TParameter>(
            this CommandFactory factory,
            Action<TParameter> execute) =>
            new DelegatedCommand<TParameter>(execute);

        public static Command CreateSync<TParameter>(
            this CommandFactory factory,
            Action<TParameter> execute,
            Func<TParameter, bool> canExecute) =>
            new DelegatedCommand<TParameter>(execute, canExecute);
    }
}
