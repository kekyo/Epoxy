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
using System.Diagnostics;

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
    public sealed class ChildrenBinder
    {
        private ChildrenBinder()
        { }

#if XAMARIN_FORMS
        private static readonly BindableProperty ChildrenBridgeProperty =
            BindableProperty.CreateAttached(
                "ChildrenBridge",
                typeof(IDisposable),
                typeof(ChildrenBinder),
                null);
#elif AVALONIA
        private static readonly AvaloniaProperty<IDisposable?> ChildrenBridgeProperty =
            DependencyProperty.RegisterAttached<ChildrenBinder, DependencyObject, IDisposable?>("ChildrenBridge");
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
                OnPropertyChanged);
#elif AVALONIA
        private static readonly AvaloniaProperty<IList<UIElement>?> CollectionProperty =
            DependencyProperty.RegisterAttached<ChildrenBinder, Panel, IList<UIElement>?>("ChildrenBridge");

        static ChildrenBinder() =>
            CollectionProperty.Changed.Subscribe(OnPropertyChanged);
#else
        public static readonly DependencyProperty CollectionProperty =
            DependencyProperty.RegisterAttached(
                "Collection",
                typeof(IList<UIElement>),
                typeof(ChildrenBinder),
                new PropertyMetadata(null, OnPropertyChanged));
#endif

        public static IList<UIElement>? GetCollection(DependencyObject d) =>
            (IList<UIElement>?)d.GetValue(CollectionProperty);

        public static void SetCollection(DependencyObject d, IList<UIElement>? children) =>
            d.SetValue(CollectionProperty, children);

#if XAMARIN_FORMS
        private static void OnPropertyChanged(
            BindableObject d, object? oldValue, object? newValue)
        {
#elif AVALONIA
        private static void OnPropertyChanged(
            AvaloniaPropertyChangedEventArgs<IList<UIElement>?> e)
        {
            var d = e.Sender;
            var newValue = e.NewValue.GetValueOrDefault();
#else
        private static void OnPropertyChanged(
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
    }
}
