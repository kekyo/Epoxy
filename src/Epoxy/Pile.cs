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

using Epoxy.Internal;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

#if WINDOWS_UWP || UNO
using Windows.UI.Xaml;
#endif

#if WINUI
using Microsoft.UI.Xaml;
#endif

#if WINDOWS_WPF || OPENSILVER
using System.Windows;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
using DependencyObject = Xamarin.Forms.BindableObject;
using UIElement = Xamarin.Forms.VisualElement;
#endif

#if MAUI
using Microsoft.Maui.Controls;
using DependencyObject = Microsoft.Maui.Controls.BindableObject;
using UIElement = Microsoft.Maui.Controls.VisualElement;
#endif

#if AVALONIA
using Avalonia;
using DependencyObject = Avalonia.IAvaloniaObject;
using UIElement = Avalonia.Interactivity.Interactive;
#endif

#if AVALONIA11
using Avalonia;
using DependencyObject = Avalonia.AvaloniaObject;
using UIElement = Avalonia.Interactivity.Interactive;
#endif

namespace Epoxy;

/// <summary>
/// The Pile factory.
/// </summary>
/// <remarks>You can manipulate XAML controls directly inside ViewModels
/// when places and binds both an Anchor (in XAML) and a Pile.</remarks>
[DebuggerStepThrough]
public static class PileFactoryExtension
{
    /// <summary>
    /// Create an anonymous control typed Pile.
    /// </summary>
    /// <returns>Pile instance</returns>
    public static Pile<UIElement> Create(
        this PileFactoryInstance factory) =>
        new Pile<UIElement>();

    /// <summary>
    /// Create a Pile.
    /// </summary>
    /// <typeparam name="TUIElement">Target control type</typeparam>
    /// <returns>Pile instance</returns>
    public static Pile<TUIElement> Create<TUIElement>(
        this PileFactoryInstance factory)
        where TUIElement : UIElement =>
        new Pile<TUIElement>();
}

/// <summary>
/// Pile methods for ValueTask based asynchronous execution.
/// </summary>
/// <remarks>You can manipulate XAML controls directly inside ViewModels
/// when places and binds both an Anchor (in XAML) and a Pile.</remarks>
[DebuggerStepThrough]
public static class PileExtension
{
    /// <summary>
    /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
    /// </summary>
    /// <typeparam name="TUIElement">Target control type</typeparam>
    /// <param name="pile">Pile instance</param>
    /// <param name="action">Asynchronous continuation delegate</param>
    /// <param name="canIgnore">Ignore if didn't complete XAML data-binding.</param>
    /// <returns>ValueTask instance</returns>
    public static ValueTask RentAsync<TUIElement>(
        this Pile<TUIElement> pile, Func<TUIElement, ValueTask> action, bool canIgnore = false)
        where TUIElement : UIElement =>
        pile.InternalRentAsync(e => action(e).AsValueTaskUnit(), canIgnore).AsValueTaskVoid();

    /// <summary>
    /// Temporary rents and manipulates XAML control directly via Anchor/Pile.
    /// </summary>
    /// <typeparam name="TUIElement">Target control type</typeparam>
    /// <typeparam name="TResult">Action result type</typeparam>
    /// <param name="pile">Pile instance</param>
    /// <param name="action">Predicts when rents control instance</param>
    /// <returns>Result for action</returns>
    /// <remarks>This overload has to complete XAML data-binding.</remarks>
    public static ValueTask<TResult> RentAsync<TUIElement, TResult>(
        this Pile<TUIElement> pile, Func<TUIElement, ValueTask<TResult>> action)
        where TUIElement : UIElement =>
        pile.InternalRentAsync<TResult>(action);
}
