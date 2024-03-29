﻿////////////////////////////////////////////////////////////////////////////
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

using Epoxy.Internal;

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Epoxy.Supplemental;

[DebuggerStepThrough]
public struct UIThreadAwaitable
{
    public UIThreadAwaiter GetAwaiter() =>
        new UIThreadAwaiter();
}

[DebuggerStepThrough]
public sealed class UIThreadAwaiter : INotifyCompletion, ICriticalNotifyCompletion
{
    private bool isBound;

    internal UIThreadAwaiter()
    { }

    public bool IsCompleted { get; private set; }

    public void OnCompleted(Action continuation) =>
        InternalUIThread.ContinueOnUIThread(isBound =>
        {
            this.isBound = isBound;
            this.IsCompleted = true;
            continuation();
        });

    public void UnsafeOnCompleted(Action continuation) =>
        InternalUIThread.ContinueOnUIThread(isBound =>
        {
            this.isBound = isBound;
            this.IsCompleted = true;
            continuation();
        });

    public void GetResult()
    {
        Debug.Assert(this.IsCompleted);
        if (!this.isBound)
        {
            throw new InvalidOperationException(
                "Epoxy: Could not bind to UI thread. UI thread is not found.");
        }
    }
}

[DebuggerStepThrough]
public struct UIThreadTryBindAwaitable
{
    public UIThreadTryBindAwaiter GetAwaiter() =>
        new UIThreadTryBindAwaiter();
}

[DebuggerStepThrough]
public sealed class UIThreadTryBindAwaiter : INotifyCompletion, ICriticalNotifyCompletion
{
    private bool isBound;

    internal UIThreadTryBindAwaiter()
    { }

    public bool IsCompleted { get; private set; }

    public void OnCompleted(Action continuation) =>
        InternalUIThread.ContinueOnUIThread(isBound =>
        {
            this.isBound = isBound;
            this.IsCompleted = true;
            continuation();
        });

    public void UnsafeOnCompleted(Action continuation) =>
        InternalUIThread.ContinueOnUIThread(isBound =>
        {
            this.isBound = isBound;
            this.IsCompleted = true;
            continuation();
        });

    public bool GetResult()
    {
        Debug.Assert(this.IsCompleted);
        return this.isBound;
    }
}

[DebuggerStepThrough]
public struct UIThreadUnbindAwaitable
{
    public UIThreadUnbindAwaiter GetAwaiter() =>
        new UIThreadUnbindAwaiter();
}

[DebuggerStepThrough]
public sealed class UIThreadUnbindAwaiter : INotifyCompletion, ICriticalNotifyCompletion
{
    internal UIThreadUnbindAwaiter()
    { }

    public bool IsCompleted { get; private set; }

    public void OnCompleted(Action continuation) =>
        InternalUIThread.ContinueOnWorkerThread(() =>
        {
            this.IsCompleted = true;
            continuation();
        });

    public void UnsafeOnCompleted(Action continuation) =>
        InternalUIThread.ContinueOnWorkerThread(() =>
        {
            this.IsCompleted = true;
            continuation();
        });

    public void GetResult() =>
        Debug.Assert(this.IsCompleted);
}
