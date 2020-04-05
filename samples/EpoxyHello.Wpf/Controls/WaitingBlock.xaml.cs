////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
// Copyright (c) 2020 Kouji Matsui (@kozy_kekyo, @kekyo2)
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

using Epoxy.Supplemental;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EpoxyHello.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for WaitingBlock.xaml
    /// </summary>
    public sealed partial class WaitingBlock : UserControl
    {
        public static readonly DependencyProperty CellBrushesProperty =
            DependencyProperty.Register(
                "CellBrushes",
                typeof(ObservableCollection<SolidColorBrush>),
                typeof(WaitingBlock));

        private int currentPosition;
        private Timer timer;

        public WaitingBlock()
        {
            InitializeComponent();
            this.DataContext = this;

            this.CellBrushes = Enumerable.Range(0, 8).
                Select(_ => Brushes.Gray).
                ToObservableCollection();

            this.timer = new Timer(_ => Dispatcher.BeginInvoke(
                new EventHandler((s, e) =>
                {
                    this.CellBrushes[this.currentPosition] = Brushes.Gray;
                    if (++this.currentPosition >= this.CellBrushes.Count)
                    {
                        this.currentPosition = 0;
                    }
                    this.CellBrushes[this.currentPosition] = Brushes.Red;
                }), this, EventArgs.Empty),
                null,
                TimeSpan.Zero,
                TimeSpan.Zero);
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            if (this.VisualParent != null)
            {
                this.timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(300));
            }
            else
            {
                this.timer.Change(TimeSpan.Zero, TimeSpan.Zero);
            }
        }

        public ObservableCollection<SolidColorBrush> CellBrushes
        {
            get => (ObservableCollection<SolidColorBrush>)GetValue(CellBrushesProperty);
            set => SetValue(CellBrushesProperty, value);
        }
    }
}
