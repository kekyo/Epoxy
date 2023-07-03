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
using System.Linq;

using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.LogicalTree;

using Epoxy.Internal;

namespace Epoxy.Supplemental;

public abstract class LogicalTreeObject :
    AvaloniaObject, ILogical, ISetLogicalParent
{
    private static readonly LogicalTreeObjectCollection<LogicalTreeObject> empty =
        new LogicalTreeObjectCollection<LogicalTreeObject>();

    protected LogicalTreeObject()
    { }

    public event EventHandler<LogicalTreeAttachmentEventArgs>? AttachedToLogicalTree;
    public event EventHandler<LogicalTreeAttachmentEventArgs>? DetachedFromLogicalTree;

    public bool IsAttachedToLogicalTree =>
        this.LogicalParent != null;

    public ILogical? LogicalParent { get; private set; }

    public void SetParent(ILogical? parent)
    {
        if (this.LogicalParent != null)
        {
            if (this.LogicalParent.Traverse(c => c.LogicalParent).
                OfType<ILogicalRoot>().
                FirstOrDefault() is { } root)
            {
                var e = new LogicalTreeAttachmentEventArgs(
                    root,
                    this,
                    this.LogicalParent);
                this.LogicalParent = null;
                this.OnNotifyDetachedFromLogicalTree(e);
            }
            else
            {
                this.LogicalParent = null;
            }
        }
        if (parent != null)
        {
            if (parent.Traverse(c => c.LogicalParent).
                OfType<ILogicalRoot>().
                FirstOrDefault() is { } root)
            {
                var e = new LogicalTreeAttachmentEventArgs(
                    root,
                    this,
                    parent);
                this.LogicalParent = parent;
                this.OnNotifyAttachedToLogicalTree(e);
            }
            else
            {
                this.LogicalParent = parent;
            }
        }
    }

    IAvaloniaReadOnlyList<ILogical> ILogical.LogicalChildren =>
        this.GetLogicalChildren();

    void ILogical.NotifyAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e) =>
        this.OnNotifyAttachedToLogicalTree(e);

    void ILogical.NotifyDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e) =>
        this.OnNotifyDetachedFromLogicalTree(e);

    void ILogical.NotifyResourcesChanged(ResourcesChangedEventArgs e) =>
        this.OnNotifyResourcesChanged(e);

    protected virtual IAvaloniaReadOnlyList<ILogical> GetLogicalChildren() =>
        empty;

    protected virtual void OnNotifyAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e) =>
        this.AttachedToLogicalTree?.Invoke(this, e);

    protected virtual void OnNotifyDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e) =>
        this.DetachedFromLogicalTree?.Invoke(this, e);

    protected virtual void OnNotifyResourcesChanged(ResourcesChangedEventArgs e)
    {
    }
}
