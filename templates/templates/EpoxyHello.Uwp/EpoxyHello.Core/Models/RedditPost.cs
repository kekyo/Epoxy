////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

using System;

namespace EpoxyHello.Models
{
    public sealed class RedditPost
    {
        public readonly string Title;
        public readonly Uri Url;
        public readonly int Score;

        public RedditPost(string title, Uri url, int score)
        {
            this.Title = title;
            this.Url = url;
            this.Score = score;
        }
    }
}
