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
using System.Threading.Tasks;

#if WINDOWS_UWP || UNO
using Windows.UI.Xaml;
#endif

#if WINDOWS_WPF
using System.Windows;
#endif

#if XAMARIN_FORMS
using UIElement = Xamarin.Forms.Element;
#endif

#if AVALONIA
using UIElement = Avalonia.IStyledElement;
#endif

using Epoxy.Internal;

namespace Epoxy.Supplemental
{
    public static class PileExtension
    {
        public static ValueTask ExecuteAsync<TUIElement>(
            this Pile<TUIElement> pile,
            Func<TUIElement, Task> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            pile.ExecuteAsync(element => InternalHelpers.FromTask(action(element)), canIgnore);

        public static ValueTask<T> ExecuteAsync<TUIElement, T>(
            this Pile<TUIElement> pile,
            Func<TUIElement, Task<T>> action)
            where TUIElement : UIElement =>
            pile.ExecuteAsync(element => InternalHelpers.FromTask(action(element)));
    }
}
