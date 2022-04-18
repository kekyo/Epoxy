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

#if WINDOWS_UWP || WINUI || UNO
using Windows.UI.Core;
#endif

#if WINUI
using Microsoft.UI.Dispatching;
#endif

#if WINDOWS_WPF || OPENSILVER
using System.Windows;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
#endif

#if AVALONIA
using Avalonia.Threading;
#endif

namespace Epoxy.Internal
{
    internal static partial class InternalUIThread
    {
        private static readonly ThreadLocal<bool?> ids = new ThreadLocal<bool?>();

        public static bool IsBound
        {
            get
            {
                switch (ids.Value)
                {
                    case true:
                        return true;
                    case false:
                        return false;
                    default:
#if WINDOWS_WPF
                        if (object.ReferenceEquals(
                            Application.Current?.Dispatcher?.Thread,
                            Thread.CurrentThread))
                        {
                            ids.Value = true;
                            return true;
                        }
#endif
#if AVALONIA
                        if (Dispatcher.UIThread?.CheckAccess() ?? false)
                        {
                            ids.Value = true;
                            return true;
                        }
#endif
#if WINDOWS_UWP || WINUI || UNO
                        if (CoreWindow.GetForCurrentThread() is { } cw1 &&
                            (cw1.Dispatcher?.HasThreadAccess ?? false))
                        {
                            ids.Value = true;
                            return true;
                        }
#endif
#if WINUI
                        if (DispatcherQueue.GetForCurrentThread() is { } queue &&
                            queue.HasThreadAccess)
                        {
                            ids.Value = true;
                            return true;
                        }
#endif
#if XAMARIN_FORMS
                        if (Application.Current?.Dispatcher?.IsInvokeRequired ?? false)
                        {
                            ids.Value = true;
                            return true;
                        }
#endif
                        try
                        {
                            if (SynchronizationContext.Current is { } context)
                            {
                                var id = -1;
                                context.Send(_ => id = Thread.CurrentThread.ManagedThreadId, null);
                                var f = id == Thread.CurrentThread.ManagedThreadId;
                                ids.Value = f;
                                return f;
                            }
                        }
                        catch
                        {
                            // On UWP, will cause NotSupportedException.
                        }
                        ids.Value = false;
                        return false;
                }
            }
        }

        public static bool UnsafeIsBound()
        {
            if (IsBound)
            {
                return true;
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
