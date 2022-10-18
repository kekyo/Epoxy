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

using Windows.ApplicationModel.Core;
using Windows.UI.Core;

#if WINUI
using Microsoft.UI.Dispatching;
#endif

namespace Epoxy.Internal;

partial class InternalUIThread
{
    private static CoreDispatcher? TryGetDispatcher()
    {
        if (CoreWindow.GetForCurrentThread()?.Dispatcher is { } d1)
        {
            return d1;
        }
        else if (CoreApplication.MainView?.CoreWindow?.Dispatcher is { } d2)
        {
            return d2;
        }
        else
        {
            return null;
        }
    }

    public static void ContinueOnUIThread(Action<bool> continuation)
    {
        if (TryGetDispatcher() is { } dispatcher)
        {
            // Maybe anytime is true
            if (dispatcher.HasThreadAccess)
            {
                continuation(true);
            }
            else
            {
                try
                {
                    var _ = dispatcher.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () => continuation(true));
                }
                catch
                {
                    continuation(false);
                }
            }
        }
#if WINUI
        else if (DispatcherQueue.GetForCurrentThread() is { } dispatcherQueue)
        {
            if (dispatcherQueue.HasThreadAccess)
            {
                continuation(true);
            }
            else
            {
                try
                {
                    var _ = dispatcherQueue.TryEnqueue(
                        DispatcherQueuePriority.Normal,
                        () => continuation(true));
                }
                catch
                {
                    continuation(false);
                }
            }
        }
#endif
        else
        {
            continuation(false);
        }
    }
}
