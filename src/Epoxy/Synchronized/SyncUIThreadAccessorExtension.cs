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
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Epoxy.Synchronized;

/// <summary>
/// UI thread commonly manipulator.
/// </summary>
[DebuggerStepThrough]
public static class SyncUIThreadAccessorExtension
{
    /// <summary>
    /// Execute synchronously delegate on the UI thread context.
    /// </summary>
    /// <param name="accessor">UIThread accessor</param>
    /// <param name="action">Action on UI thread context</param>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static async ValueTask InvokeSyncOnAsync(
        this UIThreadAccessorInstance accessor, Action action)
    {
        await UIThread.Bind();
        action();
    }

    /// <summary>
    /// Execute synchronously delegate on the UI thread context.
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="accessor">UIThread accessor</param>
    /// <param name="action">Action on UI thread context</param>
    /// <returns>Result</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static async ValueTask<T> InvokeSyncOnAsync<T>(
        this UIThreadAccessorInstance accessor, Func<T> action)
    {
        await UIThread.Bind();
        return action();
    }

    /// <summary>
    /// Execute synchronously delegate on the UI thread context.
    /// </summary>
    /// <param name="accessor">UIThread accessor</param>
    /// <param name="action">Action on UI thread context</param>
    /// <returns>True if executed.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static async ValueTask<bool> TryInvokeSyncOnAsync(
        this UIThreadAccessorInstance accessor, Action action)
    {
        if (await UIThread.TryBind())
        {
            action();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Execute synchronously delegate on the UI thread context.
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="accessor">UIThread accessor</param>
    /// <param name="action">Action on UI thread context</param>
    /// <returns>True if executed.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static async ValueTask<TryInvokeAsyncResult<T>> TryInvokeSyncOnAsync<T>(
        this UIThreadAccessorInstance accessor, Func<T> action)
    {
        if (await UIThread.TryBind())
        {
            return new TryInvokeAsyncResult<T>(true, action());
        }
        else
        {
            return new TryInvokeAsyncResult<T>(false, default!);
        }
    }
}
