@echo off

git clean -xfd
mkdir artifacts

echo.
echo "==================================================="
echo "Building packages"

dotnet build -p:Configuration=Release Epoxy-build.sln

echo.
echo "==================================================="
echo "Building packages (OpenSilver)"

msbuild -p:Configuration=Release -maxCpuCount -t:restore src\Epoxy.Core.OpenSilver\Epoxy.Core.OpenSilver.csproj
msbuild -p:Configuration=Release -maxCpuCount src\Epoxy.Core.OpenSilver\Epoxy.Core.OpenSilver.csproj
msbuild -p:Configuration=Release -maxCpuCount -t:restore src\Epoxy.OpenSilver\Epoxy.OpenSilver.csproj
msbuild -p:Configuration=Release -maxCpuCount src\Epoxy.OpenSilver\Epoxy.OpenSilver.csproj
msbuild -p:Configuration=Release -maxCpuCount -t:restore src\FSharp.Epoxy.OpenSilver\FSharp.Epoxy.OpenSilver.fsproj
msbuild -p:Configuration=Release -maxCpuCount src\FSharp.Epoxy.OpenSilver\FSharp.Epoxy.OpenSilver.fsproj

echo.
echo "==================================================="
echo "Packing packages"

dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Core.Wpf\Epoxy.Core.Wpf.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Core.Xamarin.Forms\Epoxy.Core.Xamarin.Forms.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Core.Avalonia\Epoxy.Core.Avalonia.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Core.Avalonia11\Epoxy.Core.Avalonia11.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.Core.OpenSilver\Epoxy.Core.OpenSilver.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Build\Epoxy.Build.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Wpf\Epoxy.Wpf.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Xamarin.Forms\Epoxy.Xamarin.Forms.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Avalonia\Epoxy.Avalonia.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Avalonia11\Epoxy.Avalonia11.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\Epoxy.OpenSilver\Epoxy.OpenSilver.csproj
dotnet pack -p:Configuration=Release -o artifacts src\FSharp.Epoxy.Wpf\FSharp.Epoxy.Wpf.fsproj
dotnet pack -p:Configuration=Release -o artifacts src\FSharp.Epoxy.Avalonia\FSharp.Epoxy.Avalonia.fsproj
dotnet pack -p:Configuration=Release -o artifacts src\FSharp.Epoxy.Avalonia11\FSharp.Epoxy.Avalonia11.fsproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\..\artifacts src\FSharp.Epoxy.OpenSilver\FSharp.Epoxy.OpenSilver.fsproj
dotnet build -p:Configuration=Release templates\Epoxy.Templates.csproj
dotnet pack -p:Configuration=Release -o artifacts templates\Epoxy.Templates.csproj
