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
using System.Collections.Specialized;
using System.ComponentModel;

using Windows.Foundation.Collections;
using Windows.UI.Xaml;

namespace Epoxy.Supplemental
{
    // DANGER: The implementation is very fragile. You may die if refactor this.
    public abstract class LogicalTreeObjectCollection<TObject> :
        DependencyObjectCollection, IList<TObject>, INotifyPropertyChanged, INotifyCollectionChanged
#if WINDOWS_UWP
        where TObject : DependencyObject
#else
        where TObject : class, DependencyObject
#endif
    {
        private readonly List<TObject> snapshot = new List<TObject>();

        internal LogicalTreeObjectCollection() =>
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

        protected virtual void OnAdded(TObject element)
        {
        }

        protected virtual void OnRemoving(TObject element)
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

#if WINDOWS_UWP
        public int Count =>
            ((IList<DependencyObject>)this).Count;

        bool ICollection<TObject>.IsReadOnly =>
            ((IList<DependencyObject>)this).IsReadOnly;

        public TObject this[int index]
        {
            get => (TObject)((IList<DependencyObject>)this)[index];
            set => ((IList<DependencyObject>)this)[index] = value;
        }

        public int IndexOf(TObject item) =>
            ((IList<DependencyObject>)this).IndexOf(item);

        public void Insert(int index, TObject item) =>
            ((IList<DependencyObject>)this).Insert(index, item);

        public void RemoveAt(int index) =>
            ((IList<DependencyObject>)this).RemoveAt(index);

        public void Add(TObject item) =>
            ((IList<DependencyObject>)this).Add(item);

        public void Clear() =>
            ((IList<DependencyObject>)this).Clear();

        public bool Contains(TObject item) =>
            ((IList<DependencyObject>)this).Contains(item);

        public void CopyTo(TObject[] array, int arrayIndex) =>
            ((IList<DependencyObject>)this).CopyTo(array, arrayIndex);

        public bool Remove(TObject item) =>
            ((IList<DependencyObject>)this).Remove(item);
#else
        public new TObject this[int index]
        {
            get => (TObject)base[index];
            set => base[index] = value;
        }

        public int IndexOf(TObject item) =>
            base.IndexOf(item);

        public void Insert(int index, TObject item) =>
            base.Insert(index, item);

        public void Add(TObject item) =>
            base.Add(item);

        public bool Contains(TObject item) =>
            base.Contains(item);

        public void CopyTo(TObject[] array, int arrayIndex) =>
            base.CopyTo(array, arrayIndex);

        public bool Remove(TObject item) =>
            base.Remove(item);
#endif

        public
#if !WINDOWS_UWP
            new
#endif
            IEnumerator<TObject> GetEnumerator()
        {
            var list = (IList<DependencyObject>)this;
            for (var index = 0; index < list.Count; index++)
            {
                yield return (TObject)list[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            this.GetEnumerator();
    }

    public class LogicalTreeObjectCollection<TSelf, TObject> :
        LogicalTreeObjectCollection<TObject>
#if WINDOWS_UWP
        where TObject : DependencyObject
#else
        where TObject : class, DependencyObject
#endif
        where TSelf : LogicalTreeObjectCollection<TObject>, new()
    {
    }
}
