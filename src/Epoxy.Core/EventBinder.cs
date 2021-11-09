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
using System.ComponentModel;
using System.Windows.Input;

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
using Avalonia;
using Avalonia.Data;
using DependencyObject = Avalonia.IAvaloniaObject;
#endif

using Epoxy.Internal;
using Epoxy.Supplemental;
using Epoxy.Advanced;

namespace Epoxy
{
    /// <summary>
    /// The EventBinder give bindable ability for standard CLR events by ICommand infrastructure.
    /// </summary>
    /// <remarks>See EventBinder guide: https://github.com/kekyo/Epoxy#eventbinder</remarks>
    /// <example>
    /// <code>
    /// &lt;Window xmlns:epoxy="https://github.com/kekyo/Epoxy"&gt;
    ///    &lt;!-- ... --&gt;
    ///    &lt;epoxy:EventBinder.Events&gt;
    ///         &lt;!-- Binding the Window.Loaded event to the ViewModel's Ready property --&gt;
    ///         &lt;epoxy:Event Name = "Loaded" Command="{Binding Ready}" /&gt;
    ///    &lt;/epoxy:EventBinder.Events&gt;
    /// &lt;/Window&gt;
    /// </code>
    /// </example>
#if AVALONIA
    public sealed class EventBinder
#else
    public static class EventBinder
#endif
    {
#if XAMARIN_FORMS
        private static readonly BindablePropertyKey EventsPropertyKey =
            BindableProperty.CreateAttachedReadOnly(
                "Events",
                typeof(EventsCollection),
                typeof(EventBinder),
                null,
                BindingMode.OneTime,
                null,
                null,
                null,
                null,
                d =>
                {
                    var collection = new EventsCollection();
                    collection.Attach(d);
                    return collection;
                });

        /// <summary>
        /// Declared Events attached property.
        /// </summary>
        public static readonly BindableProperty EventsProperty =
            EventsPropertyKey.BindableProperty;

        /// <summary>
        /// Get EventsCollection instance.
        /// </summary>
        public static EventsCollection? GetEvents(DependencyObject d) =>
            (EventsCollection?)d.GetValue(EventsProperty);
#elif AVALONIA
        /// <summary>
        /// The constructor.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private EventBinder()
        { }

        /// <summary>
        /// Declared Events attached property.
        /// </summary>
        public static readonly AttachedProperty<EventsCollection?> EventsProperty =
            AvaloniaProperty.RegisterAttached<EventBinder, AvaloniaObject, EventsCollection?>(
                "Events",
                default,
                false,
                BindingMode.OneTime);

        /// <summary>
        /// The type initializer.
        /// </summary>
        static EventBinder() =>
            EventsProperty.Changed.Subscribe(e =>
            {
                if (!e.OldValue.Equals(e.NewValue))
                {
                    if (e.OldValue.GetValueOrDefault() is EventsCollection oec)
                    {
                        oec.Detach();
                    }
                    if (e.NewValue.GetValueOrDefault() is EventsCollection nec)
                    {
                        nec.Attach(e.Sender);
                    }
                }
            });

        /// <summary>
        /// Get EventsCollection instance.
        /// </summary>
        public static EventsCollection? GetEvents(DependencyObject d)
        {
            var collection = d.GetValue(EventsProperty);
            if (collection == null)
            {
                // Self generated.
                collection = new EventsCollection();
                d.SetValue(EventsProperty, collection);
            }
            return collection;
        }

        /// <summary>
        /// Set EventsCollection instance.
        /// </summary>
        /// <remarks>It's used internal only.
        // It's required for Avalonia XAML compiler.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetEvents(DependencyObject d, EventsCollection? value) =>
            d.SetValue(EventsProperty, value ?? new EventsCollection());
#else
        /// <summary>
        /// Declared Events attached property.
        /// </summary>
        public static readonly DependencyProperty EventsProperty =
            DependencyProperty.RegisterAttached(
#if UNO || OPENSILVER
                "Events",
#else
                "ShadowEvents",
#endif
                typeof(EventsCollection),
                typeof(EventBinder),
                new PropertyMetadata(null, (d, e) =>
                {
                    if (!object.ReferenceEquals(e.OldValue, e.NewValue))
                    {
                        if (e.OldValue is EventsCollection oec)
                        {
                            oec.Detach();
                        }
                        if (e.NewValue is EventsCollection nec)
                        {
                            nec.Attach(d);
                        }
                    }
                }));

        /// <summary>
        /// Get EventsCollection instance.
        /// </summary>
        public static EventsCollection? GetEvents(DependencyObject d)
        {
            var collection = (EventsCollection?)d.GetValue(EventsProperty);
            if (collection == null)
            {
                // Self generated.
                collection = new EventsCollection();
                d.SetValue(EventsProperty, collection);
            }
            return collection;
        }
#endif
    }

