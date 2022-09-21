////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

using Epoxy;
using Epoxy.Synchronized;

using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

using EpoxyHello.Models;

namespace EpoxyHello.ViewModels
{
    [ViewModel]
    public sealed class MainPageViewModel
    {
        public MainPageViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();

            // A handler for window loaded
            this.Ready = Command.Factory.CreateSync(() =>
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
        }

        public Command? Ready { get; private set; }

        public bool IsEnabled { get; private set; }

        public ObservableCollection<ItemViewModel>? Items { get; private set; }

        public Command? Fetch { get; private set; }
    }
}
