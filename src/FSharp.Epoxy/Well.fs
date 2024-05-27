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

#if WINDOWS_UWP || UNO
open Windows.UI.Xaml;
#endif

#if WINUI
open Microsoft.UI.Xaml;
#endif

#if WINDOWS_WPF || OPENSILVER
open System.Windows;
#endif

#if XAMARIN_FORMS
open Xamarin.Forms;
#endif

#if MAUI
open Microsoft.Maui.Controls;
#endif

#if AVALONIA || AVALONIA11
open Avalonia;
open Avalonia.Interactivity;
#endif

/// <summary>
/// The Well factory.
/// </summary>
[<DebuggerStepThrough>]
[<AutoOpen>]
module public WellExtension =

    type public WellFactoryInstance with

        /// <summary>
        /// Create a Well.
        /// </summary>
        /// <typeparam name="TUIElement">Target control type</typeparam>
        /// <returns>Well instance</returns>
        member _.create<'TUIElement when 'TUIElement :> UIElement>() =
            new Well<'TUIElement>()
                
    /////////////////////////////////////////////////////////////////

    type public Well<'TUIElement when 'TUIElement :> UIElement> with

        /// <summary>
        /// Add a well handler.
        /// </summary>
        /// <typeparam name="TEventArgs">Handler receive type</typeparam>
        /// <param name="well">Well</param>
        /// <param name="eventName">Event name</param>
        /// <param name="action">Action delegate</param>
        member well.add<'TEventArgs>(
            eventName: string,
            action: 'TEventArgs -> Async<unit>) =
                well.InternalAdd(
                    eventName,
                    action >> asyncUnitAsValueTaskVoid)

        /// <summary>
        /// Add a well handler.
        /// </summary>
        /// <param name="well">Well</param>
        /// <param name="eventName">Event name</param>
        /// <param name="action">Action delegate</param>
        member well.add(
            eventName: string,
            action: unit -> Async<unit>) =
                well.InternalAdd<EventArgs>(
                    eventName,
                    fun _ -> (action() |> asyncUnitAsValueTaskVoid))

        /// <summary>
        /// Remove a well handler.
        /// </summary>
        /// <param name="well">Well</param>
        /// <param name="eventName">Event name</param>
        member well.remove(
            eventName: string) =
                well.InternalRemove(eventName)

#if !(XAMARIN_FORMS || MAUI)
        /// <summary>
        /// Add a well handler.
        /// </summary>
        /// <typeparam name="TEventArgs">Handler receive type</typeparam>
        /// <param name="well">Well</param>
        /// <param name="routedEvent">RoutedEvent</param>
        /// <param name="action">Action delegate</param>
        member well.add<'TEventArgs when 'TEventArgs :> RoutedEventArgs>(
#if AVALONIA || AVALONIA11
            routedEvent: RoutedEvent<'TEventArgs>,
#else
            routedEvent: RoutedEvent,
#endif
            action: 'TEventArgs -> Async<unit>) =
                well.InternalAdd(
                    routedEvent,
                    action >> asyncUnitAsValueTaskVoid)

        /// <summary>
        /// Add a well handler.
        /// </summary>
        /// <param name="well">Well</param>
        /// <param name="routedEvent">RoutedEvent</param>
        /// <param name="action">Action delegate</param>
        member well.add(
            routedEvent: RoutedEvent,
            action: unit -> Async<unit>) =
                well.InternalAdd<RoutedEventArgs>(
                    routedEvent,
                    fun _ -> (action() |> asyncUnitAsValueTaskVoid))

        /// <summary>
        /// Remove a well handler.
        /// </summary>
        /// <param name="well">Well</param>
        /// <param name="routedEvent">RoutedEvent</param>
        member well.remove(
            routedEvent: RoutedEvent) =
                well.InternalRemove(routedEvent)
#endif

        /////////////////////////////////////////////////////////////////

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        member well.add<'TUIElement, 'TEventArgs when 'TUIElement :> UIElement>(
            eventName: string,
            action: 'TEventArgs -> Async<unit>,
            adder: Action<'TUIElement, obj, IntPtr>,
            remover: Action<'TUIElement, obj, IntPtr>) =
                well.InternalAdd(eventName, action >> asyncUnitAsValueTaskVoid, adder, remover)

        [<EditorBrowsable(EditorBrowsableState.Never)>]
        member well.add<'TUIElement when 'TUIElement :> UIElement>(
            eventName: string,
            action: unit -> Async<unit>,
            adder: Action<'TUIElement, obj, IntPtr>,
            remover: Action<'TUIElement, obj, IntPtr>) =
                well.InternalAdd<EventArgs>(
                    eventName,
                    (fun _ -> (action() |> asyncUnitAsValueTaskVoid)),
                    adder,
                    remover)
