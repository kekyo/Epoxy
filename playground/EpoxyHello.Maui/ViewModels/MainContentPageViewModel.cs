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

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

using Epoxy;
using EpoxyHello.Models;
using EpoxyHello.Maui.Controls;

using Command = Epoxy.Command;

namespace EpoxyHello.Maui.ViewModels;

[ViewModel]
public sealed class MainContentPageViewModel
{
    public Command Ready { get; }

    public bool IsEnabled { get; private set; }

    public ObservableCollection<ItemViewModel> Items { get; } = new();

    public Command Fetch { get; }

    public Pile<Grid> IndicatorPile { get; } = Pile.Factory.Create<Grid>();

    public MainContentPageViewModel()
    {
        // A handler for page appearing
        this.Ready = Command.Factory.Create(() =>
        {
            this.IsEnabled = true;
            return default;
        });

        // A handler for fetch button
        this.Fetch = Command.Factory.Create(async () =>
        {
            this.IsEnabled = false;

            await this.IndicatorPile.RentAsync(async indicator =>
            {
                // Show WaitingBlock control
                var waitingBlock = new WaitingBlock();
                indicator.Children.Add(waitingBlock);

                try
                {
                    // Uses The Cat API
                    var cats = await TheCatAPI.FetchTheCatsAsync(10);

                    this.Items.Clear();

                    static async ValueTask<ImageSource> FetchImageAsync(Uri url)
                    {
                        var data = await TheCatAPI.FetchImageAsync(url);
                        return new StreamImageSource
                        {
                            Stream = _ => Task.FromResult((Stream)new MemoryStream(data))
                        };
                    }

                    foreach (var cat in cats)
                    {
                        if (cat.Url is { } url)
                        {
                            var bleed = cat?.Bleeds.FirstOrDefault();
                            this.Items.Add(new ItemViewModel
                            {
                                Title = bleed?.Description ?? bleed?.Temperament ?? "(No comment)",
                                Score = bleed?.Intelligence ?? 5,
                                Image = await FetchImageAsync(url)
                            });
                        }
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
