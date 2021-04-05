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

using Epoxy.Internal;

using System;
using System.Collections.Generic;
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
using UIElement = Xamarin.Forms.VisualElement;
#endif

#if AVALONIA
using UIElement = Avalonia.Controls.IControl;
#endif

namespace Epoxy
{
    /// <summary>
    /// The ChildrenPile factory.
    /// </summary>
    /// <remarks>You can manipulate XAML controls directly inside ViewModels
    /// when places and binds both a ChildrenAnchor (in XAML) and a ChildrenPile.</remarks>
    [DebuggerStepThrough]
    public static class ChildrenPileFactory
    {
        /// <summary>
        /// Create an anonymous child control typed ChildrenPile.
        /// </summary>
        /// <returns>ChildrenPile instance</returns>
        public static ChildrenPile<UIElement> Create() =>
            new ChildrenPile<UIElement>();

        /// <summary>
        /// Create a ChildrenPile.
        /// </summary>
        /// <typeparam name="TUIElement">Target child control type</typeparam>
        /// <returns>ChildrenPile instance</returns>
        public static ChildrenPile<TUIElement> Create<TUIElement>()
            where TUIElement : UIElement =>
            new ChildrenPile<TUIElement>();
    }

    /// <summary>
    /// ChildrenPile manipulator class.
    /// </summary>
    /// <remarks>You can manipulate XAML controls directly inside ViewModels
    /// when places and binds both an Anchor (in XAML) and a ChildrenPile.</remarks>
    [DebuggerStepThrough]
    public static class ChildrenPileExtension
    {
        /// <summary>
        /// Manipulate anchoring element.
        /// </summary>
        /// <typeparam name="TUIElement">Target child control type</typeparam>
        /// <param name="pile">ChildrenPile instance</param>
        /// <param name="action">Asynchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <returns>ValueTask instance</returns>
        public static ValueTask ManipulateAsync<TUIElement>(
            this ChildrenPile<TUIElement> pile, Func<IList<TUIElement>, ValueTask> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            pile.InternalManipulateAsync(c => action(c).AsValueTaskUnit(), canIgnore).AsValueTaskVoid();

        /// <summary>
        /// Manipulate anchoring element.
        /// </summary>
        /// <typeparam name="TUIElement">Target child control type</typeparam>
        /// <typeparam name="TResult">Action result type</typeparam>
        /// <param name="pile">ChildrenPile instance</param>
        /// <param name="action">Predicts when rents control instance</param>
        /// <returns>Result for action</returns>
        /// <remarks>This overload has to complete XAML data-binding.</remarks>
        public static ValueTask<TResult> ManipulateAsync<TUIElement, TResult>(
            this ChildrenPile<TUIElement> pile, Func<IList<TUIElement>, ValueTask<TResult>> action)
            where TUIElement : UIElement =>
            pile.InternalManipulateAsync<TResult>(action);
    }
}
