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
using Xamarin.Forms;

// Conflicted between Xamarin.Forms.Command and Epoxy.Command.
using Command = Epoxy.Command;

using EpoxyHello.Models;

namespace EpoxyHello.ViewModels
{
    public sealed class MainContentPageViewModel : ViewModel
    {
        public MainContentPageViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();

            // A handler for page appearing
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

                    static async ValueTask<ImageSource> FetchImageAsync(Uri url)
                    {
                        var data = await Reddit.FetchImageAsync(url);
                        return new StreamImageSource
                        {
                            Stream = _ => Task.FromResult((Stream)new MemoryStream(data))
                        };
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

        public Epoxy.Command? Fetch
        {
            get => this.GetValue();
            private set => this.SetValue(value);
        }
    }
}
