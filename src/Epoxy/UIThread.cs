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

using Epoxy.Supplemental;
using Epoxy.Internal;

using System.ComponentModel;

namespace Epoxy
{
    /// <summary>
    /// UI thread commonly manipulator.
    /// </summary>
    public static class UIThread
    {
        /// <summary>
        /// Detects current thread context on the UI thread.
        /// </summary>
        public static bool IsBound =>
            InternalUIThread.IsBound;

        /// <summary>
        /// Detects current thread context on the UI thread.
        /// </summary>
        /// <remarks>This method is used internal only.
        /// You may have to use IsBound property instead.</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool UnsafeIsBound() =>
            InternalUIThread.UnsafeIsBound();

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
    }
}
