////////////////////////////////////////////////////////////////////////////
//
// Epoxy - An independent flexible XAML MVVM library for .NET
// Copyright (c) Kouji Matsui (@kozy_kekyo, @kekyo@mastodon.cloud)
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

#if MAUI
using Microsoft.Maui.Controls;
using DependencyObject = Microsoft.Maui.Controls.BindableObject;
#endif

#if AVALONIA
using DependencyObject = Avalonia.IAvaloniaObject;
#endif

using Epoxy.Supplemental;

namespace Epoxy.Advanced
{
    public interface IAttachedObject
    {
        DependencyObject? AssociatedObject { get; }

        void Attach(DependencyObject dependencyObject);

        void Detach();
    }

    public abstract partial class AttachedObject :    // HACK: partial is required on Uno platform because it makes inserting for DO implementation on the building time.
#if WINDOWS_WPF
        Freezable, IAttachedObject
#endif
#if WINDOWS_UWP || WINUI || UNO || OPENSILVER
        DependencyObject, IAttachedObject
#endif
#if XAMARIN_FORMS || MAUI
        Element, IAttachedObject
#endif
#if AVALONIA
        LogicalTreeObject, IAttachedObject
#endif
    {
        private DependencyObject? associatedObject;

        /// <summary>
        /// The constructor.
        /// </summary>
        protected AttachedObject()
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

    public class AttachedObject<TSelf> :
        AttachedObject
#if WINDOWS_WPF
        where TSelf : Freezable, IAttachedObject, new()
#endif
#if WINDOWS_UWP || WINUI || UNO || OPENSILVER
        where TSelf : DependencyObject, IAttachedObject, new()
#endif
#if XAMARIN_FORMS || MAUI
        where TSelf : Element, IAttachedObject, new()
#endif
#if AVALONIA
        where TSelf : AttachedObject, new()
#endif
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        public AttachedObject()
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
