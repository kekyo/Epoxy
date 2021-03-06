﻿////////////////////////////////////////////////////////////////////////////
//
// Epoxy template source code.
// Write your own copyright and note.
// (You can use https://github.com/rubicon-oss/LicenseHeaderManager)
//
////////////////////////////////////////////////////////////////////////////

#nullable enable

using Epoxy;
using Windows.UI.Xaml.Media;

namespace EpoxyHello.ViewModels
{
    [ViewModel]
    public sealed class ItemViewModel
    {
        public string? Title { get; set; }

        public ImageSource? Image { get; set; }

        public int Score { get; set; }
    }
}
