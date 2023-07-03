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

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

using Epoxy;
using EpoxyHello.Models;

namespace EpoxyHello.WinUI.Desktop.ViewModels;

[ViewModel]
public sealed class MainPageViewModel
{
    public Command Ready { get; }

    public bool IsEnabled { get; private set; }

    public ObservableCollection<ItemViewModel> Items { get; } = new();

    public Command Fetch { get; }

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
                this.IsEnabled = true;
            }
        });
    }
}
