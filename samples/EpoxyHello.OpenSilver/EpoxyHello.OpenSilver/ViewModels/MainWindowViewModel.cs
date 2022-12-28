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
using System.IO;
using System.Threading.Tasks;

using System.Windows.Media;

using EpoxyHello.Models;
using System.Windows.Media.Imaging;

namespace EpoxyHello.OpenSilver.ViewModels
{
    public sealed class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();

            // A handler for window opened
            this.Ready = Command.Factory.CreateSync(() =>
            {
                this.IsEnabled = true;
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
                        var bitmap = new BitmapImage();
                        bitmap.SetSource(new MemoryStream(await Reddit.FetchImageAsync(url)));
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
    }
}
