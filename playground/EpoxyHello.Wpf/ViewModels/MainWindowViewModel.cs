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
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using EpoxyHello.Models;
using EpoxyHello.Wpf.Controls;

namespace EpoxyHello.Wpf.ViewModels;

[ViewModel]
public sealed class MainWindowViewModel
{
    public Command Ready { get; }

    public bool IsEnabled { get; set; }

    public ObservableCollection<ItemViewModel> Items { get; } = new();

    public Command Fetch { get; }

    public Pile<Panel> IndicatorPile { get; } = Pile.Factory.Create<Panel>();

    public MainWindowViewModel()
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
            // Disable button
            this.IsEnabled = false;

            // Temporary rent Grid children accessor
            await this.IndicatorPile.RentAsync(async indicator =>
            {
                // Show WaitingBlock control
                var waitingBlock = new WaitingBlock();
                indicator.Children.Add(waitingBlock);

                try
                {
                    // Uses Reddit API
                    var reddits = await Reddit.FetchNewPostsAsync("r/aww");

                    this.Items.Clear();

                    static async ValueTask<ImageSource?> FetchImageAsync(Uri url)
                    {
                        try
                        {
                            var bitmap = new WriteableBitmap(
                                BitmapFrame.Create(new MemoryStream(await Reddit.FetchImageAsync(url))));
                            bitmap.Freeze();
                            return bitmap;
                        }
                        // Some images will cause decoding error by WPF's BitmapFrame, so ignoring it.
                        catch (FileFormatException)
                        {
                            return null;
                        }
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
                    // Hide WaitingBlock control
                    indicator.Children.Remove(waitingBlock);

                    // Re-enable button
                    this.IsEnabled = true;
                }
            });
        });
    }
}
