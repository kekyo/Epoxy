@echo off

git clean -xfd
mkdir artifacts

dotnet restore Epoxy.sln
dotnet restore templates\Epoxy.Templates.csproj

msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Core.Wpf\Epoxy.Core.Wpf.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Core.Xamarin.Forms\Epoxy.Core.Xamarin.Forms.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Core.Avalonia\Epoxy.Core.Avalonia.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Core.OpenSilver\Epoxy.Core.OpenSilver.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Core.Uwp\Epoxy.Core.Uwp.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Core.WinUI\Epoxy.Core.WinUI.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Core.Maui\Epoxy.Core.Maui.csproj

msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Build\Epoxy.Build.csproj

msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Wpf\Epoxy.Wpf.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Xamarin.Forms\Epoxy.Xamarin.Forms.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Avalonia\Epoxy.Avalonia.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.OpenSilver\Epoxy.OpenSilver.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Uwp\Epoxy.Uwp.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.WinUI\Epoxy.WinUI.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Maui\Epoxy.Maui.csproj

msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\FSharp.Epoxy.Wpf\FSharp.Epoxy.Wpf.fsproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\FSharp.Epoxy.Avalonia\FSharp.Epoxy.Avalonia.fsproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\FSharp.Epoxy.OpenSilver\FSharp.Epoxy.OpenSilver.fsproj

msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\artifacts templates\Epoxy.Templates.csproj
