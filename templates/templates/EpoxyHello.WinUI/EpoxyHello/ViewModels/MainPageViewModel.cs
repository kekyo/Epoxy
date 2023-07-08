using Epoxy;
using System;

using EpoxyHello.Models;
using System.Collections.ObjectModel;

namespace EpoxyHello.ViewModels;

[ViewModel]
public sealed class MainPageViewModel
{
    public Command Ready { get; }
    public string Title { get; private set; } = "";

    public MainPageViewModel()
    {
        // A handler for window loaded
        this.Ready = Command.Factory.Create(() =>
        {
            this.Title = "Hello Epoxy!";
            return default;
        });
    }
}
