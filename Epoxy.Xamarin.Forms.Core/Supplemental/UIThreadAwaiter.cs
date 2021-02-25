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

using Xamarin.Forms;

namespace Epoxy.Supplemental
{
    partial class UIThreadAwaiter
    {
        public void OnCompleted(Action continuation)
        {
            var dispatcher = Application.Current?.Dispatcher;
            if (dispatcher == null)
            {
                // NOTE: Could't use SynchronizationContext.
                //   Because multi-platform targetter is capable for separated threading UI message pumps (ex: UWP).

                throw new InvalidOperationException("UI thread not found.");
            }

            if (!dispatcher.IsInvokeRequired)
            {
                this.IsCompleted = true;
                continuation();
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    this.IsCompleted = true;
                    continuation();
                });
            }
        }
    }
}
