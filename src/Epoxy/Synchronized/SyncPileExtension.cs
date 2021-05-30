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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
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

namespace Epoxy.Synchronized
{
    /// <summary>
    /// Pile methods for synchronous execution.
    /// </summary>
    /// <remarks>You can manipulate XAML controls directly inside ViewModels
    /// when places and binds both an Anchor (in XAML) and a Pile.
    /// 
    /// Notice: They handle with synchronous handler.
    /// You can use asynchronous version instead.</remarks>
    [DebuggerStepThrough]
    public static class SyncPileExtension
    {
        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="TUIElement">UI element type</typeparam>
        /// <param name="pile">Pile instance</param>
        /// <param name="action">Synchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <remarks>Notice: It handles with synchronous handler. You can use asynchronous version instead.</remarks>
        public static void RentSync<TUIElement>(
            this Pile<TUIElement> pile,
            Action<TUIElement> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            pile.InternalRentSync(action, canIgnore);

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="TUIElement">UI element type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="pile">Pile instance</param>
        /// <param name="action">Synchronous continuation delegate</param>
        /// <returns>Result value</returns>
        /// <remarks>Notice: It handles with synchronous handler. You can use asynchronous version instead.</remarks>
        public static TResult RentSync<TUIElement, TResult>(
            this Pile<TUIElement> pile,
            Func<TUIElement, TResult> action)
            where TUIElement : UIElement =>
            pile.InternalRentSync(action);

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="TUIElement">UI element type</typeparam>
        /// <param name="pile">Pile instance</param>
        /// <param name="action">Synchronous continuation delegate</param>
        /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
        /// <remarks>Notice: It handles with synchronous handler. You can use asynchronous version instead.</remarks>
        [Obsolete("Use RentSync instead.")]
        public static void ExecuteSync<TUIElement>(
            this Pile<TUIElement> pile,
            Action<TUIElement> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            pile.InternalRentSync(action, canIgnore);

        /// <summary>
        /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
        /// </summary>
        /// <typeparam name="TUIElement">UI element type</typeparam>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="pile">Pile instance</param>
        /// <param name="action">Synchronous continuation delegate</param>
        /// <returns>Result value</returns>
        /// <remarks>Notice: It handles with synchronous handler. You can use asynchronous version instead.</remarks>
        [Obsolete("Use RentSync instead.")]
        public static TResult ExecuteSync<TUIElement, TResult>(
            this Pile<TUIElement> pile,
            Func<TUIElement, TResult> action)
            where TUIElement : UIElement =>
            pile.InternalRentSync(action);

        #region Dodge mistake choicing asynchronously overloads
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use RentAsync instead.", true)]
        public static void ExecuteSync<TUIElement>(
            this Pile<TUIElement> pile,
            Func<TUIElement, ValueTask> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            throw new InvalidOperationException("Use RentAsync instead.");

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use RentAsync instead.", true)]
        public static TResult ExecuteSync<TUIElement, TResult>(
            this Pile<TUIElement> pile,
            Func<TUIElement, ValueTask<TResult>> action)
            where TUIElement : UIElement =>
            throw new InvalidOperationException("Use RentAsync instead.");
        #endregion
    }
}
