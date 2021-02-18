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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

#if WINDOWS_UWP
using System.Linq;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
#endif

#if WINDOWS_WPF
using System.Windows;
#endif

#if XAMARIN_FORMS
using System.Collections.ObjectModel;
using Xamarin.Forms;
#endif

namespace Epoxy.Supplemental
{
#if WINDOWS_WPF
    public class XamlElementCollection<TElement> :
        FreezableCollection<TElement>
        where TElement : Freezable
    {
        private readonly List<TElement> snapshot = new List<TElement>();

        public XamlElementCollection() =>
            ((INotifyCollectionChanged)this).CollectionChanged += this.OnCollectionChanged;

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs? e)
        {
            switch (e!.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TElement? element1 in e.NewItems!)
                    {
                        this.snapshot.Insert(IndexOf(element1!), element1!);
                        this.OnAdded(element1!);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (TElement? element2 in e.OldItems!)
                    {
                        try
                        {
                            this.OnRemoving(element2!);
                        }
                        finally
                        {
                            this.snapshot.Remove(element2!);
                        }
                    }
                    foreach (TElement? element3 in e.NewItems!)
                    {
                        this.snapshot.Insert(IndexOf(element3!), element3!);
                        this.OnAdded(element3!);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (TElement? element4 in e.OldItems!)
                    {
                        try
                        {
                            this.OnRemoving(element4!);
                        }
                        finally
                        {
                            this.snapshot.Remove(element4!);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var element5 in this.snapshot)
                    {
                        try
                        {
                            this.OnRemoving(element5!);
                        }
                        finally
                        {
                            this.snapshot.Remove(element5!);
                        }
                    }
                    this.snapshot.Clear();
                    foreach (var element6 in this)
                    {
                        this.snapshot.Insert(IndexOf(element6!), element6!);
                        this.OnAdded(element6!);
                    }
                    break;
            }
        }

        protected virtual void OnAdded(TElement element)
        {
        }

        protected virtual void OnRemoving(TElement element)
        {
        }

        protected override Freezable? CreateInstanceCore() =>
            (Freezable)Activator.CreateInstance(this.GetType())!;
    }
#endif

#if WINDOWS_UWP
    public class XamlElementCollection<TElement> :
        DependencyObjectCollection, IList<TElement>, INotifyPropertyChanged, INotifyCollectionChanged
        where TElement : DependencyObject
    {
        private readonly List<TElement> snapshot = new List<TElement>();

        public XamlElementCollection() =>
            base.VectorChanged += this.OnVectorChanged;

        private void OnVectorChanged(IObservableVector<DependencyObject> sender, IVectorChangedEventArgs e)
        {
            switch (e.CollectionChange)
            {
                case CollectionChange.ItemInserted:
                    var newElement1 = this[(int)e.Index];
                    this.snapshot.Insert((int)e.Index, newElement1);
                    try
                    {
                        this.OnAdded(newElement1);
                    }
                    finally
                    {
                        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
                        this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Add, newElement1, (int)e.Index));
                    }
                    break;
                case CollectionChange.ItemChanged:
                    var oldElement2 = this.snapshot[(int)e.Index];
                    var newElement2 = this[(int)e.Index];
                    try
                    {
                        this.OnRemoving(oldElement2);
                    }
                    finally
                    {
                        this.snapshot[(int)e.Index] = newElement2;
                        try
                        {
                            this.OnAdded(newElement2);
                        }
                        finally
                        {
                            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
                            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Replace, newElement2, (int)e.Index));
                        }
                    }
                    break;
                case CollectionChange.ItemRemoved:
                    var oldElement3 = this.snapshot[(int)e.Index];
                    try
                    {
                        this.OnRemoving(oldElement3);
                    }
                    finally
                    {
                        this.snapshot.RemoveAt((int)e.Index);
                        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
                        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
                        this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Remove, oldElement3, (int)e.Index));
                    }
                    break;
            }
        }

        protected virtual void OnAdded(TElement element)
        {
        }

        protected virtual void OnRemoving(TElement element)
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public int Count =>
            ((ICollection<DependencyObject>)this).Count;

        bool ICollection<TElement>.IsReadOnly =>
            ((ICollection<DependencyObject>)this).IsReadOnly;

        public TElement this[int index]
        {
            get => (TElement)((IList<DependencyObject>)this)[index];
            set => ((IList<DependencyObject>)this)[index] = value;
        }

        public int IndexOf(TElement item) =>
            ((IList<DependencyObject>)this).IndexOf(item);

        public void Insert(int index, TElement item) =>
            ((IList<DependencyObject>)this).Insert(index, item);

        public void RemoveAt(int index) =>
            ((IList<DependencyObject>)this).RemoveAt(index);

        public void Add(TElement item) =>
            ((ICollection<DependencyObject>)this).Add(item);

        public void Clear() =>
            ((ICollection<DependencyObject>)this).Clear();

        public bool Contains(TElement item) =>
            ((ICollection<DependencyObject>)this).Contains(item);

        public void CopyTo(TElement[] array, int arrayIndex) =>
            ((ICollection<DependencyObject>)this).CopyTo(array, arrayIndex);

        public bool Remove(TElement item) =>
            ((ICollection<DependencyObject>)this).Remove(item);

        public IEnumerator<TElement> GetEnumerator() =>
            ((IEnumerable<DependencyObject>)this).Cast<TElement>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable<DependencyObject>)this).GetEnumerator();

        protected void ReadPreamble()
        { }
        protected void WritePreamble()
        { }
        protected void WritePostscript()
        { }
    }
