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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Epoxy.Internal;

namespace Epoxy.Supplemental
{
    /// <summary>
    /// The ViewModel methods.
    /// </summary>
    [DebuggerStepThrough]
    public static class ViewModelExtension
    {
        /// <summary>
        /// Set value directly.
        /// </summary>
        /// <typeparam name="TValue">Value type</typeparam>
        /// <param name="viewModel">ViewModel instance</param>
        /// <param name="newValue">Value</param>
        /// <param name="propertyChanged">Callback delegate when value has changed</param>
        /// <param name="propertyName">Property name (Will auto insert by compiler)</param>
        /// <returns>ValueTask</returns>
        public static ValueTask SetValueAsync<TValue>(
            this ViewModel viewModel,
            TValue newValue,
            Func<TValue, Task> propertyChanged,
            [CallerMemberName] string? propertyName = null) =>
            viewModel.InternalSetValueAsync(
                newValue,
                value => propertyChanged(value).AsValueTaskUnit(),
                propertyName).
            AsValueTaskVoid();
    }
}
