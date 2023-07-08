using Epoxy;
using System;

// Conflicted between Microsoft.Maui.Controls.Command and Epoxy.Command.
using Command = Epoxy.Command;

using EpoxyHello.Models;

namespace EpoxyHello.ViewModels;

[ViewModel]
public sealed class MainContentPageViewModel
{
    public Command Ready { get; }
    public string Title { get; private set; } = "";

    public MainContentPageViewModel()
    {
        // A handler for page appearing
        this.Ready = Command.Factory.Create(() =>
        {
            this.Title = "Hello Epoxy!";
            return default;
        });
    }
}
