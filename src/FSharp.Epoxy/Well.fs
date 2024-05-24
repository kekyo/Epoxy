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

namespace Epoxy

open Epoxy
open Epoxy.Internal

open System
open System.ComponentModel
open System.Diagnostics
open System.Threading.Tasks
open System.Runtime.InteropServices

#if WINDOWS_UWP || UNO
open Windows.UI.Xaml
#endif

#if WINUI
open Microsoft.UI.Xaml
#endif

#if WINDOWS_WPF || OPENSILVER
open System.Windows
#endif

/// <summary>
/// The Well factory.
/// </summary>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public WellFactoryExtension =

    type public WellFactoryInstance with

        /// <summary>
        /// Create an anonymous control typed Pile.
        /// </summary>
        /// <typeparam name="TDependencyObject">Target control type</typeparam>
        /// <param name="eventName">Event name</param>
        /// <param name="action">Action</param>
        /// <returns>Well instance</returns>
        member _.create<'TDependencyObject when 'TDependencyObject :> DependencyObject>(
            eventName: string,
            action: unit -> Async<unit>) =
                new Well<'TDependencyObject>(eventName, action >> asyncUnitAsValueTaskVoid)

        /// <summary>
        /// Create a Pile.
        /// </summary>
        /// <typeparam name="TDependencyObject">Target control type</typeparam>
        /// <typeparam name="TEventArgs">Additional parameter type</typeparam>
        /// <param name="eventName">Event name</param>
        /// <param name="action">Action</param>
        /// <returns>Well instance</returns>
        member _.create<'TDependencyObject, 'TEventArgs when 'TDependencyObject :> DependencyObject>(
            eventName: string,
            action: 'TEventArgs -> Async<unit>) =
                new Well<'TDependencyObject, 'TEventArgs>(eventName, action >> asyncUnitAsValueTaskVoid)

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        member _.create<'TDependencyObject when 'TDependencyObject :> DependencyObject>(
            eventName: string,
            action: unit -> Async<unit>,
            adder: Action<'TDependencyObject, obj, IntPtr>,
            remover: Action<'TDependencyObject, obj, IntPtr>) =
                new Well<'TDependencyObject>(eventName, action >> asyncUnitAsValueTaskVoid, adder, remover)

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        member _.create<'TDependencyObject, 'TEventArgs when 'TDependencyObject :> DependencyObject>(
            eventName: string,
            action: 'TEventArgs -> Async<unit>,
            adder: Action<'TDependencyObject, obj, IntPtr>,
            remover: Action<'TDependencyObject, obj, IntPtr>) =
                new Well<'TDependencyObject, 'TEventArgs>(eventName, action >> asyncUnitAsValueTaskVoid, adder, remover)
