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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

namespace Epoxy
{
    public static class ChildrenBinder
    {
#if XAMARIN_FORMS
        private static readonly BindableProperty ChildrenBridgeProperty =
            BindableProperty.CreateAttached(
                "ChildrenBridge",
                typeof(IDisposable),
                typeof(ChildrenBinder),
                null);
#else
        private static readonly DependencyProperty ChildrenBridgeProperty =
            DependencyProperty.RegisterAttached(
                "ChildrenBridge",
                typeof(IDisposable),
                typeof(ChildrenBinder),
                new PropertyMetadata(null));
#endif

        private static IDisposable? GetChildrenBridge(DependencyObject d) =>
            (IDisposable?)d.GetValue(ChildrenBridgeProperty);

        private static void SetChildrenBridge(DependencyObject d, IDisposable? cb) =>
            d.SetValue(ChildrenBridgeProperty, cb);

        private static void ClearChildrenBridge(DependencyObject d) =>
            d.ClearValue(ChildrenBridgeProperty);

#if XAMARIN_FORMS
        public static readonly BindableProperty CollectionProperty =
            BindableProperty.CreateAttached(
                "Collection",
                typeof(IList<UIElement>),
                typeof(ChildrenBinder),
                null,
                BindingMode.OneWay,
                null,
                Collection_PropertyChanged);
#else
        public static readonly DependencyProperty CollectionProperty =
            DependencyProperty.RegisterAttached(
                "Collection",
                typeof(IList<UIElement>),
                typeof(ChildrenBinder),
                new PropertyMetadata(null, Collection_PropertyChanged));
#endif

        public static IList<UIElement>? GetCollection(DependencyObject d) =>
            (IList<UIElement>?)d.GetValue(CollectionProperty);

        public static void SetCollection(DependencyObject d, IList<UIElement>? children) =>
            d.SetValue(CollectionProperty, children);

#if XAMARIN_FORMS
        private static void Collection_PropertyChanged(
            BindableObject d, object? oldValue, object? newValue)
        {
#else
        private static void Collection_PropertyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue;
#endif
            Debug.Assert(d is Panel);

            if (d is Panel panel)
            {
                if (GetChildrenBridge(panel) is IDisposable cb)
                {
                    cb.Dispose();
                    ClearChildrenBridge(panel);
                }

                if (newValue is IList<UIElement> nv)
                {
                    cb = new ChildrenBridge(panel, nv);
                    SetChildrenBridge(panel, cb);
                }
            }
        }

#if XAMARIN_FORMS
        private abstract class PanelChildrenAccessor
        {
            public abstract int Count(Panel panel);
            public abstract int IndexOf(Panel panel, UIElement element);
            public abstract void Clear(Panel panel);
            public abstract void Add(Panel panel, UIElement element);
            public abstract void Insert(Panel panel, int index, UIElement element);
            public abstract void Set(Panel panel, int index, UIElement element);
            public abstract void Remove(Panel panel, UIElement element);
            public abstract void RemoveAt(Panel panel, int index);
        }

        [DebuggerStepThrough]
        private sealed class PanelChildrenAccessor<T> : PanelChildrenAccessor
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

        private static readonly Dictionary<Type, PanelChildrenAccessor> accessors =
            new Dictionary<Type, PanelChildrenAccessor>();

        [DebuggerStepThrough]
        private static PanelChildrenAccessor GetPanelChildrenAccessor(Panel panel)
        {
            Debug.Assert(UIThread.IsBound);

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
#else
        [DebuggerStepThrough]
        private sealed class PanelChildrenAccessor
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

            public static readonly PanelChildrenAccessor Instance =
                new PanelChildrenAccessor();
        }

        [DebuggerStepThrough]
        private static PanelChildrenAccessor GetPanelChildrenAccessor(Panel panel) =>
            PanelChildrenAccessor.Instance;
#endif

        private sealed class ChildrenBridge : IDisposable
        {
            private Panel? panel;
            private IList<UIElement>? collection;

            public ChildrenBridge(Panel panel, IList<UIElement> collection)
            {
                this.panel = panel;
                this.collection = collection;

                var panelChildren = GetPanelChildrenAccessor(this.panel);

                panelChildren.Clear(this.panel);
                foreach (var child in this.collection)
                {
                    panelChildren.Add(this.panel, child);
                }

                if (this.collection is INotifyCollectionChanged ncc)
                {
                    ncc.CollectionChanged += this.CollectionChanged;
                }
            }

            public void Dispose()
            {
                if (this.collection != null)
                {
                    if (this.collection is INotifyCollectionChanged ncc)
                    {
                        ncc.CollectionChanged -= this.CollectionChanged;
                    }

                    var panelChildren = GetPanelChildrenAccessor(this.panel!);

                    panelChildren.Clear(this.panel!);

                    this.collection = null;
                    this.panel = null;
                }
            }

            private void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
            {
                Debug.Assert(this.panel != null);

                var collection = this.collection!;
                var panelChildren = GetPanelChildrenAccessor(this.panel!);

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems!.Count >= 1)
                        {
                            var startIndex =
                                ((e.NewStartingIndex < collection.Count) &&
                                 (panelChildren.IndexOf(this.panel!, collection[e.NewStartingIndex]) is int panelIndex &&
                                  panelIndex >= 0)) ?
                                  panelIndex : panelChildren.Count(this.panel!);

                            for (var relativeIndex = 0; relativeIndex < e.NewItems.Count; relativeIndex++)
                            {
                                var child = (UIElement?)e.NewItems[relativeIndex];
                                panelChildren.Insert(this.panel!, relativeIndex + startIndex, child!);
                            }
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (UIElement? child in e.OldItems!)
                        {
                            panelChildren.Remove(this.panel!, child!);
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        foreach (var entry in
                            e.OldItems!.Cast<UIElement?>().
                            Zip(e.NewItems!.Cast<UIElement?>(),
                            (o, n) => new { panelIndex = panelChildren.IndexOf(this.panel!, o!), newChild = n }).
                            Where(entry => entry.panelIndex >= 0))
                        {
                            panelChildren.Set(this.panel!, entry.panelIndex, entry.newChild!);
                        }
                        break;

                    case NotifyCollectionChangedAction.Move:
                        for (var relativeIndex = 0; relativeIndex < e.NewItems!.Count; relativeIndex++)
                        {
                            var child = (UIElement?)e.NewItems[relativeIndex];
                            panelChildren.RemoveAt(this.panel!, e.OldStartingIndex);
                            panelChildren.Insert(this.panel!, e.NewStartingIndex + relativeIndex, child!);
                        }
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        panelChildren.Clear(this.panel!);
                        break;
                }
            }
        }
    }
}
