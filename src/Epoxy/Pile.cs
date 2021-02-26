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
using System.Threading.Tasks;

#if WINDOWS_UWP || UNO
using Windows.UI.Xaml;
#endif

#if WINUI
using Microsoft.UI.Xaml;
#endif

#if WINDOWS_WPF
using System.Windows;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
using DependencyObject = Xamarin.Forms.BindableObject;
using UIElement = Xamarin.Forms.Element;
#endif

#if AVALONIA
using Avalonia;
using DependencyObject = Avalonia.IAvaloniaObject;
using UIElement = Avalonia.IStyledElement;
#endif

namespace Epoxy
{
    public static class PileFactory
    {
        public static Pile<UIElement> Create() =>
            new Pile<UIElement>();

        public static Pile<TUIElement> Create<TUIElement>()
            where TUIElement : UIElement =>
            new Pile<TUIElement>();
    }

    public static class PileExtension
    {
        public static ValueTask ExecuteAsync<TUIElement>(
            this Pile<TUIElement> pile, Func<TUIElement, ValueTask> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            pile.InternalExecuteAsync(action, canIgnore);

        public static ValueTask<TResult> ExecuteAsync<TUIElement, TResult>(
            this Pile<TUIElement> pile, Func<TUIElement, ValueTask<TResult>> action)
            where TUIElement : UIElement =>
            pile.InternalExecuteAsync<TResult>(action);
    }
}
