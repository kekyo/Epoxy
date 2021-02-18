# Epoxy - A minimum MVVM assister library. 

![Epoxy bin](Images/Epoxy.160.png)

[![Japanese language](Images/Japanese.256.png)](https://github.com/kekyo/Epoxy/blob/main/README.ja.md)

[![Project Status: WIP – Initial development is in progress, usable release suitable for the public.](https://www.repostatus.org/badges/latest/wip.svg)](https://www.repostatus.org/#wip)

|Package|Status|
|:--|:--|
|Epoxy.Wpf|[![NuGet Epoxy.Wpf](https://img.shields.io/nuget/v/Epoxy.Wpf.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Wpf)|
|Epoxy.Xamarin.Forms|[![NuGet Epoxy.Xamarin.Forms](https://img.shields.io/nuget/v/Epoxy.Xamarin.Forms.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Xamarin.Forms)|
|Epoxy.Uwp|[![NuGet Epoxy.Uwp](https://img.shields.io/nuget/v/Epoxy.Uwp.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Uwp)|

## What is this ?

* Epoxy is an .NET XAML Model-View-ViewModel data-bindable infrastructure library, and very simple API sets.
* Supported platforms:
  * WPF: .NET 5/.NET Core 3.0/3.1, .NET Framework 4.5/4.8
  * Xamarin Forms: .NET Standard 2.0
  * UWP: Universal Windows 10
* Safe asynchronous operation (async-await) ready.
* C# 8.0 nullable reference types ready.
* Smallest footprint and easy understandable.
* Supported simplest and minimalism Model-View-ViewModel design.
  * The main goal is to avoid writing code behinds in the View, but to avoid having to write complicated processes to do so.
  * The focus is on areas where MVVM beginners might stumble.
  * We don't do complete commonality; only Epoxy has a common structure as much as possible, and other parts are dependent on each environment to avoid being the greatest common divisor.
* Friction-free for combination other MVVM frameworks such as ReactiveProperty and etc.

## Sample code

You can refer full WPF/Xamarin Forms application sample code in.
This sample displays a list of the latest posts and images from the Reddit forum r/aww, downloading them asynchronously and displays them in a list format.

* [EpoxyHello.Core - Common(Model)](samples/EpoxyHello.Core). Go to Reddit and download the posts. netstandard2.0 for common use.
* [EpoxyHello.Wpf - View,ViewModel](samples/EpoxyHello.Wpf). WPF View and ViewModel.
* [EpoxyHello.Xamarin.Forms - View,ViewModel](samples/EpoxyHello.Xamarin.Forms). Xamarin Forms View and ViewModel.

Full asynchronous fetching and updating into ListBox when you click a button.

![EpoxyHello.Wpf](https://github.com/kekyo/Epoxy/raw/main/Images/sample.Wpf.png)

![EpoxyHello.Xamarin.Forms](https://github.com/kekyo/Epoxy/raw/main/Images/sample.Xamarin.Forms.png)

## Getting started minimum MVVM application

Review of Model-View-ViewModel architecture:
* `View`: Describes the user interface in XAML and write binding expressions to the `ViewModel` (without writing code-behinds).
* `ViewModel`: Get information from `Model` and define properties that map to `View`.
* `Model`: Implement processes that are not directly related to the user interface. In this case, the process of downloading posts from Reddit.

Note: There are many theories about the architecture of MVVM.
It is a good idea to brush up on the design without aiming for perfection from the start.
Epoxy is designed to be improved step by step.

Completed separately xaml based view declarations.
(WPF, introducing focused, refer full sample code instead):

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

Completed separately `ViewModel` implementation.

```csharp
// Step 1: Write view model class deriving from Epoxy.ViewModel.
public sealed class MainWindowViewModel : ViewModel
{
    // Step 2: Expose properties to view.
    //    Epoxy can handle with C# 8.0's nullable reference types.
    public Command? Fetch
    {
        get => this.GetValue();
        private set => this.SetValue(value);
    }

    public ObservableCollection<ImageSource>? Items
    {
        // Step 2-1: Can suppress the GetValue() generic argument
        //   if use basic types (primitives, string, Command and etc).
        get => this.GetValue<ObservableCollection<ImageSource>?>();
        private set => this.SetValue(value);
    }

    public MainWindowViewModel()
    {
        // Step 3: Property setter will raise PropertyChanged events if value is changed.
        this.Items = new ObservableCollection<ItemViewModel>();

        // Step 4: A handler for fetch button.
        //   Ofcourse, we can use async/await safely in lambda expressions!
        this.Fetch = Command.Create(async () =>
        {
            var reddits = await Reddit.FetchNewPostsAsync("r/aww");

            this.Items.Clear();

            foreach (var reddit in reddits)
            {
                var bitmap = new WriteableBitmap(
                    BitmapFrame.Create(new MemoryStream(await Reddit.FetchImageAsync(url))));
                bitmap.Freeze();
                this.Items.Add(bitmap);
            }
        });
    }
}
```

The common code to access Reddit is implemented in the `EpoxyHello.Core` project.
It does not depend on either WPF or Xamarin Forms assemblies and is completely independent.

By eliminating dependencies in this way, we can achieve commonality for multi-platform support.
However, for small-scale development, you can place the `Model` implementation in the same project as the `ViewModel` implementation
(separating them eliminates the possibility of unintentional dependencies).

[Image downloader from Reddit post (EpoxyHello.Core)](https://github.com/kekyo/Epoxy/blob/1b16a9e447876a5e109166c7c5f5902a1dc52947/samples/EpoxyHello.Core/Models/Reddit.cs#L63):

```csharp
// Model implementation: The pure netstandard2.0 library.
// Downalod image data from Reddit.
public static async ValueTask<byte[]> FetchImageAsync(Uri url)
{
    using (var response =
        await httpClient.GetAsync(url).ConfigureAwait(false))
    {
        using (var stream =
            await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
        {
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms).ConfigureAwait(false);
            return ms.ToArray();
        }
    }
}
```

Since the Model implementation does not directly manipulate the user interface fragments,
it can isolate task contexts with `task.ConfigureAwait(false)` annotation to improve performance.

### About ViewModel base class

The `ViewModel` base class provides an implementation of the `GetValue`/`SetValue` methods.
These methods automatically notify to the XAML control by property changes event `NotifyPropertyChanging`/`NotifyPropertyChanged`.
For example, when a property is changed upon a button click in `ViewModel`, the change will be notified to the XAML control and reflected to the user interface.

As commented in the sample code above, the type argument may be omitted for `GetValue`.
See [definition of implicit operator](https://github.com/kekyo/Epoxy/blob/1b16a9e447876a5e109166c7c5f5902a1dc52947/Epoxy/ValueHolder.cs#L61) for the optional types.


## Minor but useful features

### ChildrenBinder

TODO:

[For example (In WPF XAML)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/Views/MainWindow.xaml#L71)

[For example (In WPF view model)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/ViewModels/MainWindowViewModel.cs#L119)

### Anchor/Pile

`Anchor`/`Pile` pair is a loose connection between `UIElement` (Xamarin Forms `Element`) and `ViewModel`s.

Rare case in MVVM architecture, we have to access directly `UIElement` member,
but sometimes gointg to wait the pitfall of circular references (and couldn't unbind by GC).

The `Pile` pull in the `UIElement`'s anchor, and we can rent temporary `UIElement` reference safely inside `ViewModel`.

```xml
<!-- Declared Epoxy namespace -->
<Window xmlns:epoxy="clr-namespace:Epoxy;assembly=Epoxy">
    <!-- Placed Anchor onto the TextBox and bound property -->
    <TextBox epoxy:Anchor.Pile="{Binding LogPile}" />
</Window>
```

```csharp
// Declared a Pile into the ViewModel.
this.LogPile = Pile.Create<TextBox>();

// ...

// Do rent by Pile when we have to manipulate the TextBox directly:
await this.LogPile.ExecuteAsync(async textBox =>
{
    // Fetch information from related model.
    var result = await ServerAccessor.GetResultTextAsync();
    // We can manipulate safer directly TextBox.
    textBox.AppendText(result);
});
```

[For example (In WPF XAML)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/Views/MainWindow.xaml#L39)

[For example (In WPF view model)](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/ViewModels/MainWindowViewModel.cs#L74)

### ValueConverter

The `ValueConverter` class is a base class for safely implementing the XAML converters.
It avoids cumbersome typecasting by explicitly specifying the type, and
It can also automatically fail to convert incompatible types.

You can give an argument to the converter with `ConverterParameter` on the XAML,
then you have to change the base class to use when you receive this parameter or not.

```csharp
// This is an implementation of a converter that takes an integer and converts it to a Brush.
// Specify the expected type as a generic argument.
public sealed class ScoreToBrushConverter : ValueConverter<Brush, int>
{
    // When the need for conversion arises, TryConvert will be called.
    public override bool TryConvert(int from, out Brush result)
    {
        // The result of the conversion is returned by the out argument.
        result = from >= 5 ? Brush.Red : Brush.White;
        // If the conversion fails, you have to return false.
        return true;
    }

    // Although not shown here as an example, TryConvertBack can also be implemented.
}
```

This is an example of receiving a converter parameter:

```csharp
// In this example, it receives the value specified by ConverterParameter.
// Its type is specified by the generic third argument.
// Here is an example of receiving a string:
public sealed class ScoreToBrushConverter : ValueConverter<Brush, int, string>
{
    // The value of the parameter is passed as the second argument.
    public override bool TryConvert(int from, string parameter, out Brush result)
    {
        // ...
    }
}
```

Note: The XAML converter cannot be asynchronous due to the structure of XAML.
This means that the `TryConvert` method cannot be made to behave like `TryConvertAsync`.

Try not to do asynchronous processing in the XAML converter!
(If you want to do so, you can implement it on the `Model` or `ViewModel` side to avoid problems such as deadlocks).

[For example](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/samples/EpoxyHello.Wpf/Views/Converters/ScoreToBrushConverter.cs#L25)

### UIThread

Some different platform contains different UI thread manipulation.
Epoxy can handle only one [UIThread class](https://github.com/kekyo/Epoxy/blob/09a274bd2852cf8120347411d898aca414a16baa/Epoxy/UIThread.cs#L29),
it has commonly manipulation methods.
We can easier combine both UI manipulation and asynchronous operations.

```csharp
// Can check what current thread
Debug.Assert(UIThread.IsBound);

// Invoke asynchronous operation and will detach current thread context.
var read = await httpStream.ReadAsync(...).ConfigureAwait(false);

// Executes on the worker thread.
Console.WriteLine($"Read={read}");

// Switches to the UI thread explicitly.
await UIThread.Bind();

// We can handle any UI elements in the UI thread (include binding operation.)
this.Log = $"Read={read}";
```

## License

Apache-v2
