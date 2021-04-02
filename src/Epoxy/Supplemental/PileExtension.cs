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

#if WINUI
using Microsoft.UI.Xaml;
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
    /// <summary>
    /// Pile methods for Task based asynchronous execution.
    /// </summary>
    /// <remarks>You can manipulate XAML controls directly inside ViewModels
    /// when places and binds both an Anchor (in XAML) and a Pile.</remarks>
    public static class PileExtension
    {
        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="TUIElement">UI element type</typeparam>
        /// <param name="pile">Pile instance</param>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>ValueTask</returns>
        public static ValueTask ExecuteAsync<TUIElement>(
            this Pile<TUIElement> pile,
            Func<TUIElement, Task> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            pile.InternalExecuteAsync(element => action(element).AsValueTaskUnit(), canIgnore).AsValueTaskVoid();

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="TUIElement">UI element type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="pile">Pile instance</param>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <returns>Result value</returns>
        public static ValueTask<TResult> ExecuteAsync<TUIElement, TResult>(
            this Pile<TUIElement> pile,
            Func<TUIElement, Task<TResult>> action)
            where TUIElement : UIElement =>
            pile.InternalExecuteAsync(element => InternalHelpers.AsValueTask(action(element)));
    }
}
