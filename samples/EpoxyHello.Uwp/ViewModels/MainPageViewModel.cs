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

using Epoxy;
using Epoxy.Synchronized;

using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

using EpoxyHello.Models;

namespace EpoxyHello.Uwp.ViewModels
{
    public sealed class MainPageViewModel : ViewModel
    {
        public MainPageViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();

            // A handler for window loaded
            this.Ready = Command.Factory.CreateSync<RoutedEventArgs>(e =>
            {
                this.IsEnabled = true;
            });

            // A handler for fetch button
            this.Fetch = CommandFactory.Create(async () =>
            {
                this.IsEnabled = false;

                try
                {
                    // Uses Reddit API
                    var reddits = await Reddit.FetchNewPostsAsync("r/aww");

                    this.Items.Clear();

                    static async ValueTask<ImageSource?> FetchImageAsync(Uri url)
                    {
                        var raStream = new InMemoryRandomAccessStream();
                        await raStream.WriteAsync((await Reddit.FetchImageAsync(url)).AsBuffer());
                        raStream.Seek(0);
                        var bitmap = new BitmapImage();
                        await bitmap.SetSourceAsync(raStream);
                        return bitmap;
                    }

                    foreach (var reddit in reddits)
                    {
                        this.Items.Add(new ItemViewModel
                        {
                            Title = reddit.Title,
                            Score = reddit.Score,
                            Image = await FetchImageAsync(reddit.Url)
                        });
                    }
                }
                finally
                {
                    IsEnabled = true;
                }
            });

            //////////////////////////////////////////////////////////////////////////
            // Anchor/Pile:

            // Pile is an safe accessor of a temporary UIElement reference in view model.
            // CAUTION: NOT RECOMMENDED for normal usage on MVVM architecture,
            //    Pile is a last solution for complex UI manipulation.
            this.ButtonPile = PileFactory.Create<Button>();
            this.ButtonPileInvoker = Command.Factory.CreateSync(() =>
                this.ButtonPile.RentSync(
                    // Rent temporary UIElement reference only inside of lambda expression.
                    button => button.Background = new SolidColorBrush(Color.FromArgb(0, 255, 0, 0))));

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

        public Command? Ready
        {
            get => this.GetValue();
            set => this.SetValue(value);
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
