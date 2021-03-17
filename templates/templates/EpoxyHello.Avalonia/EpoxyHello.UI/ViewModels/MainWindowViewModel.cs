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
using System.IO;
using System.Threading.Tasks;

using Avalonia.Media.Imaging;

using EpoxyHello.Models;

namespace EpoxyHello.ViewModels
{
    [ViewModel]
    public sealed class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();

            // A handler for window opened
            this.Ready = Command.Factory.CreateSync<EventArgs>(e =>
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

                    static async ValueTask<Bitmap?> FetchImageAsync(Uri url) =>
                        new Bitmap(new MemoryStream(await Reddit.FetchImageAsync(url)));

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
