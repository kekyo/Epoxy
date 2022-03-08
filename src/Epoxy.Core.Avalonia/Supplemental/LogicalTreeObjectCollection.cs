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

using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace Epoxy.Supplemental
{
    public class LogicalTreeObjectCollection<TObject> :
        LogicalTreeObject, IAvaloniaList<TObject>
        where TObject : ILogical, ISetLogicalParent
    {
        private readonly AvaloniaList<TObject> collection =
            new AvaloniaList<TObject>();
        private readonly List<TObject> snapshot =
            new List<TObject>();

        internal LogicalTreeObjectCollection() =>
            this.collection.CollectionChanged += this.OnCollectionChanged;

        private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs? e)
        {
            switch (e!.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TObject? element1 in e.NewItems!)
                    {
                        this.snapshot.Insert(this.collection.IndexOf(element1!), element1!);
                        element1!.SetParent(this);
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
                            element2!.SetParent(null);
                            this.snapshot.Remove(element2!);
                        }
                    }
                    foreach (TObject? element3 in e.NewItems!)
                    {
                        this.snapshot.Insert(this.collection.IndexOf(element3!), element3!);
                        element3!.SetParent(this);
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
                            element4!.SetParent(null);
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
                            element5!.SetParent(null);
                        }
                    }
                    this.snapshot.Clear();
                    foreach (var element6 in this)
                    {
                        this.snapshot.Insert(this.collection.IndexOf(element6!), element6!);
                        element6!.SetParent(this);
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

        protected override IAvaloniaReadOnlyList<ILogical> GetLogicalChildren() =>
            (IAvaloniaReadOnlyList<ILogical>)this;

        protected override void OnNotifyAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            foreach (var child in this.collection)
            {
                child.NotifyAttachedToLogicalTree(e);
            }
            base.OnNotifyAttachedToLogicalTree(e);
        }

        protected override void OnNotifyDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            foreach (var child in this.collection)
            {
                child.NotifyDetachedFromLogicalTree(e);
            }
            base.OnNotifyDetachedFromLogicalTree(e);
        }

        protected override void OnNotifyResourcesChanged(ResourcesChangedEventArgs e)
        {
            foreach (var child in this.collection)
            {
                child.NotifyResourcesChanged(e);
            }
            base.OnNotifyResourcesChanged(e);
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

        public void AddRange(IEnumerable<TObject> items) =>
            this.collection.AddRange(items);

        public void InsertRange(int index, IEnumerable<TObject> items) =>
            this.collection.InsertRange(index, items);

        public void Move(int oldIndex, int newIndex) =>
            this.collection.Move(oldIndex, newIndex);

        public void MoveRange(int oldIndex, int count, int newIndex) =>
            this.collection.MoveRange(oldIndex, count, newIndex);

        public void RemoveAll(IEnumerable<TObject> items) =>
            this.collection.RemoveAll(items);

        public void RemoveRange(int index, int count) =>
            this.collection.RemoveRange(index, count);
    }

    public class LogicalTreeObjectCollection<TSelf, TObject> :
        LogicalTreeObjectCollection<TObject>
        where TObject : ILogical, ISetLogicalParent
        where TSelf : LogicalTreeObjectCollection<TObject>, new()
    {
    }
}
