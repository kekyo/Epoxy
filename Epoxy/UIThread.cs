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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Epoxy
{
    public static class UIThread
    {
        public static bool IsBound
        {
            get
            {
#if WINDOWS_WPF
                return System.Windows.Threading.DispatcherSynchronizationContext.Current != null;
#elif WINDOWS_UWP
                return Windows.UI.Xaml.Window.Current.Dispatcher != null;
#elif XAMARIN_FORMS
                return SynchronizationContext.Current != null;
#endif
            }
        }

        public static UIThreadAwaitable Bind() =>
            new UIThreadAwaitable();

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
                var dispatcher = System.Windows.Application.Current?.Dispatcher;
                if (dispatcher == null)
                {
                    throw new InvalidOperationException("UI thread not found.");
                }

                var _ = dispatcher.BeginInvoke(
                    System.Windows.Threading.DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        this.IsCompleted = true;
                        continuation();
                    }));
#elif WINDOWS_UWP
                var dispatcher = Windows.UI.Xaml.Window.Current?.Dispatcher;
                if (dispatcher == null)
                {
                    throw new InvalidOperationException("UI thread not found.");
                }

                var _ = dispatcher.RunAsync(
                    Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {
                        this.IsCompleted = true;
                        continuation();
                    });
#elif XAMARIN_FORMS
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
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
}
