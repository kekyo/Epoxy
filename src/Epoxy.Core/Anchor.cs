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
using System.Runtime.ExceptionServices;
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
using System.Reactive;
using DependencyObject = Avalonia.IAvaloniaObject;
using UIElement = Avalonia.Interactivity.Interactive;
#endif

#if AVALONIA11
using Avalonia;
using Avalonia.Reactive;
using DependencyObject = Avalonia.AvaloniaObject;
using UIElement = Avalonia.Interactivity.Interactive;
#endif

namespace Epoxy;

/// <summary>
/// The Anchor is used with Pile, there will bind loosely and can rent control temporarily.
/// </summary>
/// <remarks>See Anchor/Pile guide: https://github.com/kekyo/Epoxy#anchorpile</remarks>
/// <example>
/// <code>
/// &lt;Window xmlns:epoxy="https://github.com/kekyo/Epoxy"&gt;
///    &lt;!-- ... --&gt;
///    &lt;!-- Placed Anchor onto the TextBox and bound property --&gt;
///    &lt;TextBox epoxy:Anchor.Pile="{Binding LogPile}" /&gt;
/// &lt;/Window&gt;
/// </code>
/// </example>
public sealed class Anchor
{
    /// <summary>
    /// The constructor.
    /// </summary>
    private Anchor()
    { }

#if XAMARIN_FORMS || MAUI
    public static readonly BindableProperty PileProperty =
        BindableProperty.CreateAttached(
            "Pile",
            typeof(Pile),
            typeof(Anchor),
            null,
            BindingMode.OneWay,
            null,
            (b, o, n) =>
            {
                if (o is Pile op)
                {
                    op.Release((UIElement)b);
                }
                if (n is Pile np)
                {
                    np.Bind((UIElement)b);
                }
            });
#elif AVALONIA || AVALONIA11
    public static readonly AvaloniaProperty<Pile?> PileProperty =
        AvaloniaProperty.RegisterAttached<Anchor, UIElement, Pile?>("Pile");

    /// <summary>
    /// The type initializer.
    /// </summary>
    static Anchor() =>
        PileProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<Pile?>>(e =>
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
    public static readonly DependencyProperty PileProperty =
        DependencyProperty.RegisterAttached(
            "Pile",
            typeof(Pile),
            typeof(Anchor),
            new PropertyMetadata(null, (d, e) =>
            {
                if (e.OldValue is Pile op)
                {
                    op.Release((UIElement)d);
                }
                if (e.NewValue is Pile np)
                {
                    np.Bind((UIElement)d);
                }
            }));
#endif

    /// <summary>
    /// Get Pile from this Anchor.
    /// </summary>
    public static Pile? GetPile(DependencyObject d) =>
        (Pile?)d.GetValue(PileProperty);

    /// <summary>
    /// Set Pile to this Anchor.
    /// </summary>
    public static void SetPile(DependencyObject d, Pile? pile) =>
        d.SetValue(PileProperty, pile);

    /// <summary>
    /// Clear Pile from this Anchor.
    /// </summary>
    public static void ClearPile(DependencyObject d) =>
        d.ClearValue(PileProperty);
}
