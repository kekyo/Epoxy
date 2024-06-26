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
/// The Pile base class is used with Anchor.
/// </summary>
/// <remarks>You can use with generic Pile&lt;T&gt; class.</remarks>
public abstract class Pile
{
    /// <summary>
    /// The constructor.
    /// </summary>
    private protected Pile()
    { }

    /// <summary>
    /// Will bind an Anchor.
    /// </summary>
    /// <param name="element">Target anchoring element</param>
    internal abstract void Bind(UIElement element);

    /// <summary>
    /// Will release an Anchor.
    /// </summary>
    /// <param name="element">Target anchoring element</param>
    internal abstract void Release(UIElement element);

    public static readonly PileFactoryInstance Factory =
        PileFactoryInstance.Instance;
}

[DebuggerStepThrough]
public sealed class PileFactoryInstance
{
    private PileFactoryInstance()
    {
    }

    internal static readonly PileFactoryInstance Instance =
        new PileFactoryInstance();
}

/// <summary>
/// The Pile is used with Anchor, there will bind loosely and can rent control temporarily.
/// </summary>
/// <remarks>See Anchor/Pile guide: https://github.com/kekyo/Epoxy#anchorpile</remarks>
/// <example>
/// <code>
/// // Declared a Pile into the ViewModel.
/// this.LogPile = Pile.Create&lt;TextBox&gt;();
///
/// // ...
///
/// // Do rent by Pile when we have to manipulate the TextBox directly:
/// await this.LogPile.ExecuteAsync(async textBox =>
/// {
///    // Fetch information from related model.
///    var result = await ServerAccessor.GetResultTextAsync();
///    // We can manipulate safer directly TextBox.
///    textBox.AppendText(result);
/// });
/// </code>
/// </example>
public sealed class Pile<TUIElement> : Pile
    where TUIElement : UIElement
{
    private readonly WeakReference element =
        new WeakReference(null);

    /// <summary>
    /// The constructor.
    /// </summary>
    internal Pile()
    { }

    /// <summary>
    /// Will bind an Anchor.
    /// </summary>
    /// <param name="element">Target anchoring element</param>
    internal override void Bind(UIElement element)
    {
        Debug.Assert(!this.element.IsAlive);

        if (element is not TUIElement e)
        {
            throw new InvalidOperationException($"Couldn't bind this anchor: {element.GetType().FullName}.");
        }

        this.element.Target = e;
    }

    /// <summary>
    /// Will release an Anchor.
    /// </summary>
    /// <param name="element">Target anchoring element</param>
    internal override void Release(UIElement element) =>
        this.element.Target = null;

    /// <summary>
    /// Execute with anchoring element.
    /// </summary>
    /// <param name="action">Continuation delegate</param>
    /// <param name="canIgnore">Ignore if not present mooring Anchor</param>
    /// <returns>ValueTask</returns>
    /// <remarks>This method is used internal only.</remarks>
    internal ValueTask<Unit> InternalRentAsync(Func<TUIElement, ValueTask<Unit>> action, bool canIgnore = false)
    {
        if (this.element.Target is TUIElement element)
        {
            return action(element);
        }
        else if (!canIgnore)
        {
            throw new InvalidOperationException("You should bind an anchor before renting from the Pile.");
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Execute with anchoring element.
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="action">Continuation delegate</param>
    /// <returns>ValueTask&lt;TResult&gt;</returns>
    /// <remarks>This method is used internal only.</remarks>
    internal ValueTask<TResult> InternalRentAsync<TResult>(Func<TUIElement, ValueTask<TResult>> action)
    {
        if (this.element.Target is TUIElement element)
        {
            return action(element);
        }
        else
        {
            throw new InvalidOperationException("You should bind an anchor before renting from the Pile.");
        }
    }

    /// <summary>
    /// Synchronous execute with anchoring element.
    /// </summary>
    /// <param name="action">Continuation delegate</param>
    /// <param name="canIgnore">Ignore if not present mooring Anchor</param>
    /// <remarks>This method is used internal only.</remarks>
    [DebuggerStepThrough]
    internal void InternalRentSync(Action<TUIElement> action, bool canIgnore) =>
        this.InternalRentAsync(element => { action(element); return default; }, canIgnore);

    /// <summary>
    /// Synchronous execute with anchoring element.
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="action">Continuation delegate</param>
    /// <returns>TResult</returns>
    /// <remarks>This method is used internal only.</remarks>
    internal TResult InternalRentSync<TResult>(Func<TUIElement, TResult> action)
    {
        var result = this.InternalRentAsync(element =>
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

    /// <summary>
    /// Generate formatted string of this instance.
    /// </summary>
    /// <returns>Formatted string</returns>
    public override string ToString() =>
        this.element.Target is TUIElement element ?
            $"Bound: {element.GetType().FullName}" :
            "Released";
}
