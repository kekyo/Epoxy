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
using System.Diagnostics;

#if WINDOWS_UWP || UNO
using Windows.UI.Xaml;
#endif

#if WINUI
using Microsoft.UI.Xaml;
#endif

#if WINDOWS_WPF || OPENSILVER
using System.Windows;
using System.Windows.Media.Animation;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
using DependencyObject = Xamarin.Forms.BindableObject;
#endif

#if AVALONIA
using Avalonia.Controls;
using Avalonia.LogicalTree;
using DependencyObject = Avalonia.IAvaloniaObject;
#endif

using Epoxy.Internal;
using Epoxy.Supplemental;

namespace Epoxy.Advanced
{
    public class AttachableCollection<TSelf, TObject> :
        LogicalTreeObjectCollection<TSelf, TObject>, IAttachedObject
        where TSelf : LogicalTreeObjectCollection<TObject>, IAttachedObject, new()
#if AVALONIA
        where TObject : ILogical, ISetLogicalParent, IAttachedObject
#elif UNO && !WINDOWS_UWP
        where TObject : class, DependencyObject, IAttachedObject
#elif WINDOWS_WPF
        where TObject : Freezable, IAttachedObject
#elif XAMARIN_FORMS
        where TObject : Element, IAttachedObject
#else
        where TObject : DependencyObject, IAttachedObject
#endif
    {
        private DependencyObject? associatedObject;

        public DependencyObject? AssociatedObject =>
            this.associatedObject;

        public AttachableCollection()
        {
        }

        protected sealed override void OnAdded(TObject element)
        {
            if (this.associatedObject != null)
            {
                element.Attach(this.associatedObject);
            }
        }

        protected sealed override void OnRemoving(TObject element)
        {
            if (this.associatedObject != null)
            {
                element.Detach();
            }
        }

        protected virtual void OnAttached()
        {
        }

        protected virtual void OnDetaching()
        {
        }

        public void Attach(DependencyObject dependencyObject)
        {
#if XAMARIN_FORMS
            this.Parent = dependencyObject as Element;
#endif
            if (this.associatedObject != null)
            {
                throw new InvalidOperationException();
            }

            if (!InternalDesigner.IsDesignTime)
            {
                this.WritePreamble();
                this.associatedObject = dependencyObject;
                this.WritePostscript();

                foreach (var item in this)
                {
                    item.Attach(dependencyObject);
                }
            }

            this.OnAttached();
        }

        public void Detach()
        {
            this.OnDetaching();

            foreach (var item in this)
            {
                item.Detach();
            }

            this.WritePreamble();
            this.associatedObject = null;
            this.WritePostscript();

#if XAMARIN_FORMS
            this.Parent = null;
#endif
        }

#if !WINDOWS_WPF
        [Conditional("WINDOWS_WPF")]
        private void ReadPreamble()
        { }
        [Conditional("WINDOWS_WPF")]
        private void WritePreamble()
        { }
        [Conditional("WINDOWS_WPF")]
        private void WritePostscript()
        { }
#endif
    }
}
