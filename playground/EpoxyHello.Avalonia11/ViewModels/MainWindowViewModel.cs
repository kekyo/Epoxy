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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Avalonia.Media.Imaging;

using Epoxy;
using EpoxyHello.Models;

namespace EpoxyHello.Avalonia11.ViewModels;

[ViewModel]
public sealed class MainWindowViewModel
{
    public Command Ready { get; }

    public bool IsEnabled { get; private set; }

    public ObservableCollection<ItemViewModel> Items { get; } = new();

    public Command Fetch { get; }

    public MainWindowViewModel()
    {
        // A handler for window opened
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
                // Uses The Cat API
                var cats = await TheCatAPI.FetchTheCatsAsync(10);

                this.Items.Clear();

                static async ValueTask<Bitmap?> FetchImageAsync(Uri url) =>
                    new Bitmap(new MemoryStream(await TheCatAPI.FetchImageAsync(url)));

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
                this.IsEnabled = true;
            }
        });
    }
}
