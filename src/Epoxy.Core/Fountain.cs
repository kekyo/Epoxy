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
using System.Diagnostics;
using System.ComponentModel;

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
using System.Reactive;
using DependencyObject = Avalonia.IAvaloniaObject;
using UIElement = Avalonia.Interactivity.Interactive;
#endif

#if AVALONIA11
using Avalonia;
using Avalonia.Interactivity;
using Avalonia.Reactive;
using DependencyObject = Avalonia.AvaloniaObject;
using UIElement = Avalonia.Interactivity.Interactive;
#endif

namespace Epoxy;

/// <summary>
/// The Fountain give bindable ability for standard CLR events by Fountain-Well infrastructure.
/// </summary>
/// <remarks>See Fountain guide: https://github.com/kekyo/Epoxy#fountain</remarks>
/// <example>
/// <code>
/// &lt;Window xmlns:epoxy="https://github.com/kekyo/Epoxy"
///    epoxy:Fountain.Well={Binding WindowWell}&gt;
///    &lt;!-- ... --&gt;
/// &lt;/Window&gt;
/// </code>
/// </example>
public sealed class Fountain
{
    /// <summary>
    /// The constructor.
    /// </summary>
    private Fountain()
    { }

#if XAMARIN_FORMS || MAUI
    public static readonly BindableProperty WellProperty =
        BindableProperty.CreateAttached(
            "Well",
            typeof(Well),
            typeof(Fountain),
            null,
            BindingMode.OneWay,
            null,
            (b, o, n) =>
            {
                if (o is Well op)
                {
                    op.Release((UIElement)b);
                }
                if (n is Well np)
                {
                    np.Bind((UIElement)b);
                }
            });
#elif AVALONIA || AVALONIA11
    public static readonly AvaloniaProperty<Well?> WellProperty =
        AvaloniaProperty.RegisterAttached<Fountain, UIElement, Well?>("Well");

    /// <summary>
    /// The type initializer.
    /// </summary>
    static Fountain() =>
        WellProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<Well?>>(e =>
            {
                if (e.OldValue.GetValueOrDefault() is { } op)
                {
                    op.Release((UIElement)e.Sender);
                }
                if (e.NewValue.GetValueOrDefault() is { } np)
                {
                    np.Bind((UIElement)e.Sender);
                }
            }));
#else
    public static readonly DependencyProperty WellProperty =
        DependencyProperty.RegisterAttached(
            "Well",
            typeof(Well),
            typeof(Fountain),
            new PropertyMetadata(null, (d, e) =>
            {
                if (e.OldValue is Well op)
                {
                    op.Release((UIElement)d);
                }
                if (e.NewValue is Well np)
                {
                    np.Bind((UIElement)d);
                }
            }));
#endif

    /// <summary>
    /// Get Pile from this Anchor.
    /// </summary>
    public static Well? GetWell(DependencyObject d) =>
        (Well?)d.GetValue(WellProperty);

    /// <summary>
    /// Set Pile from this Anchor.
    /// </summary>
    public static void SetWell(DependencyObject d, Well? well) =>
        d.SetValue(WellProperty, well);

    /// <summary>
    /// Clear Pile from this Anchor.
    /// </summary>
    public static void ClearWell(DependencyObject d) =>
        d.ClearValue(WellProperty);
}
