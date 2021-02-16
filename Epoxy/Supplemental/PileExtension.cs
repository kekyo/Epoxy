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
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

#if WINDOWS_UWP
using Windows.UI.Xaml;
#endif

#if WINDOWS_WPF
using System.Windows;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
using DependencyObject = Xamarin.Forms.BindableObject;
using UIElement = Xamarin.Forms.Element;
#endif

namespace Epoxy.Supplemental
{
    public static class PileExtension
    {
        public static void ExecuteSync<TUIElement>(
            this Pile<TUIElement> pile,
            Action<TUIElement> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            pile.ExecuteAsync(element => { action(element); return default; }, canIgnore);

        public static T ExecuteSync<TUIElement, T>(
            this Pile<TUIElement> pile,
            Func<TUIElement, T> action)
            where TUIElement : UIElement
        {
            var (result, edi) = pile.ExecuteAsync(element =>
            {
                try
                {
                    return new ValueTask<(T, ExceptionDispatchInfo?)>((action(element), default));
                }
                catch (Exception ex)
                {
                    return new ValueTask<(T, ExceptionDispatchInfo?)>((default!, ExceptionDispatchInfo.Capture(ex)));
                }
            }).Result;  // Will not block

            edi?.Throw();
            return result;
        }
    }
}
