////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
// Copyright (c) 2020 Kouji Matsui (@kozy_kekyo, @kekyo2)
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
using UIElement = Xamarin.Forms.Element;
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
        public static readonly BindableProperty ChildrenProperty =
            BindableProperty.CreateAttached(
                "Children",
                typeof(IList<UIElement>),
                typeof(ChildrenBinder),
                null,
                BindingMode.OneWay,
                null,
                Children_PropertyChanged);
#else
        public static readonly DependencyProperty ChildrenProperty =
            DependencyProperty.RegisterAttached(
                "Children",
                typeof(IList<UIElement>),
                typeof(ChildrenBinder),
                new PropertyMetadata(null, Children_PropertyChanged));
#endif

        public static IList<UIElement>? GetChildren(DependencyObject d) =>
            (IList<UIElement>?)d.GetValue(ChildrenProperty);

        public static void SetChildren(DependencyObject d, IList<UIElement>? children) =>
            d.SetValue(ChildrenProperty, children);

#if XAMARIN_FORMS
        private static void Children_PropertyChanged(
            BindableObject d, object? oldValue, object? newValue)
        {
#else
        private static void Children_PropertyChanged(
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

        private sealed class ChildrenBridge : IDisposable
        {
            private Panel? panel;
            private IList<UIElement>? children;

            public ChildrenBridge(Panel panel, IList<UIElement> children)
            {
                this.panel = panel;
                this.children = children;

#if XAMARIN_FORMS
                // TODO: Cannot use DLR infrastructure on AOT platforms.
                var panelChildren = ((dynamic)this.panel).Children;
#else
                var panelChildren = this.panel.Children;
#endif

                panelChildren.Clear();
                foreach (var child in this.children)
                {
                    panelChildren.Add(child);
                }

                if (this.children is INotifyCollectionChanged ncc)
                {
                    ncc.CollectionChanged += this.CollectionChanged;
                }
            }

            public void Dispose()
            {
                if (this.children != null)
                {
                    if (this.children is INotifyCollectionChanged ncc)
                    {
                        ncc.CollectionChanged -= this.CollectionChanged;
                    }
#if XAMARIN_FORMS
                    // TODO: Cannot use DLR infrastructure on AOT platforms.
                    var panelChildren = ((dynamic)this.panel!).Children;
#else
                    var panelChildren = this.panel!.Children;
#endif
                    panelChildren.Clear();

                    this.children = null;
                    this.panel = null;
                }
            }

            private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Debug.Assert(this.panel != null);

                var children = this.children!;
#if XAMARIN_FORMS
                // TODO: Cannot use DLR infrastructure on AOT platforms.
                var panelChildren = ((dynamic)this.panel!).Children;
#else
                var panelChildren = this.panel!.Children;
#endif

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems.Count >= 1)
                        {
                            var startIndex =
                                ((e.NewStartingIndex < children.Count) &&
                                 (panelChildren.IndexOf(children[e.NewStartingIndex]) is int panelIndex &&
                                  panelIndex >= 0)) ?
                                  panelIndex : panelChildren.Count;

                            for (var relativeIndex = 0; relativeIndex < e.NewItems.Count; relativeIndex++)
                            {
                                var child = (UIElement?)e.NewItems[relativeIndex];
                                panelChildren.Insert(relativeIndex + startIndex, child);
                            }
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (UIElement? child in e.OldItems)
                        {
                            panelChildren.Remove(child);
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        foreach (var entry in
                            e.OldItems.Cast<UIElement?>().
                            Zip(e.NewItems.Cast<UIElement?>(),
                            (o, n) => new { panelIndex = panelChildren.IndexOf(o), newChild = n }).
                            Where(entry => entry.panelIndex >= 0))
                        {
                            panelChildren[entry.panelIndex] = entry.newChild;
                        }
                        break;

                    case NotifyCollectionChangedAction.Move:
                        for (var relativeIndex = 0; relativeIndex < e.NewItems.Count; relativeIndex++)
                        {
                            var child = (UIElement?)e.NewItems[relativeIndex];
                            panelChildren.RemoveAt(e.OldStartingIndex);
                            panelChildren.Insert(e.NewStartingIndex + relativeIndex, child);
                        }

                        break;

                    case NotifyCollectionChangedAction.Reset:
                        panelChildren.Clear();
                        break;
                }
            }
        }
    }
}
