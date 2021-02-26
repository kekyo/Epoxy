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
using UIElement = Xamarin.Forms.Element;
#endif

#if AVALONIA
using UIElement = Avalonia.IStyledElement;
#endif

namespace Epoxy.Synchronized
{
    public static class PileExtension
    {
        [DebuggerStepThrough]
        public static void ExecuteSync<TUIElement>(
            this Pile<TUIElement> pile,
            Action<TUIElement> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            pile.InternalExecuteAsync(element => { action(element); return default; }, canIgnore);

        public static TResult ExecuteSync<TUIElement, TResult>(
            this Pile<TUIElement> pile,
            Func<TUIElement, TResult> action)
            where TUIElement : UIElement
        {
            var result = pile.InternalExecuteAsync(element =>
            {
                try
                {
                    return InternalHelpers.FromResult(InternalHelpers.Pair(action(element), default(ExceptionDispatchInfo)!));
                }
                catch (Exception ex)
                {
                    return InternalHelpers.FromResult(InternalHelpers.Pair(default(TResult)!, ExceptionDispatchInfo.Capture(ex)));
                }
            }).Result;  // Will not block

            result.Value?.Throw();
            return result.Key;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use ExecuteAsync instead.", true)]
        public static void ExecuteSync<TUIElement>(
            this Pile<TUIElement> pile,
            Func<TUIElement, ValueTask> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            throw new InvalidOperationException("Use ExecuteAsync instead.");

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use ExecuteAsync instead.", true)]
        public static TResult ExecuteSync<TUIElement, TResult>(
            this Pile<TUIElement> pile,
            Func<TUIElement, ValueTask<TResult>> action)
            where TUIElement : UIElement =>
            throw new InvalidOperationException("Use ExecuteAsync instead.");
    }
}
