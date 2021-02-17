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

using Epoxy.Supplemental;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
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
                BindingMode.OneWay,
                null,
                OnEventsPropertyChanged,
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
#else
        private static readonly DependencyProperty EventsProperty =
            DependencyProperty.RegisterAttached(
                "ShadowEvents",
                typeof(EventsCollection),
                typeof(EventBinder),
                new PropertyMetadata(null, (d, e) => OnEventsPropertyChanged(d, e.OldValue, e.NewValue)));
#endif

        public static EventsCollection? GetEvents(DependencyObject d)
        {
            var collection = (EventsCollection?)d.GetValue(EventsProperty);
            if (collection == null)
            {
                // Self generated.
                collection = new EventsCollection();
                d.SetValue(EventsPropertyKey, collection);
            }
            return collection;
        }

        private static void OnEventsPropertyChanged(DependencyObject d, object? oldValue, object? newValue)
        {
            if (!object.ReferenceEquals(oldValue, newValue))
            {
                if (oldValue is EventsCollection oec)
                {
                    oec.Detach();
                }
                if (newValue is EventsCollection nec)
                {
                    nec.Attach(d);
                }
            }
        }
    }

    public sealed class EventsCollection :
        XamlElementCollection<EventsCollection, Event>
    {
        private List<Event> snapshot = new List<Event>();

        private DependencyObject? associatedObject;

        public EventsCollection() =>
            ((INotifyCollectionChanged)this).CollectionChanged += this.OnCollectionChanged;

        private DependencyObject? AssociatedObject
        {
            get
            {
                ReadPreamble();
                return this.associatedObject;
            }
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs? e)
        {
            void OnAdded(Event evt)
            {
                if (this.AssociatedObject != null)
                {
                    evt.Attach(this.AssociatedObject);
                }
            }

            void OnRemoved(Event evt)
            {
                if (evt.AssociatedObject != null)
                {
                    evt.Detach();
                }
            }

            switch (e!.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Event? evt1 in e.NewItems!)
                    {
                        try
                        {
                            OnAdded(evt1!);
                        }
                        finally
                        {
                            this.snapshot.Insert(IndexOf(evt1!), evt1!);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (Event? evt2 in e.OldItems!)
                    {
                        OnRemoved(evt2!);
                        this.snapshot.Remove(evt2!);
                    }
                    foreach (Event? evt3 in e.NewItems!)
                    {
                        try
                        {
                            OnAdded(evt3!);
                        }
                        finally
                        {
                            this.snapshot.Insert(IndexOf(evt3!), evt3!);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Event? evt4 in e.OldItems!)
                    {
                        OnRemoved(evt4!);
                        this.snapshot.Remove(evt4!);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var evt5 in this.snapshot)
                    {
                        OnRemoved(evt5);
                    }
                    this.snapshot.Clear();
                    foreach (var evt6 in this)
                    {
                        OnAdded(evt6!);
                    }
                    break;
            }
        }

        internal void Attach(DependencyObject d)
        {
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
        }
    }

    internal static class EventMetadata
    {
        private struct EventKey
        {
            public readonly Type Type;
            public readonly string Name;

            public EventKey(Type type, string name)
            {
                this.Type = type;
                this.Name = name;
            }
        }

        private static readonly Dictionary<EventKey, EventInfo?> events =
            new Dictionary<EventKey, EventInfo?>();

        public static EventInfo? GetOrAddEventInfo(Type type, string name)
        {
            var key = new EventKey(type, name);
            if (!events.TryGetValue(key, out var ei))
            {
                ei = type.GetEvent(name);
                events.Add(key, ei);
            }
            if (ei == null)
            {
                throw new ArgumentException($"Couldn't bind event: Type={type.FullName}, Name={name}");
            }
            return ei;
        }

        private sealed class InvokingClosure
        {
            private readonly ICommand command;

            public InvokingClosure(ICommand command) =>
                this.command = command;

            public void Handler(object? sender, object? e)
            {
                if (this.command.CanExecute(e))
                {
                    this.command.Execute(e);
                }
            }
        }

        public static Delegate CreateHandler(EventInfo ei, ICommand command)
        {
            // Limitation:
            //   The closure handler signature valid only standard event style:
            //   `void (object?, object?)`
            //   We can make perfect trampoline by opcode emitter or expression constructor.
            //   It's decline running on the AOT platform...
            var closure = new InvokingClosure(command);
            return new EventHandler(closure.Handler).   // valid with contravariance
                GetMethodInfo()!.
                CreateDelegate(ei.EventHandlerType!, closure);
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
