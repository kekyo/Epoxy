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
using System.ComponentModel;
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
using DependencyObject = Avalonia.IAvaloniaObject;
#endif

using Epoxy.Internal;

namespace Epoxy.Supplemental
{
    public interface IAttachableObject
    {
        DependencyObject? AssociatedObject { get; }

        void Attach(DependencyObject dependencyObject);

        void Detach();
    }

    public abstract partial class AttachableObject :    // HACK: partial is required on Uno platform because it makes inserting for DO implementation on the building time.
#if WINDOWS_WPF
        Freezable, IAttachableObject
#endif
#if WINDOWS_UWP || WINUI || UNO || OPENSILVER
        DependencyObject, IAttachableObject
#endif
#if XAMARIN_FORMS
        Element, IAttachableObject
#endif
#if AVALONIA
        PlainObject, IAttachableObject
#endif
    {
        private DependencyObject? associatedObject;

        /// <summary>
        /// The constructor.
        /// </summary>
        protected AttachableObject()
        { }

        public DependencyObject? AssociatedObject =>
            this.associatedObject;

        protected virtual void OnAttached()
        {
        }

        protected virtual void OnDetaching()
        {
        }

        /// <summary>
        /// Attach a parent element.
        /// </summary>
        /// <param name="associatedObject">Parent element instance</param>
        public void Attach(DependencyObject? associatedObject)
        {
            this.associatedObject = associatedObject;
            this.OnAttached();
        }

        /// <summary>
        /// Detach already attached parent element.
        /// </summary>
        public void Detach()
        {
            this.OnDetaching();
            this.associatedObject = null;
        }
    }

    public class AttachableObject<TSelf> :
        AttachableObject
#if WINDOWS_WPF
        where TSelf : Freezable, IAttachableObject, new()
#endif
#if WINDOWS_UWP || WINUI || UNO || OPENSILVER
        where TSelf : DependencyObject, IAttachableObject, new()
#endif
#if XAMARIN_FORMS
        where TSelf : Element, IAttachableObject, new()
#endif
#if AVALONIA
        where TSelf : PlainObject, IAttachableObject, new()
#endif
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        public AttachableObject()
        { }

#if WINDOWS_WPF
        /// <summary>
        /// Create this class instance.
        /// </summary>
        /// <returns>Event instance</returns>
        /// <remarks>It will be used internal only.</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected sealed override Freezable CreateInstanceCore() =>
            new TSelf();
#endif
    }
}
