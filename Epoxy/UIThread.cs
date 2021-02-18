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

using System.Threading;

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
#elif WINDOWS_UWP
                return Windows.UI.Xaml.Window.Current.Dispatcher != null;
#elif XAMARIN_FORMS
                return SynchronizationContext.Current != null;
#endif
            }
        }

        public static UIThreadAwaitable Bind() =>
            new UIThreadAwaitable();
    }
}
