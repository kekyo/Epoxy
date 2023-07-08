using Epoxy;

namespace EpoxyHello.ViewModels;

[ViewModel]
public sealed class MainViewModel
{
    public Command Ready { get; }
    public string Title { get; private set; } = "";

    public MainViewModel()
    {
        // A handler for window loaded
        this.Ready = Command.Factory.Create(() =>
        {
            this.Title = "Hello Epoxy!";
            return default;
        });
    }
}
