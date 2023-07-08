using Epoxy;
using System;

using EpoxyHello.Models;

namespace EpoxyHello.ViewModels;

[ViewModel]
public sealed class MainWindowViewModel
{
    public Command Ready { get; }
    public string Title { get; private set; } = "";

    public MainWindowViewModel()
    {
        // A handler for window opened
        this.Ready = Command.Factory.Create(() =>
        {
            this.Title = "Hello Epoxy!";
            return default;
        });
    }
}
