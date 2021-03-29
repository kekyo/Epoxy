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
using Xamarin.Forms;
using DependencyObject = Xamarin.Forms.BindableObject;
using UIElement = Xamarin.Forms.Element;
#endif

#if AVALONIA
using Avalonia;
using DependencyObject = Avalonia.IAvaloniaObject;
using UIElement = Avalonia.IStyledElement;
#endif

namespace Epoxy
{
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

#if XAMARIN_FORMS
        private static readonly BindableProperty PileProperty =
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
                        np.Moore((UIElement)b);
                    }
                });
#elif AVALONIA
        private static readonly AvaloniaProperty<Pile?> PileProperty =
            AvaloniaProperty.RegisterAttached<Anchor, UIElement, Pile?>("Pile");

        /// <summary>
        /// The type initializer.
        /// </summary>
        static Anchor() =>
            PileProperty.Changed.Subscribe(e =>
            {
                if (e.OldValue.GetValueOrDefault() is { } op)
                {
                    op.Release((UIElement)e.Sender);
                }
                if (e.NewValue.GetValueOrDefault() is { } np)
                {
                    np.Moore((UIElement)e.Sender);
                }
            });
#else
        private static readonly DependencyProperty PileProperty =
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
                        np.Moore((UIElement)d);
                    }
                }));
#endif

        /// <summary>
        /// Get Pile from this Anchor.
        /// </summary>
        public static Pile? GetPile(DependencyObject d) =>
            (Pile?)d.GetValue(PileProperty);

        /// <summary>
        /// Set Pile from this Anchor.
        /// </summary>
        public static void SetPile(DependencyObject d, Pile? pile) =>
            d.SetValue(PileProperty, pile);

        /// <summary>
        /// Clear Pile from this Anchor.
        /// </summary>
        public static void ClearPile(DependencyObject d) =>
            d.ClearValue(PileProperty);
    }

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
        internal abstract void Moore(UIElement element);

        /// <summary>
        /// Will release an Anchor.
        /// </summary>
        /// <param name="element">Target anchoring element</param>
        internal abstract void Release(UIElement element);

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Pile.Create is obsoleted. Use PileFactory.Create instead.", true)]
        public static Pile<UIElement> Create() =>
            throw new InvalidOperationException("Pile.Create is obsoleted. Use PileFactory.Create instead.");

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Pile.Create is obsoleted. Use PileFactory.Create instead.", true)]
        public static Pile<TUIElement> Create<TUIElement>()
            where TUIElement : UIElement =>
            throw new InvalidOperationException("Pile.Create is obsoleted. Use PileFactory.Create instead.");
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
        internal override void Moore(UIElement element)
        {
            Debug.Assert(element is TUIElement);
            this.element.Target = (TUIElement)element;
        }

        /// <summary>
        /// Will release an Anchor.
        /// </summary>
        /// <param name="element">Target anchoring element</param>
        internal override void Release(UIElement element)
        {
            Debug.Assert(this.element.Target is TUIElement e && object.ReferenceEquals(e, element));
            this.element.Target = null;
        }

        /// <summary>
        /// Execute with anchoring element.
        /// </summary>
        /// <param name="action">Continuation delegate</param>
        /// <param name="canIgnore">Ignore if not present mooring Anchor</param>
        /// <returns>ValueTask</returns>
        /// <remarks>This method is used internal only.</remarks>
        internal ValueTask<Unit> InternalExecuteAsync(Func<TUIElement, ValueTask<Unit>> action, bool canIgnore = false)
        {
            if (this.element.Target is TUIElement element)
            {
                return action(element);
            }
            else if (!canIgnore)
            {
                throw new InvalidOperationException("Didn't moore a UIElement.");
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
        internal ValueTask<TResult> InternalExecuteAsync<TResult>(Func<TUIElement, ValueTask<TResult>> action)
        {
            if (this.element.Target is TUIElement element)
            {
                return action(element);
            }
            else
            {
                throw new InvalidOperationException("Didn't moore a UIElement.");
            }
        }

        /// <summary>
        /// Synchronous execute with anchoring element.
        /// </summary>
        /// <param name="action">Continuation delegate</param>
        /// <param name="canIgnore">Ignore if not present mooring Anchor</param>
        /// <remarks>This method is used internal only.</remarks>
        [DebuggerStepThrough]
        internal void InternalExecuteSync(Action<TUIElement> action, bool canIgnore) =>
            this.InternalExecuteAsync(element => { action(element); return default; }, canIgnore);

        /// <summary>
        /// Synchronous execute with anchoring element.
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="action">Continuation delegate</param>
        /// <returns>TResult</returns>
        /// <remarks>This method is used internal only.</remarks>
        internal TResult InternalExecuteSync<TResult>(Func<TUIElement, TResult> action)
        {
            var result = this.InternalExecuteAsync(element =>
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
                $"Mooring: {element.GetType().FullName}" :
                "Released";
    }
}
