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

namespace Epoxy.Internal
{
    internal sealed class ChildrenBridge : IDisposable
    {
        private Panel? panel;
        private IList<UIElement>? collection;

        public ChildrenBridge(Panel panel, IList<UIElement> collection)
        {
            this.panel = panel;
            this.collection = collection;

            var panelChildren = PanelChildrenAccessor.GetPanelChildrenAccessor(this.panel);

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

                var panelChildren = PanelChildrenAccessor.GetPanelChildrenAccessor(this.panel!);

                panelChildren.Clear(this.panel!);

                this.collection = null;
                this.panel = null;
            }
        }

        private void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Debug.Assert(this.panel != null);

            var collection = this.collection!;
            var panelChildren = PanelChildrenAccessor.GetPanelChildrenAccessor(this.panel!);

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
