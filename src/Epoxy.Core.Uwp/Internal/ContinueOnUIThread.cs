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

using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Epoxy.Internal
{
    partial class InternalUIThread
    {
        public static void ContinueOnUIThread(Action continuation)
        {
            if (CoreWindow.GetForCurrentThread()?.Dispatcher is { } d1)
            {
                // Maybe anytime is true
                if (d1.HasThreadAccess)
                {
                    continuation();
                }
                else
                {
                    var _ = d1.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () => continuation());
                }
                return;
            }

            try
            {
                if (CoreApplication.MainView?.CoreWindow?.Dispatcher is { } d2)
                {
                    if (d2.HasThreadAccess)
                    {
                        continuation();
                    }
                    else
                    {
                        var _ = d2.RunAsync(
                            CoreDispatcherPriority.Normal,
                            () => continuation());
                    }
                    return;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("UI thread not found.", ex);
            }

            throw new InvalidOperationException("UI thread not found.");
        }
    }
}
