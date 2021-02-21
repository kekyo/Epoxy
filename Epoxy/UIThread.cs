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
                return System.Windows.Threading.DispatcherSynchronizationContext.Current != null;
#endif
#if WINDOWS_UWP || UNO
                return Windows.UI.Xaml.Window.Current.Dispatcher != null;
#endif
#if XAMARIN_FORMS
                return System.Threading.SynchronizationContext.Current != null;
#endif
#if AVALONIA
                return Avalonia.Threading.Dispatcher.UIThread.CheckAccess();
#endif
            }
        }

        public static UIThreadAwaitable Bind() =>
            new UIThreadAwaitable();
    }
}
