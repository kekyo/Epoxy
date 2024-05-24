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
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

#if WINDOWS_UWP || UNO
using Windows.UI.Xaml;
#endif

#if WINUI
using Microsoft.UI.Xaml;
#endif

#if WINDOWS_WPF || OPENSILVER
using System.Windows;
using System.Windows.Media.Animation;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
using DependencyObject = Xamarin.Forms.BindableObject;
#endif

#if MAUI
using Microsoft.Maui.Controls;
using DependencyObject = Microsoft.Maui.Controls.BindableObject;
#endif

#if AVALONIA
using Avalonia;
using Avalonia.Data;
using System.Reactive;
using DependencyObject = Avalonia.IAvaloniaObject;
#endif

#if AVALONIA11
using Avalonia;
using Avalonia.Data;
using Avalonia.Reactive;
using DependencyObject = Avalonia.AvaloniaObject;
#endif

using Epoxy.Internal;
using Epoxy.Advanced;

namespace Epoxy;

/// <summary>
/// The Fountain give bindable ability for standard CLR events by Fountain-Well infrastructure.
/// </summary>
/// <remarks>See Fountain guide: https://github.com/kekyo/Epoxy#fountain</remarks>
/// <example>
/// <code>
/// &lt;Window xmlns:epoxy="https://github.com/kekyo/Epoxy"&gt;
///    &lt;!-- ... --&gt;
///    &lt;epoxy:Fountain.Ducts&gt;
///         &lt;!-- Binding the ViewModel's Well property --&gt;
///         &lt;epoxy:Duct Well="{Binding ReadyWell}" /&gt;
///    &lt;/epoxy:Fountain.Ducts&gt;
/// &lt;/Window&gt;
/// </code>
/// </example>
#if AVALONIA || AVALONIA11
public sealed class Fountain
#else
public static class Fountain
#endif
{
#if XAMARIN_FORMS || MAUI
    private static readonly BindablePropertyKey DuctsPropertyKey =
        BindableProperty.CreateAttachedReadOnly(
            "Ducts",
            typeof(DuctsCollection),
            typeof(Fountain),
            null,
            BindingMode.OneTime,
            null,
            null,
            null,
            null,
            d =>
            {
                var collection = new DuctsCollection();
                collection.Attach(d);
                return collection;
            });

    /// <summary>
    /// Declared Ducts attached property.
    /// </summary>
    public static readonly BindableProperty DuctsProperty =
        DuctsPropertyKey.BindableProperty;

    /// <summary>
    /// Get DuctsCollection instance.
    /// </summary>
    public static DuctsCollection? GetDucts(DependencyObject d) =>
        (DuctsCollection?)d.GetValue(DuctsProperty);
#elif AVALONIA || AVALONIA11
    /// <summary>
    /// The constructor.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    private Fountain()
    { }

    /// <summary>
    /// Declared Ducts attached property.
    /// </summary>
    public static readonly AttachedProperty<DuctsCollection?> DuctsProperty =
        AvaloniaProperty.RegisterAttached<Fountain, AvaloniaObject, DuctsCollection?>(
            "Ducts",
            default,
            false,
            BindingMode.OneTime);

    /// <summary>
    /// The type initializer.
    /// </summary>
    static Fountain() =>
        DuctsProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<DuctsCollection?>>(e =>
        {
            if (!e.OldValue.Equals(e.NewValue))
            {
                if (e.OldValue.GetValueOrDefault() is DuctsCollection oec)
                {
                    oec.Detach();
                }
                if (e.NewValue.GetValueOrDefault() is DuctsCollection nec)
                {
                    nec.Attach(e.Sender);
                }
            }
        }));

    /// <summary>
    /// Get DuctsCollection instance.
    /// </summary>
    public static DuctsCollection? GetDucts(DependencyObject d)
    {
        var collection = d.GetValue(DuctsProperty);
        if (collection == null)
        {
            // Self generated.
            collection = new DuctsCollection();
            d.SetValue(DuctsProperty, collection);
        }
        return collection;
    }

    /// <summary>
    /// Set DuctsCollection instance.
    /// </summary>
    /// <remarks>It's used internal only.
    /// It's required for Avalonia XAML compiler.</remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetDucts(DependencyObject d, DuctsCollection? value) =>
        d.SetValue(DuctsProperty, value ?? new DuctsCollection());
