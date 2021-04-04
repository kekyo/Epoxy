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
using Windows.UI.Xaml.Controls;
#endif

#if WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#endif

#if WINDOWS_WPF
using System.Windows;
using System.Windows.Controls;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
using DependencyObject = Xamarin.Forms.BindableObject;
using DependencyProperty = Xamarin.Forms.BindableProperty;
using UIElement = Xamarin.Forms.VisualElement;
using Panel = Xamarin.Forms.Layout;
#endif

#if AVALONIA
using Avalonia;
using DependencyObject = Avalonia.IAvaloniaObject;
using DependencyProperty = Avalonia.AvaloniaProperty;
using UIElement = Avalonia.Controls.IControl;
using Panel = Avalonia.Controls.IPanel;
#endif

namespace Epoxy
{
    /// <summary>
    /// The ChildrenAnchor is used with ChildrenPile, there will bind loosely and can rent control and manipulate children temporarily.
    /// </summary>
    /// <remarks>See ChildrenAnchor/ChildrenPile guide: https://github.com/kekyo/Epoxy#anchorpile</remarks>
    /// <example>
    /// <code>
    /// &lt;Window xmlns:epoxy="https://github.com/kekyo/Epoxy"&gt;
    ///    &lt;!-- ... --&gt;
    ///    &lt;!-- Placed Anchor onto the TextBox and bound property --&gt;
    ///    &lt;TextBox epoxy:Anchor.Pile="{Binding LogPile}" /&gt;
    /// &lt;/Window&gt;
    /// </code>
    /// </example>
    public sealed class ChildrenAnchor
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        private ChildrenAnchor()
        { }

#if XAMARIN_FORMS
        private static readonly BindableProperty ChildrenPileProperty =
            BindableProperty.CreateAttached(
                "ChildrenPile",
                typeof(ChildrenPile),
                typeof(ChildrenAnchor),
                null,
                BindingMode.OneWay,
                null,
                (b, o, n) =>
                {
                    if (o is ChildrenPile op)
                    {
                        op.Release((UIElement)b);
                    }
                    if (n is ChildrenPile np)
                    {
                        np.Moore((UIElement)b);
                    }
                });
#elif AVALONIA
        private static readonly AvaloniaProperty<ChildrenPile?> ChildrenPileProperty =
            AvaloniaProperty.RegisterAttached<ChildrenAnchor, UIElement, ChildrenPile?>("ChildrenPile");

        /// <summary>
        /// The type initializer.
        /// </summary>
        static ChildrenAnchor() =>
            ChildrenPileProperty.Changed.Subscribe(e =>
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
        private static readonly DependencyProperty ChildrenPileProperty =
            DependencyProperty.RegisterAttached(
                "ChildrenPile",
                typeof(ChildrenPile),
                typeof(ChildrenAnchor),
                new PropertyMetadata(null, (d, e) =>
                {
                    if (e.OldValue is ChildrenPile op)
                    {
                        op.Release((UIElement)d);
                    }
                    if (e.NewValue is ChildrenPile np)
                    {
                        np.Moore((UIElement)d);
                    }
                }));
#endif

        /// <summary>
        /// Get ChildrenPile from this ChildrenAnchor.
        /// </summary>
        public static ChildrenPile? GetChildrenPile(DependencyObject d) =>
            (ChildrenPile?)d.GetValue(ChildrenPileProperty);

        /// <summary>
        /// Set ChildrenPile from this ChildrenAnchor.
        /// </summary>
        public static void SetChildrenPile(DependencyObject d, ChildrenPile? pile) =>
            d.SetValue(ChildrenPileProperty, pile);

        /// <summary>
        /// Clear ChildrenPile from this ChildrenAnchor.
        /// </summary>
        public static void ClearChildrenPile(DependencyObject d) =>
            d.ClearValue(ChildrenPileProperty);
    }

    /// <summary>
    /// The ChildrenPile base class is used with ChildrenAnchor.
    /// </summary>
    /// <remarks>You can use with generic ChildrenPile&lt;T&gt; class.</remarks>
    public abstract class ChildrenPile
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        private protected ChildrenPile()
        { }

