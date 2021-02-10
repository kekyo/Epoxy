////////////////////////////////////////////////////////////////////////////
//
// Epoxy - A minimum MVVM assister library.
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

using Epoxy;
using EpoxyHello.Wpf.Controls;
using EpoxyHello.Wpf.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EpoxyHello.Wpf.ViewModels
{
    public sealed class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.Indicators = new ObservableCollection<UIElement>();

            // A handler for fetch button
            this.Fetch = Command.Create(async () =>
            {
                this.IsEnabled = false;

                var waitingBlock = new WaitingBlock();
                this.Indicators.Add(waitingBlock);

                try
                {
                    // Uses Reddit API
                    var reddits = await Reddit.FetchNewPostsAsync("r/aww");

                    this.Items.Clear();

                    foreach (var reddit in reddits)
                    {
                        this.Items.Add(new ItemViewModel
                        {
                            Title = reddit.Title,
                            Score = reddit.Score,
                            Image = await Reddit.FetchImageAsync(reddit.Url)
                        });
                    }
                }
                finally
                {
                    this.Indicators.Remove(waitingBlock);
                    IsEnabled = true;
                }
            });

            this.IsEnabled = true;

            //////////////////////////////////////////////////////////////////////////
            // Anchor/Pile:

            // Pile is an safe accessor of a temporary UIElement reference in view model.
            // CAUTION: NOT RECOMMENDED for normal usage on MVVM architecture,
            //    Pile is a last solution for complex UI manipulation.
            this.ButtonPile = Pile.Create<Button>();
            this.ButtonPileInvoker = Command.Factory.CreateSync(() =>
                this.ButtonPile.ExecuteSync(
                    // Rent temporary UIElement reference only inside of lambda expression.
                    button => button.Background = Brushes.Red));

            //////////////////////////////////////////////////////////////////////////
            // UIThread:

            var _ = Task.Run(async () =>
            {
                var count = 0;
                while (true)
                {
                    // Disjoint UI thread from current task.
                    await Task.Delay(500).ConfigureAwait(false);

                    // Rejoint UI thread.
                    // The bind method will cause InvalidOperationException if platform Application context was discarded.
                    await UIThread.Bind();

                    // Grant access UI contents.
                    this.ThreadIncrementer = count.ToString();
                    count++;
                }
            });
        }

        public bool IsEnabled
        {
            get => this.GetValue();
            private set => this.SetValue(value);
        }

        public ObservableCollection<ItemViewModel>? Items
        {
            get => this.GetValue<ObservableCollection<ItemViewModel>?>();
            private set => this.SetValue(value);
        }

        public Command? Fetch
        {
            get => this.GetValue();
            private set => this.SetValue(value);
        }

        public ObservableCollection<UIElement> Indicators
        {
            get => this.GetValue<ObservableCollection<UIElement>>();
            private set => this.SetValue(value);
        }

        public Pile<Button>? ButtonPile
        {
            get => this.GetValue<Pile<Button>?>();
            private set => this.SetValue(value);
        }

        public Command? ButtonPileInvoker
        {
            get => this.GetValue();
            private set => this.SetValue(value);
        }

        public string? ThreadIncrementer
        {
            get => this.GetValue();
            private set => this.SetValue(value);
        }
    }
}
