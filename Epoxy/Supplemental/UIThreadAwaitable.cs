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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

#if WINDOWS_WPF
using System.Windows;
using System.Windows.Threading;
#endif

#if WINDOWS_UWP || UNO
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
#endif

#if AVALONIA
using Avalonia.Threading;
#endif

namespace Epoxy.Supplemental
{
    public struct UIThreadAwaitable
    {
        public UIThreadAwaiter GetAwaiter() =>
            new UIThreadAwaiter();
    }

    public sealed class UIThreadAwaiter : INotifyCompletion
    {
        internal UIThreadAwaiter()
        {
        }

        public bool IsCompleted { get; private set; }

        public void OnCompleted(Action continuation)
        {
#if WINDOWS_WPF
            var dispatcher = Application.Current?.Dispatcher;
            if (dispatcher == null)
            {
                throw new InvalidOperationException("UI thread not found.");
            }

            var _ = dispatcher.BeginInvoke(
               DispatcherPriority.Normal,
                new Action(() =>
                {
                    this.IsCompleted = true;
                    continuation();
                }));
#endif
#if WINDOWS_UWP || UNO
            var dispatcher = CoreApplication.MainView?.CoreWindow?.Dispatcher;
            if (dispatcher == null)
            {
                throw new InvalidOperationException("UI thread not found.");
            }

            var _ = dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    this.IsCompleted = true;
                    continuation();
                });
#endif
#if XAMARIN_FORMS
            Device.BeginInvokeOnMainThread(() =>
            {
                this.IsCompleted = true;
                continuation();
            });
#endif
#if AVALONIA
            var dispatcher = Dispatcher.UIThread;
            Dispatcher.UIThread.Post(() =>
            {
                this.IsCompleted = true;
                continuation();
            });
#endif
        }

        public void GetResult()
        {
            Debug.Assert(this.IsCompleted);
        }
    }
}
