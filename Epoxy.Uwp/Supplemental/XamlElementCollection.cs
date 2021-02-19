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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using Windows.Foundation.Collections;
using Windows.UI.Xaml;

namespace Epoxy.Supplemental
{
    public class XamlElementCollection<TElement> :
        DependencyObjectCollection, INotifyPropertyChanged, INotifyCollectionChanged
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

        protected void ReadPreamble()
        { }
        protected void WritePreamble()
        { }
        protected void WritePostscript()
        { }
    }

    public class XamlElementCollection<TSelf, TElement> :
        XamlElementCollection<TElement>
        where TElement : DependencyObject
        where TSelf : XamlElementCollection<TElement>, new()
    {
    }
}
