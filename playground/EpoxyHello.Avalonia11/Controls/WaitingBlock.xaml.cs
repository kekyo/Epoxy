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

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Epoxy;
using Epoxy.Supplemental;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace EpoxyHello.Avalonia11.Controls;

/// <summary>
/// Interaction logic for WaitingBlock.xaml
/// </summary>
public sealed partial class WaitingBlock : UserControl
{
    public static readonly AvaloniaProperty CellBrushesProperty =
        AvaloniaProperty.Register<WaitingBlock, ObservableCollection<IImmutableSolidColorBrush>?>(
            "CellBrushes");

    private int currentPosition;
    private Timer timer;

    public WaitingBlock()
    {
        InitializeComponent();
        this.DataContext = this;

        this.CellBrushes = Enumerable.Range(0, 8).
            Select(_ => Brushes.Gray).
            ToObservableCollection();

        this.timer = new Timer(async _ =>
            {
                if (await UIThread.TryBind())
                {
                    this.CellBrushes[this.currentPosition] = Brushes.Gray;
                    if (++this.currentPosition >= this.CellBrushes.Count)
                    {
                        this.currentPosition = 0;
                    }
                    this.CellBrushes[this.currentPosition] = Brushes.Red;
                }
            },
            null,
            TimeSpan.Zero,
            TimeSpan.Zero);
    }

    private void InitializeComponent() =>
        AvaloniaXamlLoader.Load(this);

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        this.timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(300));
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        this.timer.Change(TimeSpan.Zero, TimeSpan.Zero);
        base.OnDetachedFromVisualTree(e);
    }

    public ObservableCollection<IImmutableSolidColorBrush>? CellBrushes
    {
        get => (ObservableCollection<IImmutableSolidColorBrush>?)GetValue(CellBrushesProperty);
        set => SetValue(CellBrushesProperty, value);
    }
}
