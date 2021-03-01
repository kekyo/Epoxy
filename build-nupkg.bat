@echo off
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Wpf.Core\Epoxy.Wpf.Core.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Xamarin.Forms.Core\Epoxy.Xamarin.Forms.Core.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Avalonia.Core\Epoxy.Avalonia.Core.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Uwp.Core\Epoxy.Uwp.Core.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Uno.Core\Epoxy.Uno.Core.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.WinUI.Core\Epoxy.WinUI.Core.csproj

msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Wpf\Epoxy.Wpf.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Xamarin.Forms\Epoxy.Xamarin.Forms.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Avalonia\Epoxy.Avalonia.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Uwp\Epoxy.Uwp.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Uno\Epoxy.Uno.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.WinUI\Epoxy.WinUI.csproj

msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\FSharp.Epoxy.Wpf\FSharp.Epoxy.Wpf.fsproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\FSharp.Epoxy.Avalonia\FSharp.Epoxy.Avalonia.fsproj
