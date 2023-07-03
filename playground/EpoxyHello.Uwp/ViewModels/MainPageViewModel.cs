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
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

using Epoxy;
using EpoxyHello.Models;

namespace EpoxyHello.Uwp.ViewModels;

[ViewModel]
public sealed class MainPageViewModel
{
    public Command Ready { get; }

    public bool IsEnabled { get; private set; }

    public ObservableCollection<ItemViewModel> Items { get; } = new();

    public Command Fetch { get; }

    public Pile<Button> ButtonPile { get; } = Pile.Factory.Create<Button>();

    public Command ButtonPileInvoker { get; }

    public string? ThreadIncrementer { get; private set; }

    public MainPageViewModel()
    {
        // A handler for window loaded
        this.Ready = Command.Factory.Create(() =>
        {
            this.IsEnabled = true;
            return default;
        });

        // A handler for fetch button
        this.Fetch = Command.Factory.Create(async () =>
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
        this.ButtonPileInvoker = Command.Factory.Create(() =>
            this.ButtonPile.RentAsync(
                // Rent temporary UIElement reference only inside of lambda expression.
                button =>
                {
                    button.Background = new SolidColorBrush(Color.FromArgb(0, 255, 0, 0));
                    return default;
                }));

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
}
