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
using System.Windows.Input;

#if WINDOWS_UWP
using Windows.UI.Xaml;
#endif

#if WINDOWS_WPF
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
using DependencyObject = Xamarin.Forms.BindableObject;
#endif

using Epoxy.Internal;
using Epoxy.Supplemental;

namespace Epoxy
{
    public static class EventBinder
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
        public static readonly BindableProperty EventsProperty =
            EventsPropertyKey.BindableProperty;

        public static EventsCollection? GetEvents(DependencyObject d) =>
            (EventsCollection?)d.GetValue(EventsProperty);
#else
        private static readonly DependencyProperty EventsProperty =
            DependencyProperty.RegisterAttached(
                "ShadowEvents",
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

    public sealed class EventsCollection :
        XamlElementCollection<EventsCollection, Event>
    {
        private DependencyObject? associatedObject;

        public EventsCollection()
        { }

        private DependencyObject? AssociatedObject
        {
            get
            {
                ReadPreamble();
                return this.associatedObject;
            }
        }

        protected override void OnAdded(Event evt)
        {
            if (this.AssociatedObject != null)
            {
                evt.Attach(this.AssociatedObject);
            }
        }

        protected override void OnRemoving(Event evt)
        {
            if (evt.AssociatedObject != null)
            {
                evt.Detach();
            }
        }

        internal void Attach(DependencyObject d)
        {
#if XAMARIN_FORMS
            this.Parent = d as Element;
#endif

            if (d != this.AssociatedObject)
            {
                if (this.AssociatedObject != null)
                {
                    throw new InvalidOperationException();
                }

#if WINDOWS_WPF
                var isInDesignMode = GetValue(DesignerProperties.IsInDesignModeProperty) is bool iidm && iidm;
                if (!isInDesignMode)
#endif
                {
                    WritePreamble();
                    this.associatedObject = d;
                    WritePostscript();
                }

                foreach (var evt in this)
                {
                    evt.Attach(this.AssociatedObject);
                }
            }
        }

        internal void Detach()
        {
            foreach (var evt in this)
            {
                evt.Detach();
            }

            WritePreamble();
            this.associatedObject = null;
            WritePostscript();

#if XAMARIN_FORMS
            this.Parent = null;
#endif
        }
    }

    public sealed class Event :
#if WINDOWS_WPF
        Freezable
#endif
#if WINDOWS_UWP
        DependencyObject
#endif
#if XAMARIN_FORMS
        Element
#endif
    {
#if XAMARIN_FORMS
        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(
                "Name",
                typeof(string),
                typeof(Event),
                null,
                BindingMode.Default,
                null,
                (d, ov, nv) => ((Event)d).OnNamePropertyChanged(ov, nv));

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                "Command",
                typeof(ICommand),
                typeof(Event),
                null,
                BindingMode.Default,
                null,
                (d, _, nv) => ((Event)d).OnCommandPropertyChanged(nv));
#else
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(
                "Name",
                typeof(string),
                typeof(Event),
                new PropertyMetadata(null, (d, e) => ((Event)d).OnNamePropertyChanged(e.OldValue, e.NewValue)));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(Event),
                new PropertyMetadata(null, (d, e) => ((Event)d).OnCommandPropertyChanged(e.NewValue)));
#endif

        public Event()
        { }

#if WINDOWS_WPF
        protected override Freezable CreateInstanceCore() =>
            new Event();
#endif

        internal DependencyObject? AssociatedObject { get; private set; }

        public string? Name
        {
            get => (string?)base.GetValue(NameProperty);
            set => base.SetValue(NameProperty, value);
        }

        public ICommand? Command
        {
            get => (ICommand?)base.GetValue(CommandProperty);
            set => base.SetValue(CommandProperty, value);
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
                    ei.RemoveEventHandler(ao, oh);
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
                    ei.AddEventHandler(ao, nh);
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

        internal void Attach(DependencyObject? associatedObject)
        {
            this.RemoveHandler(this.AssociatedObject, this.Name);
            this.AddHandler(associatedObject, this.Name, this.Command);

            this.AssociatedObject = associatedObject;
        }

        internal void Detach()
        {
            this.RemoveHandler(this.AssociatedObject, this.Name);

            this.AssociatedObject = null;
        }
    }
}
