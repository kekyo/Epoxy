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
using System.Windows;

namespace Epoxy.Internal
{
    partial class InternalUIThread
    {
        public static void ContinueOnUIThread(Action<bool> continuation)
        {
            if (Application.Current?.RootVisual?.Dispatcher is { } dispatcher)
            {
                try
                {
                    var _ = dispatcher.BeginInvoke(new Action(() => continuation(true)));
                }
                catch
                {
                    continuation(false);
                }
            }
            else
            {
                continuation(false);
            }
        }
    }
}
