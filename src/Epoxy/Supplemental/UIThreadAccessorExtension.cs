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
using System.Threading.Tasks;

namespace Epoxy.Supplemental
{
    /// <summary>
    /// UI thread commonly manipulator.
    /// </summary>
    [DebuggerStepThrough]
    public static class UIThreadAccessorExtension
    {
        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="accessor">UIThread accessor</param>
        /// <param name="action">Action on UI thread context</param>
        public static async ValueTask InvokeAsync(
            this UIThreadAccessorInstance accessor, Func<Task> action)
        {
            await UIThread.Bind();
            await action().ConfigureAwait(false);
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="accessor">UIThread accessor</param>
        /// <param name="action">Action on UI thread context</param>
        /// <returns>Result</returns>
        public static async ValueTask<T> InvokeAsync<T>(
            this UIThreadAccessorInstance accessor, Func<Task<T>> action)
        {
            await UIThread.Bind();
            return await action().ConfigureAwait(false);
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="accessor">UIThread accessor</param>
        /// <param name="action">Action on UI thread context</param>
        /// <returns>True if executed.</returns>
        public static async ValueTask<bool> TryInvokeAsync(
            this UIThreadAccessorInstance accessor, Func<Task> action)
        {
            if (await UIThread.TryBind())
            {
                await action().ConfigureAwait(false);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="accessor">UIThread accessor</param>
        /// <param name="action">Action on UI thread context</param>
        /// <returns>True if executed.</returns>
        public static async ValueTask<TryInvokeAsyncResult<T>> TryInvokeAsync<T>(
            this UIThreadAccessorInstance accessor, Func<Task<T>> action)
        {
            if (await UIThread.TryBind())
            {
                return new TryInvokeAsyncResult<T>(true, await action().ConfigureAwait(false));
            }
            else
            {
                return new TryInvokeAsyncResult<T>(false, default!);
            }
        }
    }
}