    /// <summary>
    /// Event declaration holding collection class.
    /// </summary>
    /// <remarks>It will be implicitly used on the XAML code.
    /// 
    /// See EventBinder guide: https://github.com/kekyo/Epoxy#eventbinder</remarks>
#if WINDOWS_UWP || UNO
    [Windows.UI.Xaml.Data.Bindable]
#endif
    public sealed class EventsCollection :
        AttachableCollection<EventsCollection, Event>
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EventsCollection()
        { }
    }

    /// <summary>
    /// EventBinder's event declaration class.
    /// </summary>
    /// <remarks>See EventBinder guide: https://github.com/kekyo/Epoxy#eventbinder</remarks>
    /// <example>
    /// <code>
    /// &lt;Window xmlns:epoxy="https://github.com/kekyo/Epoxy"&gt;
    ///    &lt;!-- ... --&gt;
    ///    &lt;epoxy:EventBinder.Events&gt;
    ///         &lt;!-- Binding the Window.Loaded event to the ViewModel's Ready property --&gt;
    ///         &lt;epoxy:Event Name = "Loaded" Command="{Binding Ready}" /&gt;
    ///    &lt;/epoxy:EventBinder.Events&gt;
    /// &lt;/Window&gt;
    /// </code>
    /// </example>
#if WINDOWS_UWP || UNO
    [Windows.UI.Xaml.Data.Bindable]
#endif
    public sealed class Event :
        AttachableObject<Event>
    {
#if XAMARIN_FORMS
        /// <summary>
        /// Binds target CLR event name bindable property declaration.
        /// </summary>
        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(
                "Name",
                typeof(string),
                typeof(Event),
                null,
                BindingMode.Default,
                null,
                (d, ov, nv) => ((Event)d).OnNamePropertyChanged(ov, nv));

        /// <summary>
        /// Binds ICommand expression bindable property declaration.
        /// </summary>
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                "Command",
                typeof(ICommand),
                typeof(Event),
                null,
                BindingMode.Default,
                null,
                (d, _, nv) => ((Event)d).OnCommandPropertyChanged(nv));
#elif AVALONIA
        /// <summary>
        /// Binds target CLR event name bindable property declaration.
        /// </summary>
        public static readonly AvaloniaProperty<string> NameProperty =
            AvaloniaProperty.Register<Event, string>("Name");

        /// <summary>
        /// Binds ICommand expression bindable property declaration.
        /// </summary>
        public static readonly AvaloniaProperty<ICommand> CommandProperty =
            AvaloniaProperty.Register<Event, ICommand>("Command");

        /// <summary>
        /// The type initializer.
        /// </summary>
        static Event()
        {
            NameProperty.Changed.Subscribe(e => ((Event)e.Sender).OnNamePropertyChanged(e.OldValue, e.NewValue));
            CommandProperty.Changed.Subscribe(e => ((Event)e.Sender).OnCommandPropertyChanged(e.NewValue));
        }
#else
        /// <summary>
        /// Binds target CLR event name bindable property declaration.
        /// </summary>
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(
                "Name",
                typeof(string),
                typeof(Event),
                new PropertyMetadata(null, (d, e) => ((Event)d).OnNamePropertyChanged(e.OldValue, e.NewValue)));

        /// <summary>
        /// Binds ICommand expression bindable property declaration.
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(Event),
                new PropertyMetadata(null, (d, e) => ((Event)d).OnCommandPropertyChanged(e.NewValue)));
#endif

        /// <summary>
        /// The constructor.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Event()
        { }

        /// <summary>
        /// Binds target CLR event name.
        /// </summary>
        public string? Name
        {
            get => (string?)this.GetValue(NameProperty);
            set => this.SetValue(NameProperty, value);
        }

        /// <summary>
        /// Binds ICommand expression.
        /// </summary>
        public ICommand? Command
        {
            get => (ICommand?)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        private Delegate? Handler { get; set; }

        private void RemoveHandler(object? associatedObject, object? name)
        {
            if (associatedObject is DependencyObject ao &&
                name is string n &&
                this.Handler is { } oh)
            {
                var type = ao.GetType();
                if (EventMetadata.GetOrAddEventInfo(type, n) is { } ei)
                {
                    EventMetadata.RemoveEvent(ei, ao, oh);
                    this.Handler = null;
                }
            }
        }

        private void AddHandler(object? associatedObject, object? name, object? command)
        {
            if (associatedObject is DependencyObject ao &&
                name is string n &&
                command is ICommand c)
            {
                var type = ao.GetType();
                if (EventMetadata.GetOrAddEventInfo(type, n) is { } ei)
                {
                    var nh = EventMetadata.CreateHandler(ei, c);
                    EventMetadata.AddEvent(ei, ao, nh);
                    this.Handler = nh;
                }
            }
        }

        private void OnNamePropertyChanged(object? oldName, object? newName)
        {
            this.RemoveHandler(this.AssociatedObject, oldName);
            this.AddHandler(this.AssociatedObject, newName, this.Command);
        }

        private void OnCommandPropertyChanged(object? newCommand)
        {
            this.RemoveHandler(this.AssociatedObject, this.Name);
            this.AddHandler(this.AssociatedObject, this.Name, newCommand);
        }

        /// <summary>
        /// Attach a parent element.
        /// </summary>
        protected override void OnAttached()
        {
            this.RemoveHandler(this.AssociatedObject, this.Name);
            this.AddHandler(this.AssociatedObject, this.Name, this.Command);
        }

        /// <summary>
        /// Detach already attached parent element.
        /// </summary>
        protected override void OnDetaching()
        {
            this.RemoveHandler(this.AssociatedObject, this.Name);
        }
    }
}
