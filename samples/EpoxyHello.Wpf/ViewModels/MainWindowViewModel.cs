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

using Epoxy;
using Epoxy.Synchronized;

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using EpoxyHello.Models;
using EpoxyHello.Wpf.Controls;

namespace EpoxyHello.Wpf.ViewModels
{
    public sealed class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.Indicators = new ObservableCollection<UIElement>();

            // A handler for window loaded
            this.Ready = Command.Factory.CreateSync<RoutedEventArgs>(e =>
            {
                this.IsEnabled = true;
            });

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
                    this.Indicators.Remove(waitingBlock);
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

        public ObservableCollection<UIElement> Indicators
        {
            get => this.GetValue<ObservableCollection<UIElement>>();
            private set => this.SetValue(value);
        }
    }
}