#endif

#if XAMARIN_FORMS
    public class XamlElementCollection<TElement> :
        Element, IList<TElement>, INotifyPropertyChanged, INotifyCollectionChanged
        where TElement : Element
    {
        private readonly ObservableCollection<TElement> collection =
            new ObservableCollection<TElement>();
        private readonly List<TElement> snapshot =
            new List<TElement>();

        public XamlElementCollection()
        {
            ((INotifyPropertyChanged)this.collection).PropertyChanged += (s, e) =>
                this.OnPropertyChanged(e.PropertyName);
            this.collection.CollectionChanged += this.OnCollectionChanged;
        }

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs? e)
        {
            switch (e!.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TElement? element1 in e.NewItems!)
                    {
                        this.snapshot.Insert(IndexOf(element1!), element1!);
                        element1!.Parent = this;
                        this.OnAdded(element1!);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (TElement? element2 in e.OldItems!)
                    {
                        try
                        {
                            this.OnRemoving(element2!);
                        }
                        finally
                        {
                            element2!.Parent = null;
                            this.snapshot.Remove(element2!);
                        }
                    }
                    foreach (TElement? element3 in e.NewItems!)
                    {
                        this.snapshot.Insert(IndexOf(element3!), element3!);
                        element3!.Parent = null;
                        this.OnAdded(element3!);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (TElement? element4 in e.OldItems!)
                    {
                        try
                        {
                            this.OnRemoving(element4!);
                        }
                        finally
                        {
                            element4!.Parent = null;
                            this.snapshot.Remove(element4!);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var element5 in this.snapshot)
                    {
                        try
                        {
                            this.OnRemoving(element5!);
                        }
                        finally
                        {
                            element5!.Parent = null;
                        }
                    }
                    this.snapshot.Clear();
                    foreach (var element6 in this)
                    {
                        this.snapshot.Insert(IndexOf(element6!), element6!);
                        element6!.Parent = this;
                        this.OnAdded(element6!);
                    }
                    break;
            }

            this.CollectionChanged?.Invoke(this, e);
        }
        
        protected virtual void OnAdded(TElement element)
        {
        }

        protected virtual void OnRemoving(TElement element)
        {
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public int Count =>
            this.collection.Count;

        bool ICollection<TElement>.IsReadOnly =>
            false;

        public TElement this[int index]
        {
            get => this.collection[index];
            set => this.collection[index] = value;
        }

        public int IndexOf(TElement item) =>
            this.collection.IndexOf(item);

        public void Insert(int index, TElement item) =>
            this.collection.Insert(index, item);

        public void RemoveAt(int index) =>
            this.collection.RemoveAt(index);

        public void Add(TElement item) =>
            this.collection.Add(item);

        public void Clear() =>
            this.collection.Clear();

        public bool Contains(TElement item) =>
            this.collection.Contains(item);

        public void CopyTo(TElement[] array, int arrayIndex) =>
            this.collection.CopyTo(array, arrayIndex);

        public bool Remove(TElement item) =>
            this.collection.Remove(item);

        public IEnumerator<TElement> GetEnumerator() =>
            this.collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            this.collection.GetEnumerator();

        protected void ReadPreamble()
        { }
        protected void WritePreamble()
        { }
        protected void WritePostscript()
        { }
    }
#endif

    public class XamlElementCollection<TSelf, TElement> :
        XamlElementCollection<TElement>
#if WINDOWS_WPF
        where TElement : Freezable
#endif
#if WINDOWS_UWP
        where TElement : DependencyObject
#endif
#if XAMARIN_FORMS
        where TElement : Element
#endif
        where TSelf : XamlElementCollection<TElement>, new()
    {
#if WINDOWS_WPF
        protected override sealed Freezable? CreateInstanceCore() =>
            new TSelf();
#endif
    }
}
