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

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using Xamarin.Forms;

namespace Epoxy.Supplemental
{
    public abstract class LogicalTreeObjectCollection<TObject> :
        Element, IList<TObject>, INotifyPropertyChanged, INotifyCollectionChanged
        where TObject : Element
    {
        private readonly ObservableCollection<TObject> collection =
            new ObservableCollection<TObject>();
        private readonly List<TObject> snapshot =
            new List<TObject>();

        internal LogicalTreeObjectCollection()
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
                    foreach (TObject? element1 in e.NewItems!)
                    {
                        this.snapshot.Insert(IndexOf(element1!), element1!);
                        element1!.Parent = this;
                        this.OnAdded(element1!);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (TObject? element2 in e.OldItems!)
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
                    foreach (TObject? element3 in e.NewItems!)
                    {
                        this.snapshot.Insert(IndexOf(element3!), element3!);
                        element3!.Parent = null;
                        this.OnAdded(element3!);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (TObject? element4 in e.OldItems!)
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
        
        protected virtual void OnAdded(TObject element)
        {
        }

        protected virtual void OnRemoving(TObject element)
        {
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public int Count =>
            this.collection.Count;

        bool ICollection<TObject>.IsReadOnly =>
            false;

        public TObject this[int index]
        {
            get => this.collection[index];
            set => this.collection[index] = value;
        }

        public int IndexOf(TObject item) =>
            this.collection.IndexOf(item);

        public void Insert(int index, TObject item) =>
            this.collection.Insert(index, item);

        public void RemoveAt(int index) =>
            this.collection.RemoveAt(index);

        public void Add(TObject item) =>
            this.collection.Add(item);

        public void Clear() =>
            this.collection.Clear();

        public bool Contains(TObject item) =>
            this.collection.Contains(item);

        public void CopyTo(TObject[] array, int arrayIndex) =>
            this.collection.CopyTo(array, arrayIndex);

        public bool Remove(TObject item) =>
            this.collection.Remove(item);

        public IEnumerator<TObject> GetEnumerator() =>
            this.collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            this.collection.GetEnumerator();
    }

    public class LogicalTreeObjectCollection<TSelf, TObject> :
        LogicalTreeObjectCollection<TObject>
        where TObject : Element
        where TSelf : LogicalTreeObjectCollection<TObject>, new()
    {
    }
}
