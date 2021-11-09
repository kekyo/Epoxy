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

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;

namespace Epoxy.Supplemental
{
    public class DependencyObjectCollection<TObject> :
        DependencyObjectCollection, IEnumerable<TObject>, INotifyPropertyChanged, INotifyCollectionChanged
        where TObject : DependencyObject
    {
        private readonly List<TObject> snapshot = new List<TObject>();

        public DependencyObjectCollection() =>
            base.VectorChanged += this.OnVectorChanged;

        private void OnVectorChanged(IObservableVector<DependencyObject> sender, IVectorChangedEventArgs e)
        {
            switch (e.CollectionChange)
            {
                case CollectionChange.ItemInserted:
                    var newElement1 = this[(int)e.Index];
                    this.snapshot.Insert((int)e.Index, (TObject)newElement1);
                    try
                    {
                        this.OnAdded((TObject)newElement1);
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
                        this.snapshot[(int)e.Index] = (TObject)newElement2;
                        try
                        {
                            this.OnAdded((TObject)newElement2);
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

        public new IEnumerator<TObject> GetEnumerator()
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

    public class DependencyObjectCollection<TSelf, TObject> :
        DependencyObjectCollection<TObject>
        where TObject : DependencyObject
        where TSelf : DependencyObjectCollection<TObject>, new()
    {
    }
}
