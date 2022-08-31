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
using System.Threading;
using System.Threading.Tasks;

#if WINDOWS_UWP || WINUI || UNO
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
#endif

#if WINUI
using Microsoft.UI.Dispatching;
#endif

#if WINDOWS_WPF || OPENSILVER
using System.Windows;
using System.Windows.Threading;
#endif

#if OPENSILVER
using DotNetForHtml5.Core;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
#endif

#if MAUI
using Microsoft.Maui.Controls;
#endif

#if AVALONIA
using Avalonia.Threading;
#endif

namespace Epoxy.Internal
{
    internal static partial class InternalUIThread
    {
        private static readonly ThreadLocal<bool?> ids = new ThreadLocal<bool?>();

        private static bool InternalIsBound()
        {
#if WINDOWS_WPF
            var dispatcher = Application.Current?.Dispatcher;
            if (dispatcher?.CheckAccess() ?? false)
            {
                return true;
            }
#endif
#if AVALONIA
            var dispatcher = Dispatcher.UIThread;
            if (dispatcher?.CheckAccess() ?? false)
            {
                return true;
            }
#endif
#if WINDOWS_UWP || WINUI || UNO
            var dispatcher = CoreWindow.GetForCurrentThread()?.Dispatcher;
            if (dispatcher == null)
            {
                dispatcher = CoreApplication.MainView?.CoreWindow?.Dispatcher;
            }
            if (dispatcher?.HasThreadAccess ?? false)
            {
                return true;
            }
#endif
#if WINUI
            var dispatcher2 = DispatcherQueue.GetForCurrentThread();
            if (dispatcher2?.HasThreadAccess ?? false)
            {
                return true;
            }
#endif
#if XAMARIN_FORMS
            var dispatcher = Application.Current?.Dispatcher;
            if (!(dispatcher?.IsInvokeRequired ?? true))
            {
                return true;
            }
#endif
#if MAUI
            var dispatcher = Application.Current?.Dispatcher;
            if (!(dispatcher?.IsDispatchRequired ?? true))
            {
                return true;
            }
#endif

            if (dispatcher == null)
            {
                try
                {
                    // Check equality of UI thread.
                    if (SynchronizationContext.Current is { } context)
                    {
                        var id = -1;
                        context.Send(_ => id = Thread.CurrentThread.ManagedThreadId, null);
                        return id == Thread.CurrentThread.ManagedThreadId;
                    }
                }
                catch
                {
                    // On UWP, will cause NotSupportedException.
                }
            }

            return false;
        }

        public static ValueTask<bool> IsBoundAsync()
        {
            switch (ids.Value)
            {
                case true:
                    return new ValueTask<bool>(true);
                case false:
                    return new ValueTask<bool>(false);
                default:
#if OPENSILVER
                    if (Application.Current?.RootVisual?.Dispatcher is { } dispatcher)
                    {
                        var id = Thread.CurrentThread.ManagedThreadId;
                        var tcs = new TaskCompletionSource<bool>();
                        dispatcher.BeginInvoke(new Action(() =>
                            tcs.TrySetResult(id == Thread.CurrentThread.ManagedThreadId)));
                        return new ValueTask<bool>(tcs.Task);
                    }
#endif
                    var f = InternalIsBound();
                    ids.Value = f;
                    return new ValueTask<bool>(f);
            }
        }

        public static bool UnsafeIsBound()
        {
            switch (ids.Value)
            {
                case true:
                    return true;
                case false:
                    break;
                default:
#if OPENSILVER
                    // Could not check in OpenSilver.
                    return true;
#else
                    var f = InternalIsBound();
                    ids.Value = f;
                    if (f)
                    {
                        return true;
                    }
                    break;
#endif
            }
#if XAMARIN_FORMS
            // Workaround XF on UWP:
            //   The dispatcher will make invalid result for IsInvokeRequired in
            //   BindableContext initialize sequence.
            if (Device.RuntimePlatform.Equals(Device.UWP))
            {
                return true;
            }
#endif
            return false;
        }

        public static void ContinueOnWorkerThread(Action continuation)
        {
            if (UnsafeIsBound())
            {
                ThreadPool.QueueUserWorkItem(_ => continuation(), null);
            }
            else
            {
                continuation();
            }
        }
    }
}
