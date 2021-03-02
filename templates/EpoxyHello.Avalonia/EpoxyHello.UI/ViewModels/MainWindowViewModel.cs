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
    public sealed class MainWindowViewModel : ViewModel
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

        public Command? Ready
        {
            get => this.GetValue();
            private set => this.SetValue(value);
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
