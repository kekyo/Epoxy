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
using System.Collections.Generic;
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
/// The Well is used with Fountation, there will bind loosely and can transfer event signal.
/// </summary>
/// <remarks>You can use with generic Well&lt;T&gt; class.</remarks>
public abstract class Well
{
    /// <summary>
    /// The constructor.
    /// </summary>
    private protected Well()
    { }

    /// <summary>
    /// Will bind a Fountain.
    /// </summary>
    /// <param name="e">Target Fountain element</param>
    internal abstract void Bind(UIElement e);

    /// <summary>
    /// Will release a Fountain.
    /// </summary>
    /// <param name="e">Target Fountain element</param>
    internal abstract void Release(UIElement e);

    internal abstract void InternalAdd<TEventArgs>(
        string eventName,
        Func<TEventArgs, ValueTask> action);

    internal abstract void InternalRemove(
        string eventName);

#if !(XAMARIN_FORMS || MAUI)
    internal abstract void InternalAdd<TEventArgs>(
        RoutedEvent routedEvent,
        Func<TEventArgs, ValueTask> action)
        where TEventArgs : RoutedEventArgs;

    internal abstract void InternalRemove(
        RoutedEvent routedEvent);
#endif

    public static readonly WellFactoryInstance Factory =
        WellFactoryInstance.Instance;
}

[DebuggerStepThrough]
public sealed class WellFactoryInstance
{
    private WellFactoryInstance()
    {
    }

    internal static readonly WellFactoryInstance Instance =
        new WellFactoryInstance();
}

/// <summary>
/// The Well is used with Fountation, there will bind loosely and can transfer event signal.
/// </summary>
/// <remarks>See Fountain guide: https://github.com/kekyo/Epoxy#fountain</remarks>
/// <example>
/// <code>
/// // Declared a Well into the ViewModel.
/// this.KeyDownWell = Well.Factory.Create<Panel, KeyEventArgs>("KeyDown", async e =&gt;
/// {
///     // Event received.
/// });
/// </code>
/// </example>
public sealed class Well<TUIElement> : Well
    where TUIElement : UIElement
{
    // With one or more events hooked, the WeakReference should always hold a reference.
    // Here, WeakReference is used to consider the state where no events are hooked at all.

    private readonly Dictionary<string, EventTrampoline<TUIElement>> ets = new();
    private readonly WeakReference element =
        new WeakReference(null);

    /// <summary>
    /// The constructor.
    /// </summary>
    internal Well()
    {
    }

    /// <summary>
    /// Will bind a Fountain.
    /// </summary>
    /// <param name="element">Target Fountain element</param>
    internal override void Bind(UIElement element)
    {
        Debug.Assert(!this.element.IsAlive);

        if (element is not TUIElement e)
        {
            throw new InvalidOperationException($"Couldn't bind this Fountain: {element.GetType().FullName}.");
        }

        this.element.Target = e;

        foreach (var et in this.ets.Values)
        {
            et.Bind(e);
        }
    }

    /// <summary>
    /// Will release a Fountain.
    /// </summary>
    /// <param name="element">Target Fountain element</param>
    internal override void Release(UIElement element)
    {
        Debug.Assert(this.element.Target is TUIElement e && object.ReferenceEquals(element, e));

        foreach (var et in this.ets.Values)
        {
            et.Release((TUIElement)element);
        }

        this.element.Target = null;
    }

    private void InternalAdd(
        EventTrampoline<TUIElement> et)
    {
        if (this.ets.TryGetValue(et.EventName, out var oet))
        {
            if (this.element.Target is TUIElement e)
            {
                oet.Release(e);
            }
        }

        this.ets[et.EventName] = et;

        if (this.element.Target is TUIElement e2)
        {
            et.Bind(e2);
        }
    }

    /// <summary>
    /// Add a handler with dynamic event trampoline.
    /// </summary>
    /// <param name="eventName">Event name</param>
    /// <param name="action">Action delegate</param>
    internal override void InternalAdd<TEventArgs>(
        string eventName,
        Func<TEventArgs, ValueTask> action)
    {
        var et = new DynamicEventTrampoline<TUIElement, TEventArgs>(
            eventName, action);
        this.InternalAdd(et);
    }

    /// <summary>
    /// Add a handler with static event trampoline.
    /// </summary>
    /// <param name="eventName">Event name</param>
    /// <param name="action">Action delegate</param>
    /// <param name="adder">Handler adder</param>
    /// <param name="remover">Handler remover</param>
    internal void InternalAdd<TEventArgs>(
        string eventName,
        Func<TEventArgs, ValueTask> action,
        Action<TUIElement, object, IntPtr> adder,
        Action<TUIElement, object, IntPtr> remover)
    {
        var et = new StaticEventTrampoline<TUIElement, TEventArgs>(
            eventName, action, adder, remover);
        this.InternalAdd(et);
    }

    /// <summary>
    /// Remove a handler with event name.
    /// </summary>
    /// <param name="eventName">Event name</param>
    internal override void InternalRemove(
        string eventName)
    {
        if (this.ets.TryGetValue(eventName, out var oet))
        {
            if (this.element.Target is TUIElement e)
            {
                oet.Release(e);
            }

            this.ets.Remove(eventName);
        }
    }

#if !(XAMARIN_FORMS || MAUI)
    /// <summary>
    /// Add a handler indicates RoutedEvent.
    /// </summary>
    /// <param name="routedEvent">RoutedEvent</param>
    /// <param name="action">Action delegate</param>
    internal override void InternalAdd<TEventArgs>(
        RoutedEvent routedEvent,
        Func<TEventArgs, ValueTask> action)
    {
        var et = new RoutedEventTrampoline<TUIElement, TEventArgs>(
            routedEvent, action);
        this.InternalAdd(et);
    }

    /// <summary>
    /// Remove a handler indicates RoutedEvent.
    /// </summary>
    /// <param name="routedEvent">RoutedEvent</param>
    internal override void InternalRemove(
        RoutedEvent routedEvent)
    {
        var eventName = routedEvent.GetFullName();

        if (this.ets.TryGetValue(eventName, out var oet))
        {
            if (this.element.Target is TUIElement e)
            {
                oet.Release(e);
            }

            this.ets.Remove(eventName);
        }
    }
#endif

    /// <summary>
    /// Generate formatted string of this instance.
    /// </summary>
    /// <returns>Formatted string</returns>
    public override string ToString() =>
        $"Well: Ducts={this.ets.Count}";
}
