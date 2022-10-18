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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace Epoxy.Supplemental;

public abstract class LogicalTreeObjectCollection<TObject> :
    FreezableCollection<TObject>
    where TObject : Freezable
{
    private readonly List<TObject> snapshot = new List<TObject>();

    internal LogicalTreeObjectCollection() =>
        ((INotifyCollectionChanged)this).CollectionChanged += this.OnCollectionChanged;

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs? e)
    {
        switch (e!.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (TObject? element1 in e.NewItems!)
                {
                    this.snapshot.Insert(IndexOf(element1!), element1!);
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
                        this.snapshot.Remove(element2!);
                    }
                }
                foreach (TObject? element3 in e.NewItems!)
                {
                    this.snapshot.Insert(IndexOf(element3!), element3!);
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

    protected virtual void OnAdded(TObject element)
    {
    }

    protected virtual void OnRemoving(TObject element)
    {
    }
}

public class LogicalTreeObjectCollection<TSelf, TObject> :
    LogicalTreeObjectCollection<TObject>
    where TObject : Freezable
    where TSelf : LogicalTreeObjectCollection<TObject>, new()
{
    protected sealed override Freezable? CreateInstanceCore() =>
        new TSelf();
}