        /// <summary>
        /// Will bind an ChildrenAnchor.
        /// </summary>
        /// <param name="element">Target anchoring element</param>
        internal abstract void Moore(UIElement element);

        /// <summary>
        /// Will release an ChildrenAnchor.
        /// </summary>
        /// <param name="element">Target anchoring element</param>
        internal abstract void Release(UIElement element);
    }

    /// <summary>
    /// The ChildrenPile is used with ChildrenAnchor, there will bind loosely and can rent control and manipulate children temporarily.
    /// </summary>
    /// <remarks>See ChildrenAnchor/ChildrenPile guide: https://github.com/kekyo/Epoxy#anchorpile</remarks>
    /// <example>
    /// <code>
    /// // Declared a ChildrenPile into the ViewModel.
    /// this.ButtonPanelPile = ChildrenPile.Create&lt;Button&gt;();
    ///
    /// // ...
    ///
    /// // Do rent by ChildrenPile when we have to manipulate the Button directly:
    /// await this.ButtonPanelPile.ManipulateAsync(async children =>
    /// {
    ///    // Manipulate children directly.
    ///    children.Add(new Button { ... });
    /// });
    /// </code>
    /// </example>
    public sealed class ChildrenPile<TUIElement> : ChildrenPile
        where TUIElement : UIElement
    {
        private readonly WeakReference element =
            new WeakReference(null);

        /// <summary>
        /// The constructor.
        /// </summary>
        internal ChildrenPile()
        { }

        /// <summary>
        /// Will bind a ChildrenAnchor.
        /// </summary>
        /// <param name="element">Target anchoring element</param>
        internal override void Moore(UIElement element)
        {
            Debug.Assert(element is Panel);
            this.element.Target = (Panel)element;
        }

        /// <summary>
        /// Will release a ChildrenAnchor.
        /// </summary>
        /// <param name="element">Target anchoring element</param>
        internal override void Release(UIElement element)
        {
            Debug.Assert(this.element.Target is Panel e && object.ReferenceEquals(e, element));
            this.element.Target = null;
        }

        /// <summary>
        /// Manipulate with anchoring element.
        /// </summary>
        /// <param name="action">Continuation delegate</param>
        /// <param name="canIgnore">Ignore if not present mooring Anchor</param>
        /// <returns>ValueTask</returns>
        /// <remarks>This method is used internal only.</remarks>
        internal ValueTask<Unit> InternalManipulateAsync(Func<IList<TUIElement>, ValueTask<Unit>> action, bool canIgnore = false)
        {
            if (this.element.Target is Panel panel)
            {
                return action(new PanelChildrenCollection<TUIElement>(panel));
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
        /// Manipulate with anchoring element.
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="action">Continuation delegate</param>
        /// <returns>ValueTask&lt;TResult&gt;</returns>
        /// <remarks>This method is used internal only.</remarks>
        internal ValueTask<TResult> InternalManipulateAsync<TResult>(Func<IList<TUIElement>, ValueTask<TResult>> action)
        {
            if (this.element.Target is Panel panel)
            {
                return action(new PanelChildrenCollection<TUIElement>(panel));
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
        internal void InternalManipulateSync(Action<IList<TUIElement>> action, bool canIgnore) =>
            this.InternalManipulateAsync(children => { action(children); return default; }, canIgnore);

        /// <summary>
        /// Synchronous execute with anchoring element.
        /// </summary>
        /// <typeparam name="TResult">Result type</typeparam>
        /// <param name="action">Continuation delegate</param>
        /// <returns>TResult</returns>
        /// <remarks>This method is used internal only.</remarks>
        internal TResult InternalManipulateSync<TResult>(Func<IList<TUIElement>, TResult> action)
        {
            var result = this.InternalManipulateAsync(children =>
            {
                try
                {
                    return InternalHelpers.FromResult(InternalHelpers.Pair(action(children), default(ExceptionDispatchInfo)!));
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
            this.element.Target is Panel panel ?
                $"Mooring: {panel.GetType().FullName}" :
                "Released";
    }
}
