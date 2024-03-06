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

using Epoxy;
using System;
using System.Threading;

using Microsoft.Maui.Controls;
using System.Runtime.CompilerServices;

namespace EpoxyHello.Maui.Controls;

/// <summary>
/// Interaction logic for WaitingBlock.xaml
/// </summary>
public sealed partial class WaitingBlock : ContentView
{
    public static readonly BindableProperty CellBrush0Property =
        BindableProperty.Create(nameof(CellBrush0), typeof(Brush), typeof(WaitingBlock), Brush.Gray);
    public static readonly BindableProperty CellBrush1Property =
        BindableProperty.Create(nameof(CellBrush1), typeof(Brush), typeof(WaitingBlock), Brush.Gray);
    public static readonly BindableProperty CellBrush2Property =
        BindableProperty.Create(nameof(CellBrush2), typeof(Brush), typeof(WaitingBlock), Brush.Gray);
    public static readonly BindableProperty CellBrush3Property =
        BindableProperty.Create(nameof(CellBrush3), typeof(Brush), typeof(WaitingBlock), Brush.Gray);
    public static readonly BindableProperty CellBrush4Property =
        BindableProperty.Create(nameof(CellBrush4), typeof(Brush), typeof(WaitingBlock), Brush.Gray);
    public static readonly BindableProperty CellBrush5Property =
        BindableProperty.Create(nameof(CellBrush5), typeof(Brush), typeof(WaitingBlock), Brush.Gray);
    public static readonly BindableProperty CellBrush6Property =
        BindableProperty.Create(nameof(CellBrush6), typeof(Brush), typeof(WaitingBlock), Brush.Gray);
    public static readonly BindableProperty CellBrush7Property =
        BindableProperty.Create(nameof(CellBrush7), typeof(Brush), typeof(WaitingBlock), Brush.Gray);

    private int currentPosition;
    private Timer timer;

    public WaitingBlock()
    {
        this.InitializeComponent();
        this.BindingContext = this;

        this.timer = new Timer(async _ =>
            {
                if (await UIThread.TryBind())
                {
                    for (var index = 0; index < 8; index++)
                    {
                        var brush = index == this.currentPosition ? Brush.Red : Brush.Gray;
                        {
                            switch (index)
                            {
                                case 0:
                                    this.CellBrush0 = brush;
                                    break;
                                case 1:
                                    this.CellBrush1 = brush;
                                    break;
                                case 2:
                                    this.CellBrush2 = brush;
                                    break;
                                case 3:
                                    this.CellBrush3 = brush;
                                    break;
                                case 4:
                                    this.CellBrush4 = brush;
                                    break;
                                case 5:
                                    this.CellBrush5 = brush;
                                    break;
                                case 6:
                                    this.CellBrush6 = brush;
                                    break;
                                case 7:
                                    this.CellBrush7 = brush;
                                    break;
                            }
                        }
                    }

                    if (++this.currentPosition >= 8)
                    {
                        this.currentPosition = 0;
                    }
                }
            },
            null,
            Timeout.InfiniteTimeSpan,
            Timeout.InfiniteTimeSpan);
    }

    protected override void OnPropertyChanging([CallerMemberName] string propertyName = null!)
    {
        if (propertyName == nameof(Parent))
        {
            if (this.Parent != null)
            {
                this.timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            }
        }
        base.OnPropertyChanging(propertyName);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null!)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(Parent))
        {
            if (this.Parent != null)
            {
                this.timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(300));
            }
        }
    }

    public Brush CellBrush0
    {
        get => (Brush?)GetValue(CellBrush0Property) ?? Brush.Gray;
        set => SetValue(CellBrush0Property, value);
    }

    public Brush CellBrush1
    {
        get => (Brush?)GetValue(CellBrush1Property) ?? Brush.Gray;
        set => SetValue(CellBrush1Property, value);
    }

    public Brush CellBrush2
    {
        get => (Brush?)GetValue(CellBrush2Property) ?? Brush.Gray;
        set => SetValue(CellBrush2Property, value);
    }

    public Brush CellBrush3
    {
        get => (Brush?)GetValue(CellBrush3Property) ?? Brush.Gray;
        set => SetValue(CellBrush3Property, value);
    }

    public Brush CellBrush4
    {
        get => (Brush?)GetValue(CellBrush4Property) ?? Brush.Gray;
        set => SetValue(CellBrush4Property, value);
    }

    public Brush CellBrush5
    {
        get => (Brush?)GetValue(CellBrush5Property) ?? Brush.Gray;
        set => SetValue(CellBrush5Property, value);
    }

    public Brush CellBrush6
    {
        get => (Brush?)GetValue(CellBrush6Property) ?? Brush.Gray;
        set => SetValue(CellBrush6Property, value);
    }

    public Brush CellBrush7
    {
        get => (Brush?)GetValue(CellBrush7Property) ?? Brush.Gray;
        set => SetValue(CellBrush7Property, value);
    }
}
