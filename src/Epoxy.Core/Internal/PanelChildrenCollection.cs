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
using System.Collections;
using System.Collections.Generic;
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
using UIElement = Xamarin.Forms.VisualElement;
using Panel = Xamarin.Forms.Layout;
#endif

#if AVALONIA
using Avalonia;
using UIElement = Avalonia.Controls.IControl;
using Panel = Avalonia.Controls.IPanel;
#endif

namespace Epoxy.Internal
{
    internal sealed class PanelChildrenCollection<TUIElement> : IList<TUIElement>
        where TUIElement : UIElement
    {
        private Panel panel;
        private PanelChildrenAccessor accessor;

        public PanelChildrenCollection(Panel panel)
        {
            this.panel = panel;
            this.accessor = PanelChildrenAccessor.GetPanelChildrenAccessor(panel);
        }

        public TUIElement this[int index]
        {
            get => (TUIElement)this.accessor.Get(this.panel, index);
            set => this.accessor.Set(this.panel, index, value);
        }

        public int Count =>
            this.accessor.Count(this.panel);

        bool ICollection<TUIElement>.IsReadOnly => false;

        public void Add(TUIElement item) =>
            this.accessor.Add(this.panel, item);

        public void Clear() =>
            this.accessor.Clear(this.panel);

        public bool Contains(TUIElement item) =>
            this.accessor.Contains(this.panel, item);

        public void CopyTo(TUIElement[] array, int arrayIndex) =>
            throw new NotImplementedException();

        public int IndexOf(TUIElement item) =>
            this.accessor.IndexOf(this.panel, item);

        public void Insert(int index, TUIElement item) =>
            this.accessor.Insert(this.panel, index, item);

        public bool Remove(TUIElement item) =>
            this.accessor.Remove(this.panel, item);

        public void RemoveAt(int index) =>
            this.accessor.RemoveAt(this.panel, index);

        public IEnumerator<TUIElement> GetEnumerator() =>
            this.accessor.GetEnumerable(this.panel).Cast<TUIElement>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            this.accessor.GetEnumerable(this.panel).GetEnumerator();
    }
}
