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
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using EpoxyHello.Models;
using EpoxyHello.Wpf.Controls;

namespace EpoxyHello.Wpf.ViewModels;

[ViewModel]
public sealed class MainWindowViewModel
{
    public Well<Window> MainWindowWell { get; } = Well.Factory.Create<Window>();

    public bool IsEnabled { get; set; }

    public ObservableCollection<ItemViewModel> Items { get; } = new();

    public Command Fetch { get; }

    public Pile<Panel> IndicatorPile { get; } = Pile.Factory.Create<Panel>();

    public MainWindowViewModel()
    {
        // A handler for window opened
        //this.MainWindowWell.Add(
        //    //FrameworkElement.LoadedEvent,
        //    "Loaded",
        //    () =>
        //    {
        //        this.IsEnabled = true;
        //        return default;
        //    },
        //    (window, obj, ptr) => {
        //        var dlg = new EventHandler(obj, ptr);
        //        window.Opened += dlg;
        //    },
        //    (window, obj, ptr) => {
        //        var dlg = new EventHandler(obj, ptr);
        //        window.Opened -= dlg;
        //    });

        this.MainWindowWell.Add(
            //FrameworkElement.LoadedEvent,
            "Loaded",
            () =>
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
                    // Uses The Cat API
                    var cats = await TheCatAPI.FetchTheCatsAsync(10);

                    this.Items.Clear();

                    static async ValueTask<ImageSource?> FetchImageAsync(Uri url)
                    {
                        try
                        {
                            var bitmap = new WriteableBitmap(
                                BitmapFrame.Create(new MemoryStream(await TheCatAPI.FetchImageAsync(url))));
                            bitmap.Freeze();
                            return bitmap;
                        }
                        // Some images will cause decoding error by WPF's BitmapFrame, so ignoring it.
                        catch (FileFormatException)
                        {
                            return null;
                        }
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
