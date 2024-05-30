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
using System.ComponentModel;
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
using Avalonia.Interactivity;
using DependencyObject = Avalonia.IAvaloniaObject;
using UIElement = Avalonia.Interactivity.Interactive;
#endif

#if AVALONIA11
using Avalonia;
using Avalonia.Interactivity;
using DependencyObject = Avalonia.AvaloniaObject;
using UIElement = Avalonia.Interactivity.Interactive;
#endif

namespace Epoxy;

/// <summary>
/// The Well is used with Fountation, there will bind and can transfer .NET event signal.
/// </summary>
/// <remarks>See Fountain guide: https://github.com/kekyo/Epoxy#fountain</remarks>
/// <example>
/// <code>
/// // Declared a Well into the ViewModel.
/// public Well&lt;Window&gt; WindowWell = Well.Factory.Create&lt;Window&gt;();
/// 
/// // ...
/// 
/// public MainWindowViewModel()
/// {
///     this.WindowWell.Add(Window.Loaded, () =>
///     {
///         // Event received.
///     });
/// 
///     this.WindowWell.Add&lt;KeyEventArgs&gt;("KeyDown", e =>
///     {
///         // Event received.
///     });
/// });
/// </code>
/// </example>
[DebuggerStepThrough]
public static class WellExtension
{
    /// <summary>
    /// Create a Well.
    /// </summary>
    /// <typeparam name="TUIElement">Target control type</typeparam>
    /// <param name="factory">Factory instance (not used)</param>
    /// <returns>Well instance</returns>
    /// <example>
    /// <code>
    /// // Declared a Well into the ViewModel.
    /// public Well&lt;Window&gt; WindowWell = Well.Factory.Create&lt;Window&gt;();
    /// </code>
    /// </example>
    public static Well<TUIElement> Create<TUIElement>(
        this WellFactoryInstance factory)
        where TUIElement : UIElement =>
        new Well<TUIElement>();

    /////////////////////////////////////////////////////////////////

    /// <summary>
    /// Add a well handler.
    /// </summary>
    /// <typeparam name="TEventArgs">Handler receive type</typeparam>
    /// <param name="well">Well</param>
    /// <param name="eventName">Event name</param>
    /// <param name="action">Action delegate</param>
    /// <example>
    /// <code>
    /// public MainWindowViewModel()
    /// {
    ///     this.WindowWell.Add&lt;KeyEventArgs&gt;("KeyDown", e =>
    ///     {
    ///         // Event received.
    ///     });
    /// });
    /// </code>
    /// </example>
    public static void Add<TEventArgs>(
        this Well well,
        string eventName,
        Func<TEventArgs, ValueTask> action) =>
        well.InternalAdd(eventName, action);

    /// <summary>
    /// Add a well handler.
    /// </summary>
    /// <param name="well">Well</param>
    /// <param name="eventName">Event name</param>
    /// <param name="action">Action delegate</param>
    /// <example>
    /// <code>
    /// public MainWindowViewModel()
    /// {
    ///     this.WindowWell.Add("Loaded", () =>
    ///     {
    ///         // Event received.
    ///     });
    /// });
    /// </code>
    /// </example>
    public static void Add(
        this Well well,
        string eventName,
        Func<ValueTask> action) =>
        well.InternalAdd<EventArgs>(eventName, _ => action());

    /// <summary>
    /// Remove a well handler.
    /// </summary>
    /// <param name="well">Well</param>
    /// <param name="eventName">Event name</param>
    public static void Remove(
        this Well well,
        string eventName) =>
        well.InternalRemove(eventName);

#if !(XAMARIN_FORMS || MAUI)
    /// <summary>
    /// Add a well handler.
    /// </summary>
    /// <typeparam name="TEventArgs">Handler receive type</typeparam>
    /// <param name="well">Well</param>
    /// <param name="routedEvent">RoutedEvent</param>
    /// <param name="action">Action delegate</param>
    /// <example>
    /// <code>
    /// public MainWindowViewModel()
    /// {
    ///     this.WindowWell.Add(DragDrop.DragEnter, e =>
    ///     {
    ///         // Event received.
    ///     });
    /// });
    /// </code>
    /// </example>
    public static void Add<TEventArgs>(
        this Well well,
#if AVALONIA || AVALONIA11
        RoutedEvent<TEventArgs> routedEvent,
#else
        RoutedEvent routedEvent,
#endif
        Func<TEventArgs, ValueTask> action)
        where TEventArgs : RoutedEventArgs =>
        well.InternalAdd(routedEvent, action);

    /// <summary>
    /// Add a well handler.
    /// </summary>
    /// <param name="well">Well</param>
    /// <param name="routedEvent">RoutedEvent</param>
    /// <param name="action">Action delegate</param>
    /// <example>
    /// <code>
    /// public MainWindowViewModel()
    /// {
    ///     this.WindowWell.Add(Window.Loaded, () =>
    ///     {
    ///         // Event received.
    ///     });
    /// });
    /// </code>
    /// </example>
    public static void Add(
        this Well well,
        RoutedEvent routedEvent,
        Func<ValueTask> action) =>
        well.InternalAdd<RoutedEventArgs>(routedEvent, _ => action());

    /// <summary>
    /// Remove a well handler.
    /// </summary>
    /// <param name="well">Well</param>
    /// <param name="routedEvent">RoutedEvent</param>
    public static void Remove(
        this Well well,
        RoutedEvent routedEvent) =>
        well.InternalRemove(routedEvent);
#endif

    /////////////////////////////////////////////////////////////////

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Add<TUIElement, TEventArgs>(
        this Well<TUIElement> well,
        string eventName,
        Func<TEventArgs, ValueTask> action,
        Action<TUIElement, object, IntPtr> adder,
        Action<TUIElement, object, IntPtr> remover)
        where TUIElement : UIElement =>
        well.InternalAdd(eventName, action, adder, remover);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void Add<TUIElement>(
        this Well<TUIElement> well,
        string eventName,
        Func<ValueTask> action,
        Action<TUIElement, object, IntPtr> adder,
        Action<TUIElement, object, IntPtr> remover)
        where TUIElement : UIElement =>
        well.InternalAdd<EventArgs>(eventName, _ => action(), adder, remover);
}
