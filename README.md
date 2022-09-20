# Epoxy - An independent flexible XAML MVVM library

![Epoxy bin](Images/Epoxy.160.png)

[![Japanese language](Images/Japanese.256.png)](https://github.com/kekyo/Epoxy/blob/main/README.ja.md)

[![Project Status: Active](https://www.repostatus.org/badges/latest/active.svg)](https://www.repostatus.org/#wip)

## NuGet for All platform (C#)

|Package|main|devel|Description|
|:--|:--|:--|:--|
|Epoxy.Wpf|[![NuGet Epoxy.Wpf](https://img.shields.io/nuget/v/Epoxy.Wpf.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Wpf)|[![MyGet Epoxy.Wpf](https://img.shields.io/myget/epoxy/v/Epoxy.Wpf.svg?style=flat&label=myget)](https://www.myget.org/feed/epoxy/package/nuget/Epoxy.Wpf)|WPF version|
|Epoxy.Avalonia|[![NuGet Epoxy.Avalonia](https://img.shields.io/nuget/v/Epoxy.Avalonia.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Avalonia)|[![MyGet Epoxy.Avalonia](https://img.shields.io/myget/epoxy/v/Epoxy.Avalonia.svg?style=flat&label=myget)](https://www.myget.org/feed/epoxy/package/nuget/Epoxy.Avalonia)|Avalonia version|
|Epoxy.OpenSilver|[![NuGet Epoxy.OpenSilver](https://img.shields.io/nuget/v/Epoxy.OpenSilver.svg?style=flat)](https://www.nuget.org/packages/Epoxy.OpenSilver)|[![MyGet Epoxy.OpenSilver](https://img.shields.io/myget/epoxy/v/Epoxy.OpenSilver.svg?style=flat&label=myget)](https://www.myget.org/feed/epoxy/package/nuget/Epoxy.OpenSilver)|OpenSilver version|
|Epoxy.Xamarin.Forms|[![NuGet Epoxy.Xamarin.Forms](https://img.shields.io/nuget/v/Epoxy.Xamarin.Forms.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Xamarin.Forms)|[![MyGet Epoxy.Xamarin.Forms](https://img.shields.io/myget/epoxy/v/Epoxy.Xamarin.Forms.svg?style=flat&label=myget)](https://www.myget.org/feed/epoxy/package/nuget/Epoxy.Xamarin.Forms)|Xamarin Forms version|
|Epoxy.Uwp|[![NuGet Epoxy.Uwp](https://img.shields.io/nuget/v/Epoxy.Uwp.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Uwp)|[![MyGet Epoxy.Uwp](https://img.shields.io/myget/epoxy/v/Epoxy.Uwp.svg?style=flat&label=myget)](https://www.myget.org/feed/epoxy/package/nuget/Epoxy.Uwp)|Universal Windows version|
|Epoxy.WinUI|[![NuGet Epoxy.WinUI](https://img.shields.io/nuget/v/Epoxy.WinUI.svg?style=flat)](https://www.nuget.org/packages/Epoxy.WinUI)|[![MyGet Epoxy.WinUI](https://img.shields.io/myget/epoxy/v/Epoxy.WinUI.svg?style=flat&label=myget)](https://www.myget.org/feed/epoxy/package/nuget/Epoxy.WinUI)|WinUI 3 version|
|Epoxy.Uno|[![NuGet Epoxy.Uno](https://img.shields.io/nuget/v/Epoxy.Uno.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Uno)|[![MyGet Epoxy.Uno](https://img.shields.io/myget/epoxy/v/Epoxy.Uno.svg?style=flat&label=myget)](https://www.myget.org/feed/epoxy/package/nuget/Epoxy.Uno)|Uno platform version (**BUGGY**)|

## NuGet for F# specialized

|Package|main|devel|Description|
|:--|:--|:--|:--|
|FSharp.Epoxy.Wpf|[![NuGet FSharp.Epoxy.Wpf](https://img.shields.io/nuget/v/FSharp.Epoxy.Wpf.svg?style=flat)](https://www.nuget.org/packages/FSharp.Epoxy.Wpf)|[![MyGet FSharp.Epoxy.Wpf](https://img.shields.io/myget/epoxy/v/FSharp.Epoxy.Wpf.svg?style=flat&label=myget)](https://www.myget.org/feed/epoxy/package/nuget/FSharp.Epoxy.Wpf)|WPF version|
|FSharp.Epoxy.Avalonia|[![NuGet FSharp.Epoxy.Avalonia](https://img.shields.io/nuget/v/FSharp.Epoxy.Avalonia.svg?style=flat)](https://www.nuget.org/packages/FSharp.Epoxy.Avalonia)|[![MyGet FSharp.Epoxy.Avalonia](https://img.shields.io/myget/epoxy/v/FSharp.Epoxy.Avalonia.svg?style=flat&label=myget)](https://www.myget.org/feed/epoxy/package/nuget/FSharp.Epoxy.Avalonia)|Avalonia version|

## dotnet CLI template

|Package|main|devel|Description|
|:--|:--|:--|:--|
|Epoxy.Templates|[![NuGet Epoxy.Templates](https://img.shields.io/nuget/v/Epoxy.Templates.svg?style=flat)](https://www.nuget.org/packages/Epoxy.Templates)|[![MyGet Epoxy.Templates](https://img.shields.io/myget/epoxy/v/Epoxy.Templates.svg?style=flat&label=myget)](https://www.myget.org/feed/epoxy/package/nuget/Epoxy.Templates)|dotnet CLI template package|

## What is this ?

* Epoxy is a .NET XAML Model-View-ViewModel data-bindable infrastructure library, independent flexible API sets.
  * All .NET languages including C#, and specialized F# NuGet packages are available.
* Supported platforms:
  * WPF: .NET 6.0/5.0, .NET Core 3.0/3.1, .NET Framework 4.5/4.8
  * Avalonia: [Avalonia](https://avaloniaui.net/) (0.10.18 or higher)
  * OpenSilver: [OpenSilver](https://opensilver.net/) (1.0.0 or higher)
  * Xamarin Forms: [Xamarin Forms](https://github.com/xamarin/Xamarin.Forms) (5.0.0.2515 or higher)
  * Universal Windows: Universal Windows 10 (uap10.0.16299 or higher)
  * WinUI 3: [Windows App SDK](https://github.com/microsoft/WindowsAppSDK) (net5.0-windows10.0.17134.0 or higher)
  * Uno: [Uno platform](https://platform.uno/) (Uno.UI 3.7.6 or higher: uap10.0.17763, netstandard2.0[wpf, wasm, tizen], xamarinios10, xamarinmac20 and monoandroid10.0) / **Uno is not stable, so we can only confirm it on UWP hosts**
* Safe asynchronous operation (async-await) ready.
* C# 8.0 nullable reference types ready.
* F# is 6.0 compatible, F# signatures (camel-case functions, function types, `Async` type assumptions) are defined.
* Smallest footprint and easy understandable.
  * No dependency on non-platform standard frameworks or libraries.
* Supported simplest and minimalism Model-View-ViewModel design.
  * The main goal is to avoid writing code behinds in the View, but to avoid having to write complicated processes to do so.
  * The focus is on areas where MVVM beginners might stumble.
  * We don't do complete commonality; only Epoxy has a common structure as much as possible, and other parts are dependent on each environment to avoid being the greatest common divisor.
  * Each function is "unrelated" to each other. Since they are independent, they can be freely combined.
* Friction-free for combination other framework libraries such as ReactiveProperty and etc.

## Sample code

You can refer multi-platform application sample code variation in.
This sample displays a list of the latest posts and images from the Reddit forum r/aww, downloading them asynchronously and displays them in a list format.

### How to get and build the sample code

The .NET 6.0 SDK CLI template is supported. You can easily try the sample code in a clean state with the following command:

```bash
# Install the template package (Only at the first time or version update)
dotnet new -i Epoxy.Templates

# Extract the WPF sample code to the current directory.
dotnet new epoxy-wpf

# Build
dotnet build
```

* Caution: .NET 6.0 SDK must be installed beforehand because the template assumes .NET 6.0.
  For other versions only (e.g. .NET 5.0 SDK), the build will fail if you do not modify `TargetFramework` property.

### List of currently supported templates

|`dotnet new` parameter|Language|Target|
|:--|:--|:--|
|`epoxy-wpf`|C#, F#|Sample code for WPF|
|`epoxy-avalonia`|C#, F#|Sample code for Avalonia|
|`epoxy-opensilver`|C#|Sample code for OpenSilver|
|`epoxy-xamarin-forms`|C#|Sample code for Xamarin Forms|
|`epoxy-uwp`|C#|Sample code for UWP|
|`epoxy-winui`|C#|Sample code for WinUI 3|

* By default, the C# sample code is extracted; to change to F#, add option into command line like: `dotnet new epoxy-wpf -lang F#`.
* Xamarin Forms and UWP are required old style MSBuild project.
  * If you want to build and run, you need to open the solution in Visual Studio instead of `dotnet build`.
* OpenSilver sample code is contained only .NET Framework based simulator project.
  * If you want to build and run, you need to open the solution in Visual Studio instead of `dotnet build`.
  * You need to add a web hosting project when need to host onto Chrome and Firefox by WebAssembly.
* For sample code other than the above, refer to the `samples` directory in the repository.
* You can use devel branch package placed MyGet, describes below: `dotnet new -i Epoxy.Templates::<version> --nuget-source https://www.myget.org/F/epoxy/api/v3/index.json`

### Choose by Visual Studio wizard dialog

When you install templates with above step, Visual Studio project creation wizard will show up Epoxy's templates in the dialog:

![Template selection dialog](Images/vswizard_en.png)

----

### Detail for sample code

Full asynchronous fetching and updating into ListBox when you click a button.

![EpoxyHello.Wpf](https://github.com/kekyo/Epoxy/raw/main/Images/sample.Wpf.png)

![EpoxyHello.Xamarin.Forms](https://github.com/kekyo/Epoxy/raw/main/Images/sample.Xamarin.Forms.png)

----

## Getting started minimum MVVM application

Review of Model-View-ViewModel architecture:

* `View`: Describes the user interface in XAML and write binding expressions to the `ViewModel` (without writing code-behinds).
* `ViewModel`: Get information from `Model` and define properties that map to `View`.
* `Model`: Implement processes that are not directly related to the user interface. In this case, the process of downloading posts from Reddit.

The relationship between these MVVM elements is illustrated in the following figure:

![MVVM architecture](Images/diagram.png)

NOTE: There are many theories about the architecture of MVVM.
It is a good idea to brush up on the design without aiming for perfection from the start.
Epoxy is designed to be improved step by step.

Completed separately xaml based view declarations.
(WPF, introducing focused, refer full sample code instead):

### Example of View (WPF/XAML) implementation

```xml
<Window
    x:Class="EpoxyHello.Wpf.Views.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    viewmodels="clr-namespace:EpoxyHello.Wpf.ViewModels"
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

### Example of ViewModel (WPF) implementation

Completed separately `ViewModel` implementation.

```csharp
// Step 1: Create a ViewModel class. Then add the ViewModel attribute.
//    This attribute automatically implements PropertyChanged
//    so that it can be propagated to the XAML side.
[ViewModel]
public sealed class MainWindowViewModel
{
    // Step 2: Define the property you want to reference from XAML
    //    in the auto-implemented property.
    //    Epoxy can handle with C# 8.0's nullable reference types.
    public Command? Fetch { get; }
    public ObservableCollection<ImageSource>? Items { get; }

    public MainWindowViewModel()
    {
        // Step 3: Property setter will raise PropertyChanged events if value is changed.
        this.Items = new ObservableCollection<ItemViewModel>();

        // Step 4: A handler for fetch button.
        //   Ofcourse, we can use async/await safely in lambda expressions!
        this.Fetch = CommandFactory.Create(async () =>
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

### Example of Model implementation

The common code to access Reddit is implemented in the `EpoxyHello.Core` project.
It does not depend on either WPF, Xamarin Forms, Uno and UWP assemblies and is completely independent.

By eliminating dependencies in this way, we can achieve commonality for multi-platform support.
However, for small-scale development, you can place the `Model` implementation in the same project as the `ViewModel` implementation
(separating them eliminates the possibility of unintentional dependencies).

[Image downloader from Reddit post (EpoxyHello.Core)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Core/Models/Reddit.cs#L63):

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

----

## Features

Since each function is independent, it can be used in any combination.
(For example, it is NOT necessary to inherit from `ViewModel` to use it.)

|Function|Summary|
|:----|:----|
|ViewModel Injector|This function allows you to automatically implement the PropertyChanged event and other events required for ViewModel at build time. Simply apply the attributes to the target class, and you can skip the complicated code implementation.|
|ViewModel base class|The ViewModel Injector provides an orthodox base class for the ViewModel's PropertyChanged events, etc. It can be used in scenarios where the ViewModel Injector is not suitable.|
|CommandFactory|Enables arbitrary asynchronous delegates to be used as ICommand. You can safely implement asynchronous processing as an ICommand. |
|EventBinder|An attached property that allows you to bind CLR events of any XAML control as ICommand.|
|Anchor/Pile|Enables any XAML control to be temporarily and safely referenced from the ViewModel,eliminating all code binding and improving implementation visibility when using MVVM. The technique known as the Messenger pattern can also be integrated into the ViewModel with Anchor/Pile.|
|ValueConverter|Provides a base class for the XAML value converter. It provides a base class for XAML value converters, and can be implemented with type constraints in place.|
|UIThread|It can also be used as a continuation of asynchronous operations, so threads can be handled seamlessly in async-await and F# asynchronous workflows.|
|GlobalService|It is a compact infrastructure for dependency injection. It is very fast and simple, suitable for many scenarios, and can be implemented in a uniform way across platforms.|
|Designer|Supports design-time editing.|

Although it is not described in detail in the following sections, Epoxy is designed with asynchronous processing in mind throughout its functions.

* As a rule, method signatures use asynchronous (`ValueTask` return).
* Potentially misused overloads are absent or located only in the `Epoxy.Synchronized` namespace.
Synchronized namespace.
* Overloads that use `Task` are separated into the `Epoxy.Supplemental` namespace. This is to reduce the possibility of accidentally using `Task` instead of `ValueTask`.

----

### ViewModel injector and ViewModel base class

When the `ViewModel` attribute is applied,` PropertyChanging` and `PropertyChanged` are automatically implemented at compile time. Also, the auto-implemented property setter handles these events so that they occur automatically. This function called `ViewModel injector.`

In the previous implementation of Epoxy (<0.15), it was forced to inherit from the `ViewModel` base class, but by using this attribute, any class can be made into a ViewModel without any depends.

Also, if you apply the `IgnoreInject` attribute to a property, it can be excluded from the processing of `PropertyChanging` and `PropertyChanged`.

By adding the following signature method, you can easily add the process when changed the property:

```csharp
// Defined property
public string Title { get; set; }

// Called when the property changes.
// Signatures are not enforced, so the following conditions must be applied:
// * The argument is same type as the property (argument name is arbitrary)
// * Return value must be ValueTask type
// * Apply PropertyChangedAttribute. Specify the property name in the argument (The method name is free.)
//   * If you do not use the PropertyChanged attribute, set the method name to "On<property name>ChangedAsync."
[PropertyChanged(nameof(Title))]
private ValueTask TitleChangedAsync(string value)
{
    // What to do if the value changes ...
}
```

You can also derive and implement the `ViewModel` base class as before without using the `ViewModel injector`.

`ViewModel` base class provides an implementation of the `GetValue`/`SetValue` methods.
These methods automatically notify to the XAML control by property changes event `PropertyChanging`/`PropertyChanged`.
For example, when a property is changed upon a button click in `ViewModel`, the change will be notified to the XAML control and reflected to the user interface.

In addition, `GetValue` defines the default value,
and `SetValue` defines an overload that can perform additional operations when the value is changed.

If you don't use the ViewModel injector at entire in your project,
you can disable it to stop parsing code automatically and speed up the build.
Specify `False` for` EpoxyBuildEnable` of `PropertyGroup` of csproj.

----

### EventBinder

`EventBinder` allows binding of unbindable events as `Command` when they are exposed.
This feature avoids the practice of writing a code-behind for the sake of writing an event handler.

For example, you can bind the `Window.Loaded` event of WPF as follows:

```xml
<!-- Declared Epoxy namespace -->
<Window xmlns:epoxy="https://github.com/kekyo/Epoxy">

    <!-- ... -->

    <epoxy:EventBinder.Events>
        <!-- Binding the Window.Loaded event to the ViewModel's Ready property -->
        <epoxy:Event EventName="Loaded" Command="{Binding Ready}" />
    </epoxy:EventBinder.Events>
</Window>
```

On the `ViewModel` side, you can write handlers in Command, just like Button:

```csharp
// Defining the Command property for receiving Loaded events
public Command? Ready
{
    get => this.GetValue();
    private set => this.SetValue(value);
}

// ...

// Describe what to do when the Loaded event occurs.
this.Ready = CommandFactory.Create<EventArgs>(async _ =>
{
    // ex: Asynchronous acquisition of information to be displayed in the list from Model.
    foreach (var item in await Model.FetchInitialItemsAsync())
    {
        this.Items.Add(item);
    }
});
```

The generic argument of `CommandFactory.Create<T>` is the second argument of the event (usually a class that inherits from EventArgs).
Non-generic methods can also be used when event arguments are not required.

TIP 1: In WPF, UWP, and Xamarin Forms, you can use `Behavior` and `Trigger` to achieve the same thing.
However, WPF and UWP require additional packages and are designed to be generic,
so they are a bit more complex.
Using `EventBinder` has the advantage of being simple and using the same notation.

TIP 2: In a UWP environment (including UWP builds of Uno), the target event should have the following signature:

```csharp
// Events that can be bound by EventBinder
public event RoutedEventHandler Loaded;
```

In other words, only events published with the RoutedEventHandler type are eligible.
The UWP runtime environment has strict security checks.
This is because the UWP runtime environment has strict security checks, and there are restrictions when dynamically hooking events.

* [For example (In WPF XAML)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Wpf/Views/MainWindow.xaml#L36)
* [For example (In WPF view model)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Wpf/ViewModels/MainWindowViewModel.cs#L45)
* [For example (In Xamarin Forms XAML)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Xamarin.Forms/EpoxyHello.Xamarin.Forms/Views/MainPage.xaml#L33)
* [For example (In Xamarin Forms view model)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Xamarin.Forms/EpoxyHello.Xamarin.Forms/ViewModels/MainContentPageViewModel.cs#L40)

----

### Anchor/Pile

`Anchor`/`Pile` pair is a loose connection between `UIElement` (Xamarin Forms `Element`) and `ViewModel`s.

Rare case in MVVM architecture, we have to access directly `UIElement` member,
but sometimes gointg to wait the pitfall of circular references (and couldn't unbind by GC).

The `Pile` pull in the `UIElement`'s anchor, and we can rent temporary `UIElement` reference safely inside `ViewModel`.

```xml
<!-- Declared Epoxy namespace -->
<Window xmlns:epoxy="https://github.com/kekyo/Epoxy">

    <!-- ... -->

    <!-- Placed Anchor onto the TextBox and bound property -->
    <TextBox epoxy:Anchor.Pile="{Binding LogPile}" />
</Window>
```

```csharp
// Declared a Pile into the ViewModel.
this.LogPile = PileFactory.Create<TextBox>();

// ...

// Do rent by Pile when we have to manipulate the TextBox directly:
await this.LogPile.RentAsync(async textBox =>
{
    // Fetch information from related model.
    var result = await ServerAccessor.GetResultTextAsync();
    // We can manipulate safer directly TextBox.
    textBox.AppendText(result);
});
```

* [For example (In WPF XAML)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Wpf/Views/MainWindow.xaml#L39)
* [For example (In WPF view model)](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Wpf/ViewModels/MainWindowViewModel.cs#L74)

----

### ValueConverter

`ValueConverter` class is a base class for safely implementing the XAML converters.
It avoids cumbersome typecasting by explicitly specifying the type, and
It can also automatically fail to convert incompatible types.

You can give an argument to the converter with `ConverterParameter` on the XAML,
then you have to change the base class to use when you receive this parameter or not.

```csharp
// This is an implementation of a converter that takes an integer and converts it to a Brush.
// Specify the expected type as a generic argument.
public sealed class ScoreToBrushConverter : ValueConverter<int, Brush>
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
// Its type is specified by the generic second argument.
// Here is an example of receiving a string:
public sealed class ScoreToBrushConverter : ValueConverter<int, string, Brush>
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

* [For example](https://github.com/kekyo/Epoxy/blob/main/samples/EpoxyHello.Wpf/Views/Converters/ScoreToBrushConverter.cs#L25)

----

### UIThread

Some different platform contains different UI thread manipulation.
Epoxy can handle only one [UIThread class](https://github.com/kekyo/Epoxy/blob/main/Epoxy/UIThread.cs#L29),
it has commonly manipulation methods.
We can easier combine both UI manipulation and asynchronous operations.

```csharp
// Can check what current thread
Debug.Assert(await UIThread.IsBoundAsync());

// Invoke asynchronous operation and will detach current thread context.
var read = await httpStream.ReadAsync(...).ConfigureAwait(false);

// Executes on the worker thread.
Console.WriteLine($"Read={read}");

// Switches to the UI thread explicitly.
await UIThread.Bind();

// We can handle any UI elements in the UI thread (include binding operation.)
this.Log = $"Read={read}";

// Explicitly switch to worker thread.
await UIThread.Unbind();

// (Continued on worker thread)

// Temporary operations are performed on UI thread.
await UIThread.InvokeAsync(async () =>
    this.FetchedText = await httpStream.ReadStringAsync(...));
```

Another use of `UIThread.TryBind()` is to check if the switch to the UI thread was successful.
This is useful when the host UI framework (e.g., WPF) is aborting in progress,
then use `UIThread.TryBind()` to verify continue processing can be performed.

#### Note on running in a UWP environment

In the current implementation, if you use the `UIThread` class,
in the constructor of a `ViewModel` while building a `View` and like,
inside a UWP environment such as UWP native, Xamarin Forms/Uno, or a UWP-derived runtime such as WinUI,
you may not get the correct results.

UWP has a different UI thread assigned to each window that holds a view,
and if you use it while constructing an instance,
it will not be able to determine the view correctly.

----

### GlobalService (Advanced topic)

`GlobalService` class is an Epoxy implementation of techniques
such as dependency injection and dependency isolation.
Like other functions, it can safely implement asynchronous processing.

The key of dependency separation is to define a common interface type:

```csharp
// Commonly Sample.Xamarin.Forms project.

// Define platform-independent Bluetooth operations,
// applying the GlobalService attribute.
[GlobalService]
public interface IBluetoothAccessor
{
    // Start discovering Bluetooth.
    ValueTask BeginDiscoverAsync();
}
```

Then, in each platform's project, register the implementation of this interface.
The following is an example for Android:

```csharp
// In Sample.Xamarin.Forms.Android project:

// Implementation for Android Bluetooth.
public sealed class AndroidBluetoothAccessor : IBluetoothAccessor
{
    public async ValueTask BeginDiscoverAsync()
    {
        // Android-specific implementation...
    }
}

// The Application constructor
public Application()
{
    // Register a class instance that performs Android-dependent processing.
    GlobalService.Register(new AndroidBluetoothAccessor());
}
```

Now you can use separate implementations through interfaces in a common project:

```csharp
// Commonly Sample.Xamarin.Forms project.

// I want to use Bluetooth:
await GlobalService.ExecuteAsync<IBluetoothAccessor>(async accessor =>
{
    // Start discovering Bluetooth.
    await accessor.BeginDiscoverAsync();

    // ...
});

```

Existing libraries for dependency injection and dependency isolation
(e.g. `DependencyService` class, Unity, MEF, etc.) have the following problems:

* Have complex functionality: In many situations, you simply want an instance that implements
a common interface, so the `GlobalService` class allows you to perform such operations in a fast way.
* If the retrieved instance is retained, it is not possible to manage the lifetime:
Since it is fast, it is not a problem to call `ExecuteAsync` every time.
Rather, it is preferable to use it only when necessary, each time.

NOTE: As the name "Global" implies, `GlobalService` behaves like a kind of global variable.
Try not to use `GlobalService` in places where it is not really needed.
`Epoxy.Advanced` namespace (using declarations are required) to make it a bit more distinguishable.

----

### Designer (advanced topic)

The `Designer` class can be used to describe processes related to design editing.

If you implement a custom control or user control,
when an IDE (such as Visual Studio or Rider) is doing visual editing of a control,
it is possible to actually create an instance of the control within the IDE.

In such cases, you may want to change the look and behavior of
your design edits rather than doing the real control behavior.

By referring to `IsDesignTime` property, you can get whether or
not you are editing at design time in a platform-independent way.

----

## About the F# version

By using the F# version of the package, you can write code that follows the F# style as follows.
The instances used are shared, you can use the preferred API for both C# and F# while maintaining the same instances.

For an explanation of using Epoxy in F#, please refer to the slides previously presented in a meetup event:

[F# Epoxy - Fun Fan F# (If could not see)](https://speakerdeck.com/kekyo/f-number-epoxy-english-translated)

### Camel-case function names

All functions in FSharp.Epoxy are camel-cased. For example, instead of the `GetValue`/`SetValue` methods in the `ViewModel` base class, use the `getValue`/`setValue` functions.

```fsharp
open Epoxy
type ItemViewModel() =
    inherit ViewModel()

    // Use the getValue and setValue functions to transfer properties.
    // You can write the type where type inference is effective,
    // so get() and set() can be written with type annotations.
    member __.Title
        with get(): string = __.getValue()
        and set (value: string) = __.setValue value
```

### Direct support for F# types

Function types instead of delegate types, and `Option` types instead of out parameters, to make it easier for F# to handle them.

```fsharp
// Arguments that receive a delegate can directly receive
// F# function type instead.
self.Ready <- Command.Factory.createSync(fun (e:RoutedEventArgs) ->
    self.IsEnabled <- true)
```

```fsharp
type public ScoreToBrushConverter() =
    inherit ValueConverter<int, Brush>()

    // The convert function has no out parameter and
    // can be written to return the 'T option.
    override __.convert from =
        if from >= 5 then Some yellow else Some gray
```

### Use the type `Async` instead of `ValueTask` as the default asynchronous type

Basically, all asynchronous operations are designed to be described smoothly with the type `Async`.

```fsharp
// Since the default function definitions are all defined to
// accept F#'s `Async` type, so we can use asynchronous workflows
// with `async { ... }`.
self.Fetch <- CommandFactory.create(fun () -> async {
    let! reddits = Reddit.fetchNewPostsAsync "r/aww"
    // ...
})
```

When used in conjunction with my other project [FusionTasks](https://github.com/kekyo/FSharp.Control.FusionTasks), it makes easier to work with existing libraries that use `Task`/`ValueTask` (such as `HttpClient.GetAsync`). dotnet CLI templates are enabled by default.

`Epoxy.Supplements` namespace should be explicitly imported when directly passing methods that return `Task` or `ValueTask` types, or when providing computation expressions that constitute these types.

NOTE: The preference for the `Async` type may change when [the `resumable` structure is released in a future F# release.](https://github.com/dotnet/fsharp/pull/6811)

### ViewModel Injector

You can also use the `ViewModel injector` in F#. However, there are syntactic restrictions on auto-implemented properties:

```fsharp
open Epoxy

// Use ViewModel injector.
[<ViewModel>]
type ItemViewModel() as self =
    do
        // This expression usually raises an exception,
        // but it is legal when using the ViewModel injector.
        self.Title <- "CCC"
        // You can use this behavior to assign a Command within the `do` block.
        self.Click <- CommandFactory.create(fun () -> async {
            // ...
        })

    // The F# auto-implemented property requires an initialization expression,
    // but it ignored if assigned any instance in the `do` block.
    member val Title = "AAA" with get, set
    member val Body = "BBB" with get, set
    member val Click: Command = null with get, set

// Result:
let vm = new ItemViewModel()
Debug.Assert(vm.Title = "CCC")
Debug.Assert(vm.Body = "BBB")
```

The `IgnoreInject` attribute can be used in F# as well. The process when changing properties returns `Async<unit>`:

```fsharp
// Defined property
member val Title = "Unknown"
    with get, set

// Called when the property changes.
// Signatures are not enforced, so the following conditions must be applied:
// * The argument is same type as the property (argument name is arbitrary)
// * Return value must be Async<unit> type
// * Apply PropertyChangedAttribute. Specify the property name in the argument (The method name is free.)
//   * If you do not use the PropertyChanged attribute, set the method name to "on<property name>ChangedAsync."
[<PropertyChanged("Title")>]
member self.titleChangedAsync (value: string) = async {
    // What to do if the value changes ...
}
```

### Automatic resourceization of WPF XAML pages

So the XAML build action is automatically changed so that it is added to the project as a resource. When you add the XAML file to the project, you don't need to do anything in particular to set it up correctly.

One limitation of this feature is that XAML is always stored in the resource as raw source code (XML text) and is not converted to binary (BAML). Also, the assembly name must always be specified in the XAML namespace in order to be able to reference the type at runtime:

```xml
<!-- Always add the assembly directive to the clr-namespace directive -->
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:viewmodels="clr-namespace:EpoxyHello.ViewModels;assembly=EpoxyHello"
    Title="EpoxyHello" Height="450" Width="800">

    <Window.DataContext>
        <viewmodels:MainWindowViewModel />
    </Window.DataContext>

    <!-- ... -->
</Window>
```

----

## License

Apache-v2

## History

* 1.7.0:
  * Fixed an error in a custom task when building a project containing Epoxy on MacOS.
* 1.6.0:
  * Added `UIThread.TryBind()`, `UIThread.InvokeAsync()` and `UIThread.TryInvokeAsync()`.
* 1.5.0:
  * Added `UIThread.Unbind()`.
  * Fixed broken and updated latest package versions WinUI 3 (`Microsoft.WindowsAppSDK` 1.0.0).
  * Added project template for WinUI 3.
  * Arguments for commands to be bound to `EventBinder` are now treated as optional. If not needed, the non-generic `CommandFactory.Create(() => ...)` can be used.
  * Updated project templates with `ViewModel` attribute when it isn't used.
* 1.4.0:
  * Updated latest package versions Xamarin Forms.
  * Fixed unable to find Anchor.Pile property on Xamarin.Forms.
* 1.3.0:
  * Updated latest package versions both Avalonia and Xamarin Forms.
  * Fixed implicitly requirements for installation .NET Core 2.0 environment.
* 1.2.0:
  * Supported .NET 6.0.
  * Supported OpenSilver.
  * Renewed latest packages except UWP, WinUI and Uno.
  * Removed WinUI sample template (You can refer `samples` directory if you need it).
  * Supported Visual Studio project wizard.
* 1.1.1:
  * Fixed causes NU1201 when lacks tfm platform version suffix in WPF packages.
* 1.1.0:
  * Added `PropertyChanged` attribute so that the handler target when PropertyChanged occurs can be specified by the attribute. [See #8](https://github.com/kekyo/Epoxy/issues/8)
  * Obsoleted `ExecuteAsync` in Anchor/Pile and added the alias `RentAsync` instead. [See #9](https://github.com/kekyo/Epoxy/issues/9)
  * By specifying `EpoxyBuildEnable` in the project, the ViewModel injector can be stopped completely. [See #6](https://github.com/kekyo/Epoxy/issues/6)
  * Updated dependent packages (Uno.UI: 3.7.6, but unverified except for UWP hosts)
* 1.0.0:
  * Reached 1.0 🎉
  * Omitted ChildrenAnchor/ChildrenPile/ChildrenBinder. [See #5](https://github.com/kekyo/Epoxy/issues/5)
* 0.17.0:
  * Added ChildrenAnchor and ChildrenPile. ChildrenBinder will be discontinued.
  * Expanded XML comments.
  * Minor interface cleanups and fixes.
  * Will be close formal release :)
* 0.16.0:
  * Added IgnoreInject attribute and support for custom SetValue handlers to ViewModel injectors.
  * Fixed failure injecting on only installed sdk3.1 or 5.0.
* 0.15.0:
  * Added ViewModel injector function that enables automatic implementation of ViewModel.
  * Added F#'s camel-casing UIThread API.
  * Made safer handler for catching excedptions on Command infrastructure.
* 0.14.0:
  * Added XamlDesigner class.
  * Added Avalonia runtime platforms on net48, netcoreapp2.1, netcoreapp3.1 and net5.0.
  * Added F#'s camel-casing API entry points.
  * Downgraded FSharp.Core version from 5.0.1 to 5.0.0.
  * Fixed runnable view model on Avalonia XAML design time.
  * Fixed failure editing on Avalonia C# XAML designer.
* 0.13.0:
  * Added dotnet CLI templates.
  * Improved UIThread detection on WinUI platform.
  * In F# WPF NuGet package, it will place XAML code into assembly resource automatically.
* 0.11.0:
  * Added F# support.
  * Swapped ValueConverter generic arguments. (Breaking)
  * Moved some factory methods into "Factory" marked types. (Breaking)
* 0.10.0:
  * Supported WinUI platform.
* 0.9.0:
  * Supported Uno platform and Avalonia.
* 0.8.0:
  * Added GlobalService and EventBinder features.
* 0.7.0:
  * Added Xamarin Forms sample code.
* 0.6.0:
  * Split synchronous Command handler.
* 0.5.0:
  * Added UIThread and Anchor/Pile features.
