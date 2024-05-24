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
#endif

#if MAUI
using Microsoft.Maui.Controls;
using DependencyObject = Microsoft.Maui.Controls.BindableObject;
#endif

#if AVALONIA
using Avalonia;
using DependencyObject = Avalonia.IAvaloniaObject;
#endif

#if AVALONIA11
using Avalonia;
using DependencyObject = Avalonia.AvaloniaObject;
#endif

namespace Epoxy;

/// <summary>
/// The Well base class is used with Fountain and Duct.
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
    /// <param name="d">Target Fountain element</param>
    internal abstract void Bind(DependencyObject d);

    /// <summary>
    /// Will release a Fountain.
    /// </summary>
    /// <param name="d">Target Fountain element</param>
    internal abstract void Release(DependencyObject d);

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
/// The Well is used with Fountation, there will bind and can transfer .NET event signal.
/// </summary>
/// <remarks>See Fountain guide: https://github.com/kekyo/Epoxy#fountain</remarks>
/// <example>
/// <code>
/// // Declared a Well into the ViewModel.
/// this.ReadyWell = Well.Factory.Create<Window>("Loaded", async () =&gt;
/// {
///     // Event received.
/// });
/// </code>
/// </example>
public sealed class Well<TDependencyObject> : Well
    where TDependencyObject : DependencyObject
{
    private readonly EventTrampoline<TDependencyObject, EventArgs> et;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="eventName">Event name</param>
    /// <param name="action">Action</param>
    internal Well(
        string eventName,
        Func<ValueTask> action) =>
        this.et = new DynamicEventTrampoline<TDependencyObject, EventArgs>(eventName, _ => action());

    internal Well(string eventName,
        Func<ValueTask> action,
        Action<TDependencyObject, object, IntPtr> adder,
        Action<TDependencyObject, object, IntPtr> remover) =>
        this.et = new StaticEventTrampoline<TDependencyObject, EventArgs>(eventName, _ => action(), adder, remover);

    /// <summary>
    /// Will bind a Fountain.
    /// </summary>
    /// <param name="d">Target Fountain element</param>
    internal override void Bind(DependencyObject d)
    {
        if (d is not TDependencyObject depo)
        {
            throw new InvalidOperationException($"Couldn't bind this Fountain: {d.GetType().FullName}.");
        }
        this.et.Bind(depo);
    }

    /// <summary>
    /// Will release a Fountain.
    /// </summary>
    /// <param name="d">Target Fountain element</param>
    internal override void Release(DependencyObject d)
    {
        if (d is TDependencyObject depo)
        {
            this.et.Release(depo);
        }
    }

    /// <summary>
    /// Generate formatted string of this instance.
    /// </summary>
    /// <returns>Formatted string</returns>
    public override string ToString() =>
        $"Well: {this.et}";
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
public sealed class Well<TDependencyObject, TEventArgs> : Well
    where TDependencyObject : DependencyObject
{
    private readonly EventTrampoline<TDependencyObject, TEventArgs> et;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="eventName">Event name</param>
    /// <param name="action">Action</param>
    internal Well(
        string eventName,
        Func<TEventArgs, ValueTask> action) =>
        this.et = new DynamicEventTrampoline<TDependencyObject, TEventArgs>(eventName, action);

    internal Well(string eventName,
        Func<TEventArgs, ValueTask> action,
        Action<TDependencyObject, object, IntPtr> adder,
        Action<TDependencyObject, object, IntPtr> remover) =>
        this.et = new StaticEventTrampoline<TDependencyObject, TEventArgs>(eventName, action, adder, remover);

    /// <summary>
    /// Will bind a Fountain.
    /// </summary>
    /// <param name="d">Target Fountain element</param>
    internal override void Bind(DependencyObject d)
    {
        if (d is not TDependencyObject depo)
        {
            throw new InvalidOperationException($"Couldn't bind this Fountain: {d.GetType().FullName}.");
        }
        this.et.Bind(depo);
    }

    /// <summary>
    /// Will release a Fountain.
    /// </summary>
    /// <param name="d">Target Fountain element</param>
    internal override void Release(DependencyObject d)
    {
        if (d is TDependencyObject depo)
        {
            this.et.Release(depo);
        }
    }

    /// <summary>
    /// Generate formatted string of this instance.
    /// </summary>
    /// <returns>Formatted string</returns>
    public override string ToString() =>
        $"Well: {this.et}";
}
