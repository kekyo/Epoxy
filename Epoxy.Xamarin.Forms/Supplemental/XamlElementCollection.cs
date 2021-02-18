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

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using Xamarin.Forms;

namespace Epoxy.Supplemental
{
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

    public class XamlElementCollection<TSelf, TElement> :
        XamlElementCollection<TElement>
        where TElement : Element
        where TSelf : XamlElementCollection<TElement>, new()
    {
    }
}
