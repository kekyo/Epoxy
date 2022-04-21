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
using Epoxy.Supplemental;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Epoxy
{
    [DebuggerStepThrough]
    public readonly struct TryInvokeAsyncResult<T>
    {
        public readonly bool IsBound;
        public readonly T Result;

        public TryInvokeAsyncResult(bool isBound, T result)
        {
            this.IsBound = isBound;
            this.Result = result;
        }

        public void Deconstruct(out bool isBound, out T result)
        {
            isBound = this.IsBound;
            result = this.Result;
        }
    }

    /// <summary>
    /// UI thread commonly manipulator.
    /// </summary>
    [DebuggerStepThrough]
    public static class UIThread
    {
        /// <summary>
        /// Detects current thread context on the UI thread.
        /// </summary>
        public static ValueTask<bool> IsBoundAsync() =>
            InternalUIThread.IsBoundAsync();

        /// <summary>
        /// Detects current thread context on the UI thread.
        /// </summary>
        [Obsolete("IsBound property is deprecated. Use IsBoundAsync() method instead.", true)]
        public static bool IsBound =>
            throw new NotImplementedException();

        /// <summary>
        /// Binds current task to the UI thread context manually.
        /// </summary>
        /// <returns>Awaitable UI thread object.</returns>
        /// <example>
        /// <code>
        /// // (On the arbitrary thread context here)
        /// 
        /// // Switch to UI thread context uses async-await.
        /// await UIThread.Bind();
        /// 
        /// // (On the UI thread context here)
        /// </code>
        /// </example>
        public static UIThreadAwaitable Bind() =>
            new UIThreadAwaitable();

        /// <summary>
        /// Try to bind current task to the UI thread context manually.
        /// </summary>
        /// <returns>True if bound UI thread</returns>
        /// <example>
        /// <code>
        /// // (On the arbitrary thread context here)
        /// 
        /// // Switch to UI thread context uses async-await.
        /// if (!(await UIThread.TryBind()))
        /// {
        ///     // Failed to bind (UI thread is not found, maybe reason is UI shutdown)
        ///     return;
        /// }
        /// 
        /// // (On the UI thread context here)
        /// </code>
        /// </example>
        public static UIThreadTryBindAwaitable TryBind() =>
            new UIThreadTryBindAwaitable();

        /// <summary>
        /// Unbinds current UI task to the worker thread context manually.
        /// </summary>
        /// <returns>Awaitable worker thread object.</returns>
        /// <example>
        /// <code>
        /// // (On the UI thread context here)
        /// 
        /// // Switch to worker thread context uses async-await.
        /// await UIThread.Unbind();
        /// 
        /// // (On the worker thread context here)
        /// </code>
        /// </example>
        public static UIThreadUnbindAwaitable Unbind() =>
            new UIThreadUnbindAwaitable();

        ////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="action">Action on UI thread context</param>
        /// <returns>Result</returns>
        public static async ValueTask<T> InvokeAsync<T>(Func<ValueTask<T>> action)
        {
            await Bind();
            return await action().ConfigureAwait(false);
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="action">Action on UI thread context</param>
        public static async ValueTask InvokeAsync(Func<ValueTask> action)
        {
            await Bind();
            await action().ConfigureAwait(false);
        }

        /// <summary>
        /// Execute on the UI thread context.
        /// </summary>
        /// <param name="action">Action on UI thread context</param>
        /// <returns>True if executed.</returns>
        public static async ValueTask<bool> TryInvokeAsync(Func<ValueTask> action)
        {
            if (await TryBind())
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
        /// <param name="action">Action on UI thread context</param>
        /// <returns>True if executed.</returns>
        public static async ValueTask<TryInvokeAsyncResult<T>> TryInvokeAsync<T>(Func<ValueTask<T>> action)
        {
            if (await TryBind())
            {
                return new TryInvokeAsyncResult<T>(true, await action().ConfigureAwait(false));
            }
            else
            {
                return new TryInvokeAsyncResult<T>(false, default!);
            }
        }

        ////////////////////////////////////////////////////////////////////////

        public static readonly UIThreadAccessorInstance Accessor =
            UIThreadAccessorInstance.Instace;
    }
}
