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

#if WINDOWS_UWP || UNO
using Windows.UI.Xaml;
#endif

#if WINUI
using Microsoft.UI.Xaml;
#endif

#if WINDOWS_WPF || OPENSILVER
using System.Windows;
#endif

#if XAMARIN_FORMS
using Xamarin.Forms;
using DependencyObject = Xamarin.Forms.BindableObject;
#endif

#if MAUI
using Microsoft.Maui.Controls;
using DependencyObject = Microsoft.Maui.Controls.BindableObject;
#endif

#if AVALONIA
using Avalonia;
using DependencyObject = Avalonia.IAvaloniaObject;
#endif

#if AVALONIA11
using Avalonia;
using DependencyObject = Avalonia.AvaloniaObject;
#endif

namespace Epoxy;

/// <summary>
/// The Well is used with Fountation, there will bind and can transfer .NET event signal.
/// </summary>
/// <remarks>See Fountain guide: https://github.com/kekyo/Epoxy#fountain</remarks>
/// <example>
/// <code>
/// // Declared a Well into the ViewModel.
/// this.ReadyWell = Well.Factory.Create<Window>("Loaded", async () =&gt;
/// {
///     // Event received.
/// });
/// </code>
/// </example>
[DebuggerStepThrough]
public static class WellFactoryExtension
{
    /// <summary>
    /// Create a Well.
    /// </summary>
    /// <typeparam name="TDependencyObject">Target control type</typeparam>
    /// <param name="factory">Factory instance (not used)</param>
    /// <param name="eventName">Event name</param>
    /// <param name="action">Action handler</param>
    /// <returns>Well instance</returns>
    public static Well<TDependencyObject> Create<TDependencyObject>(
        this WellFactoryInstance factory,
        string eventName,
        Func<ValueTask> action)
        where TDependencyObject : DependencyObject =>
        new Well<TDependencyObject>(eventName, action);

    /// <summary>
    /// Create a Well.
    /// </summary>
    /// <typeparam name="TDependencyObject">Target control type</typeparam>
    /// <typeparam name="TEventArgs">Additional parameter type</typeparam>
    /// <param name="factory">Factory instance (not used)</param>
    /// <param name="eventName">Event name</param>
    /// <param name="action">Action handler</param>
    /// <returns>Well instance</returns>
    public static Well<TDependencyObject, TEventArgs> Create<TDependencyObject, TEventArgs>(
        this WellFactoryInstance factory,
        string eventName,
        Func<TEventArgs, ValueTask> action)
        where TDependencyObject : DependencyObject =>
        new Well<TDependencyObject, TEventArgs>(eventName, action);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Well<TDependencyObject> Create<TDependencyObject>(
        this WellFactoryInstance factory,
        string eventName,
        Func<ValueTask> action,
        Action<TDependencyObject, object, IntPtr> adder,
        Action<TDependencyObject, object, IntPtr> remover)
        where TDependencyObject : DependencyObject =>
        new Well<TDependencyObject>(eventName, action, adder, remover);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Well<TDependencyObject, TEventArgs> Create<TDependencyObject, TEventArgs>(
        this WellFactoryInstance factory,
        string eventName,
        Func<TEventArgs, ValueTask> action,
        Action<TDependencyObject, object, IntPtr> adder,
        Action<TDependencyObject, object, IntPtr> remover)
        where TDependencyObject : DependencyObject =>
        new Well<TDependencyObject, TEventArgs>(eventName, action, adder, remover);
}
