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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

namespace Epoxy.Internal
{
#if XAMARIN_FORMS
    [DebuggerStepThrough]
    internal abstract class PanelChildrenAccessor
    {
        public abstract int Count(Panel panel);
        public abstract int IndexOf(Panel panel, UIElement element);
        public abstract void Clear(Panel panel);
        public abstract void Add(Panel panel, UIElement element);
        public abstract void Insert(Panel panel, int index, UIElement element);
        public abstract void Set(Panel panel, int index, UIElement element);
        public abstract void Remove(Panel panel, UIElement element);
        public abstract void RemoveAt(Panel panel, int index);

        public static readonly Dictionary<Type, PanelChildrenAccessor> accessors =
            new Dictionary<Type, PanelChildrenAccessor>();

        public static PanelChildrenAccessor GetPanelChildrenAccessor(Panel panel)
        {
            Debug.Assert(InternalUIThread.UnsafeIsBound());

            var type = panel.GetType();
            if (!accessors.TryGetValue(type, out var accessor))
            {
                var elementType = type.GetInterfaces().
                    Where(it => it.IsGenericType && it.GetGenericTypeDefinition() == typeof(IViewContainer<>)).
                    Select(it => it.GetGenericArguments()[0]).
                    First();

                accessor = (PanelChildrenAccessor)Activator.CreateInstance(
                    typeof(PanelChildrenAccessor<>).MakeGenericType(elementType));
                accessors.Add(type, accessor);
            }
            return accessor;
        }
    }

    [DebuggerStepThrough]
    internal sealed class PanelChildrenAccessor<T> : PanelChildrenAccessor
        where T : UIElement
    {
        public override int Count(Panel panel) =>
            ((IViewContainer<T>)panel).Children.Count;

        public override int IndexOf(Panel panel, UIElement element) =>
            ((IViewContainer<T>)panel).Children.IndexOf((T)element);

        public override void Clear(Panel panel) =>
            ((IViewContainer<T>)panel).Children.Clear();

        public override void Add(Panel panel, UIElement element) =>
            ((IViewContainer<T>)panel).Children.Add((T)element);

        public override void Insert(Panel panel, int index, UIElement element) =>
            ((IViewContainer<T>)panel).Children.Insert(index, (T)element);

        public override void Set(Panel panel, int index, UIElement element) =>
            ((IViewContainer<T>)panel).Children[index] = (T)element;

        public override void Remove(Panel panel, UIElement element) =>
            ((IViewContainer<T>)panel).Children.Remove((T)element);

        public override void RemoveAt(Panel panel, int index) =>
            ((IViewContainer<T>)panel).Children.RemoveAt(index);
    }
#else
    [DebuggerStepThrough]
    internal sealed class PanelChildrenAccessor
    {
        private PanelChildrenAccessor()
        { }

        public int Count(Panel panel) =>
            panel.Children.Count;

        public int IndexOf(Panel panel, UIElement element) =>
            panel.Children.IndexOf(element);

        public void Clear(Panel panel) =>
            panel.Children.Clear();

        public void Add(Panel panel, UIElement element) =>
            panel.Children.Add(element);

        public void Insert(Panel panel, int index, UIElement element) =>
            panel.Children.Insert(index, element);

        public void Set(Panel panel, int index, UIElement element) =>
            panel.Children[index] = element;

        public void Remove(Panel panel, UIElement element) =>
            panel.Children.Remove(element);

        public void RemoveAt(Panel panel, int index) =>
            panel.Children.RemoveAt(index);

        private static readonly PanelChildrenAccessor accessor =
            new PanelChildrenAccessor();

        public static PanelChildrenAccessor GetPanelChildrenAccessor(Panel panel) =>
            accessor;
    }
#endif
}
