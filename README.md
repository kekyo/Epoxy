# Epoxy - A minimum MVVM assister library. 

![Epoxy bin](Images/Epoxy.160.png)

|Package|Status|
|:--|:--|
|Epoxy.Wpf|[![NuGet Epoxy.Wpf](https://img.shields.io/nuget/v/Epoxy.Wpf.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Wpf)|
|Epoxy.Uwp|[![NuGet Epoxy.Uwp](https://img.shields.io/nuget/v/Epoxy.Uwp.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Uwp)|
|Epoxy.Xamarin.Forms|[![NuGet Epoxy.Xamarin.Forms](https://img.shields.io/nuget/v/Epoxy.Xamarin.Forms.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Xamarin.Forms)|

## What is this ?

* Epoxy is .NET XAML Model-View-ViewModel data-bindable infrastructure library, and very simple usage API sets.
* Safe asynchronous operation (async-await) ready.
* C# 8.0 nullable reference types ready.
* Easy understandable.
* Supported simplest and minimalism Model-View-ViewModel design.
* Smallest footprint.
* Friction-free for combination other MVVM frameworks such as ReactiveProperty and etc.

## Current status:

* Still under construction.

## Sample code

You can refer full WPF application sample code in [EpoxyHello.Wpf](samples/EpoxyHello.Wpf).

Full asynchronous fetching and updating into ListBox when you click a button.

![EpoxyHello.Wpf](https://github.com/kekyo/Epoxy/raw/master/Images/sample.png)

## Getting started minimum MVVM application

Completed separately xaml based view declarations (Suppressed, refer full sample code instead):

```xml
<Window
    x:Class="EpoxyHello.Wpf.Views.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    Title="EpoxyHello.Wpf" Height="450" Width="800">

    <!-- Place view model instance, it'll verify types (by IDE)  -->
    <Window.DataContext>
        <viewmodels:MainWindowViewModel />
    </Window.DataContext>
    
    <DockPanel>
        <!-- Binding button click event. -->
        <Button DockPanel.Dock="Top" Height="30"
                Command="{Binding Fetch}">Asynchronous fetch r/aww from Reddit!</Button>
        <Grid>
            <!-- Binding an image collection. -->
            <ListBox ItemsSource="{Binding Items}"
                ScrollViewer.HorizontalScrollBarVisibility="Disabled"
                ScrollViewer.VerticalScrollBarVisibility="Auto"
                ScrollViewer.CanContentScroll="False">
                <ListBox.ItemTemplate>
                    <DataTemplate>
                        <!-- Binding an image. -->
                        <Image
                            Source="{Binding Image}"
                            Stretch="UniformToFill" />
                    </DataTemplate>
                </ListBox.ItemTemplate>
            </ListBox>
        </Grid>
    </DockPanel>
</Window>
```

Completed separately ViewModel implementation.

```csharp
// Step 1: Write view model class deriving from Epoxy.ViewModel.
public sealed class MainWindowViewModel : ViewModel
{
    // Step 2: Expose properties to view.
    //    Epoxy can handle with C# 8.0's nullable reference types.
    public Command? Fetch
    {
        get => GetValue();
        private set => SetValue(value);
    }

    public ObservableCollection<Image>? Items
    {
        // Step 2-1: Can suppress the GetValue() generic argument
        //   if use basic types (primitives, string, Command and etc).
        get => GetValue<ObservableCollection<Image>?>();
        private set => SetValue(value);
    }

    public MainWindowViewModel()
    {
        // Step 3: Property setter will raise PropertyChanged events if value is changed.
        this.Items = new ObservableCollection<ItemViewModel>();

        // Step 4: A handler for fetch button.
        //   Ofcourse, we can use async/await with safer lambda expressions!
        this.Fetch = CreateCommand(async () =>
        {
            var reddits = await Reddit.FetchNewPostsAsync("r/aww");

            this.Items.Clear();

            foreach (var reddit in reddits)
            {
                this.Items.Add(await Reddit.FetchImageAsync(reddit.Url));
            }
        });
    }
}
```

## License

Apache-v2
