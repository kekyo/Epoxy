# Epoxy WPF template (F#)

## Resources

* [Epoxy GitHub repository](https://github.com/kekyo/Epoxy)
* [You can apply license headers from template](https://marketplace.visualstudio.com/items?itemName=StefanWenig.LicenseHeaderManager)

## Projects

* `EpoxyHello.Core`: Independent common component project includes MVVM `Model` code.
* `EpoxyHello`: The WPF application project includes MVVM `View` and `ViewModel` code.

## TIPS

* F# on WPF has limitation for XAML related partially depending build tasks with C#.
  So, you can embed XAML code file annotated `Resource` instead `Page` on
  Visual Studio property or edit directly tag on fsproj script.
