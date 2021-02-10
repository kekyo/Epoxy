////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
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

using System;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

#if WINDOWS_UWP
using Windows.UI.Xaml;
#endif

#if WINDOWS_WPF
using System.Windows;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
using DependencyObject = Xamarin.Forms.BindableObject;
using UIElement = Xamarin.Forms.Element;
#endif

namespace Epoxy
{
    public static class Anchor
    {
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

        public static Pile? GetPile(DependencyObject d) =>
            (Pile?)d.GetValue(PileProperty);

        public static void SetPile(DependencyObject d, Pile? pile) =>
            d.SetValue(PileProperty, pile);

        public static void ClearPile(DependencyObject d) =>
            d.ClearValue(PileProperty);
    }

    public abstract class Pile
    {
        internal Pile()
        { }

        internal abstract void Moore(UIElement element);
        internal abstract void Release(UIElement element);

        public static Pile<UIElement> Create() =>
            new Pile<UIElement>();

        public static Pile<TUIElement> Create<TUIElement>()
            where TUIElement : UIElement =>
            new Pile<TUIElement>();
    }

    public sealed class Pile<TUIElement> : Pile
        where TUIElement : UIElement
    {
        private readonly WeakReference element =
            new WeakReference(null);

        internal override void Moore(UIElement element)
        {
            Debug.Assert(element is TUIElement);
            this.element.Target = (TUIElement)element;
        }

        internal override void Release(UIElement element)
        {
            Debug.Assert(this.element.Target is TUIElement e && object.ReferenceEquals(e, element));
            this.element.Target = null;
        }

        [Obsolete("Synchronous instance method is obsoleted. Use ExecuteSync instead.")]
        public void Execute(Action<TUIElement> action, bool canIgnore = false)
        {
            if (this.element.Target is TUIElement element)
            {
                action(element);
            }
            else if (!canIgnore)
            {
                throw new InvalidOperationException("Didn't moore a UIElement.");
            }
        }

        [Obsolete("Synchronous instance method is obsoleted. Use ExecuteSync instead.")]
        public T Execute<T>(Func<TUIElement, T> action)
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

        public ValueTask ExecuteAsync(Func<TUIElement, ValueTask> action, bool canIgnore = false)
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

        public ValueTask<T> ExecuteAsync<T>(Func<TUIElement, ValueTask<T>> action)
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

        public override string ToString() =>
            this.element.Target is TUIElement element ?
                $"Mooring: {element.GetType().FullName}" :
                "Released";
    }

    public static class PileExtension
    {
        public static ValueTask ExecuteAsync<TUIElement>(
            this Pile<TUIElement> pile,
            Func<TUIElement, Task> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            pile.ExecuteAsync(element => new ValueTask(action(element)), canIgnore);

        public static ValueTask<T> ExecuteAsync<TUIElement, T>(
            this Pile<TUIElement> pile,
            Func<TUIElement, Task<T>> action)
            where TUIElement : UIElement =>
            pile.ExecuteAsync(element => new ValueTask<T>(action(element)));

        public static void ExecuteSync<TUIElement>(
            this Pile<TUIElement> pile,
            Action<TUIElement> action, bool canIgnore = false)
            where TUIElement : UIElement =>
            pile.ExecuteAsync(element => { action(element); return default; }, canIgnore);

        public static T ExecuteSync<TUIElement, T>(
            this Pile<TUIElement> pile,
            Func<TUIElement, T> action)
            where TUIElement : UIElement
        {
            var (result, edi) = pile.ExecuteAsync(element =>
            {
                try
                {
                    return new ValueTask<(T, ExceptionDispatchInfo?)>((action(element), default));
                }
                catch (Exception ex)
                {
                    return new ValueTask<(T, ExceptionDispatchInfo?)>((default!, ExceptionDispatchInfo.Capture(ex)));
                }
            }).Result;  // Will not block

            edi?.Throw();
            return result;
        }
    }
}
