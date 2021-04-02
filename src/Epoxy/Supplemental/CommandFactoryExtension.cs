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
    /// <summary>
    /// Command factory methods for Task based asynchronous handler.
    /// </summary>
    public static class CommandFactoryExtension
    {
        /// <summary>
        /// Generate a Command instance with Task based asynchronous handler.
        /// </summary>
        /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
        /// <param name="executeAsync">Asynchronous handler</param>
        /// <returns>A Command instance</returns>
        public static Command Create(
            this CommandFactoryInstance factory,
            Func<Task> executeAsync) =>
            new DelegatedCommand(() => executeAsync().AsValueTaskUnit());

        /// <summary>
        /// Generate a Command instance with Task based asynchronous handler.
        /// </summary>
        /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
        /// <param name="executeAsync">Asynchronous handler</param>
        /// <param name="canExecute">Responder for be able to execute</param>
        /// <returns>A Command instance</returns>
        public static Command Create(
            this CommandFactoryInstance factory,
            Func<Task> executeAsync,
            Func<bool> canExecute) =>
            new DelegatedCommand(() => executeAsync().AsValueTaskUnit(), canExecute);

        /// <summary>
        /// Generate a Command instance with Task based asynchronous handler.
        /// </summary>
        /// <typeparam name="TParameter">Handler parameter type</typeparam>
        /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
        /// <param name="executeAsync">Asynchronous handler</param>
        /// <returns>A Command instance</returns>
        public static Command Create<TParameter>(
            this CommandFactoryInstance factory,
            Func<TParameter, Task> executeAsync) =>
            new DelegatedCommand<TParameter>(parameter => executeAsync(parameter).AsValueTaskUnit());

        /// <summary>
        /// Generate a Command instance with Task based asynchronous handler.
        /// </summary>
        /// <typeparam name="TParameter">Handler parameter type</typeparam>
        /// <param name="factory">Factory instance (will use only fixup by compiler)</param>
        /// <param name="executeAsync">Asynchronous handler</param>
        /// <param name="canExecute">Responder for be able to execute</param>
        /// <returns>A Command instance</returns>
        public static Command Create<TParameter>(
            this CommandFactoryInstance factory,
            Func<TParameter, Task> executeAsync,
            Func<TParameter, bool> canExecute) =>
            new DelegatedCommand<TParameter>(parameter => executeAsync(parameter).AsValueTaskUnit(), canExecute);
    }
}