#else
    /// <summary>
    /// Declared Ducts attached property.
    /// </summary>
    public static readonly DependencyProperty DuctsProperty =
        DependencyProperty.RegisterAttached(
#if UNO || OPENSILVER
            "Ducts",
#else
            "ShadowDucts",
#endif
            typeof(DuctsCollection),
            typeof(Fountain),
            new PropertyMetadata(null, (d, e) =>
            {
                if (!object.ReferenceEquals(e.OldValue, e.NewValue))
                {
                    if (e.OldValue is DuctsCollection oec)
                    {
                        oec.Detach();
                    }
                    if (e.NewValue is DuctsCollection nec)
                    {
                        nec.Attach(d);
                    }
                }
            }));

    /// <summary>
    /// Get DuctsCollection instance.
    /// </summary>
    public static DuctsCollection? GetDucts(DependencyObject d)
    {
        var collection = (DuctsCollection?)d.GetValue(DuctsProperty);
        if (collection == null)
        {
            // Self generated.
            collection = new DuctsCollection();
            d.SetValue(DuctsProperty, collection);
        }
        return collection;
    }
#endif
}

/// <summary>
/// Duct declaration holding collection class.
/// </summary>
/// <remarks>It will be implicitly used on the XAML code.
/// 
/// See Fountain guide: https://github.com/kekyo/Epoxy#fountain</remarks>
#if WINDOWS_UWP || UNO
[Windows.UI.Xaml.Data.Bindable]
#endif
public sealed class DuctsCollection :
    AttachableCollection<DuctsCollection, Duct>
{
    /// <summary>
    /// The constructor.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public DuctsCollection()
    { }
}

/// <summary>
/// Fountain's duct declaration class.
/// </summary>
/// <remarks>See Fountain guide: https://github.com/kekyo/Epoxy#fountain</remarks>
/// <example>
/// <code>
/// &lt;Window xmlns:epoxy="https://github.com/kekyo/Epoxy"&gt;
///    &lt;!-- ... --&gt;
///    &lt;epoxy:Fountain.Ducts&gt;
///         &lt;!-- Binding the ViewModel's Well property --&gt;
///         &lt;epoxy:Duct Well="{Binding ReadyWell}" /&gt;
///    &lt;/epoxy:Fountain.Ducts&gt;
/// &lt;/Window&gt;
/// </code>
/// </example>
#if WINDOWS_UWP || UNO
[Windows.UI.Xaml.Data.Bindable]
#endif
public sealed class Duct :
    AttachedObject<Duct>
{
#if XAMARIN_FORMS || MAUI
    /// <summary>
    /// Binds Well expression bindable property declaration.
    /// </summary>
    public static readonly BindableProperty WellProperty =
        BindableProperty.Create(
            "Well",
            typeof(Well),
            typeof(Duct),
            null,
            BindingMode.Default,
            null,
            (d, _, nv) => ((Duct)d).OnWellPropertyChanged(nv));
#elif AVALONIA || AVALONIA11
    /// <summary>
    /// Binds ICommand expression bindable property declaration.
    /// </summary>
    public static readonly AvaloniaProperty<Well> WellProperty =
        AvaloniaProperty.Register<Duct, Well>("Well");

    /// <summary>
    /// The type initializer.
    /// </summary>
    static Duct()
    {
        WellProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<Well>>(e =>
                ((Duct)e.Sender).OnWellPropertyChanged(
                    e.NewValue.HasValue ? e.NewValue.Value : null)));
    }
#else
    /// <summary>
    /// Binds Well expression bindable property declaration.
    /// </summary>
    public static readonly DependencyProperty WellProperty =
        DependencyProperty.Register(
            "Well",
            typeof(Well),
            typeof(Duct),
            new PropertyMetadata(null, (d, e) => ((Duct)d).OnWellPropertyChanged(e.NewValue)));
#endif

    /// <summary>
    /// The constructor.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Duct()
    { }

    /// <summary>
    /// Binds Well expression.
    /// </summary>
    public Well? Well
    {
        get => (Well?)this.GetValue(WellProperty);
        set => this.SetValue(WellProperty, value);
    }

    private Well? lastWell;

    private void Unbind(object? associatedObject)
    {
        if (associatedObject is DependencyObject d &&
            this.lastWell is { } lastWell)
        {
            this.lastWell = null;
            lastWell.Release(d);
        }
    }

    private void Bind(object? associatedObject, object? newWell)
    {
        Debug.Assert(this.lastWell == null);

        if (associatedObject is DependencyObject ao &&
            newWell is Well w)
        {
            w.Bind(ao);
            this.lastWell = w;
        }
    }

    private void OnWellPropertyChanged(object? newWell)
    {
        this.Unbind(this.AssociatedObject);
        this.Bind(this.AssociatedObject, newWell);
    }

    /// <summary>
    /// Attach a parent element.
    /// </summary>
    protected override void OnAttached()
    {
        this.Unbind(this.AssociatedObject);
        this.Bind(this.AssociatedObject, this.Well);
    }

    /// <summary>
    /// Detach already attached parent element.
    /// </summary>
    protected override void OnDetaching()
    {
        this.Unbind(this.AssociatedObject);
    }
}
