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
using System.ComponentModel;
using System.Threading;

#if WINDOWS_WPF
using System.Diagnostics;
using System.Windows;
#endif

#if WINDOWS_UWP || WINUI || UNO
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
#endif

#if AVALONIA
using Avalonia;
#endif

using Epoxy.Supplemental;

namespace Epoxy
{
    public static class UIThread
    {
        public static bool IsBound
        {
            get
            {
#if WINDOWS_WPF
                return object.ReferenceEquals(
                    Application.Current?.Dispatcher?.Thread,
                    Thread.CurrentThread);
#endif
#if AVALONIA
                return Avalonia.Threading.Dispatcher.UIThread?.CheckAccess() ?? false;
#endif
#if WINDOWS_UWP || WINUI || UNO
                if (CoreWindow.GetForCurrentThread() is { } cw1 &&
                    (cw1.Dispatcher?.HasThreadAccess ?? false))
                {
                    return true;
                }

                // Naive impl:
                //   Before view create timing, can't get the Dispatcher instance.
                //   So answered it if current thread id is 1. Exactly we know not equals both UI thread and main thread.
                //   For example: maybe success when main view model is initialized on main thread :)
                if (Thread.CurrentThread.ManagedThreadId == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
#endif
#if XAMARIN_FORMS
                return Application.Current?.Dispatcher?.IsInvokeRequired ?? false;
#endif
            }
        }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool UnsafeIsBound
        {
            get
            {
#if XAMARIN_FORMS
                if (IsBound)
                {
                    return true;
                }

                // Workaround XF on UWP:
                //   The dispatcher will make invalid result for IsInvokeRequired in
                //   BindableContext initialize sequence.
                else if (Device.RuntimePlatform.Equals(Device.UWP))
                {
                    return true;
                }
                else
#endif
                return IsBound;
            }
        }

        public static UIThreadAwaitable Bind() =>
            new UIThreadAwaitable();
    }
}
