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

namespace EpoxyHello.Wpf.Controls

open Epoxy
open Epoxy.Supplemental

open System
open System.Collections.ObjectModel
open System.Linq
open System.Threading
open System.Windows
open System.Windows.Controls
open System.Windows.Media

/// <summary>
/// Interaction logic for WaitingBlock.xaml
/// </summary>
type public WaitingBlock() as self =
    inherit UserControl()

    [<DefaultValue>]
    val mutable currentPosition: int
    [<DefaultValue>]
    val mutable timer: Timer

    do
        //self.InitializeComponent()
        //self.DataContext <- self

        self.CellBrushes <- Enumerable.Range(0, 8).
            Select(fun _ -> Brushes.Gray).
            ToObservableCollection()

        let timerHandler() =
            do self.CellBrushes.[self.currentPosition] <- Brushes.Gray
            do self.currentPosition <- self.currentPosition + 1
            if self.currentPosition >= self.CellBrushes.Count then
                do self.currentPosition <- 0
            do self.CellBrushes.[self.currentPosition] <- Brushes.Red

        self.timer <- new Timer((fun _ -> self.Dispatcher.invokeAsync(timerHandler) |> ignore), null, TimeSpan.Zero, TimeSpan.Zero)

    static member CellBrushesProperty =
        DependencyProperty.Register(
            "CellBrushes",
            typeof<ObservableCollection<SolidColorBrush>>,
            typeof<WaitingBlock>)

    override self.OnVisualParentChanged oldParent =
        base.OnVisualParentChanged(oldParent)
        if self.VisualParent != null then
            self.timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(300.0)) |> ignore
        else
            self.timer.Change(TimeSpan.Zero, TimeSpan.Zero) |> ignore

    member self.CellBrushes
        with get(): ObservableCollection<SolidColorBrush> =
            self.GetValue(WaitingBlock.CellBrushesProperty) :?> ObservableCollection<SolidColorBrush>
        and set(value: ObservableCollection<SolidColorBrush>) =
            self.SetValue(WaitingBlock.CellBrushesProperty, value)
