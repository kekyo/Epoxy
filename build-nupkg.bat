@echo off

git clean -xfd
mkdir artifacts

dotnet restore Epoxy.sln
dotnet restore templates\Epoxy.Templates.csproj

dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Core.Wpf\Epoxy.Core.Wpf.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Core.Xamarin.Forms\Epoxy.Core.Xamarin.Forms.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Core.Avalonia\Epoxy.Core.Avalonia.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Core.Avalonia11\Epoxy.Core.Avalonia11.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Core.OpenSilver\Epoxy.Core.OpenSilver.csproj

dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Build\Epoxy.Build.csproj

dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Wpf\Epoxy.Wpf.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Xamarin.Forms\Epoxy.Xamarin.Forms.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Avalonia\Epoxy.Avalonia.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.Avalonia11\Epoxy.Avalonia11.csproj
dotnet pack -p:Configuration=Release -o artifacts src\Epoxy.OpenSilver\Epoxy.OpenSilver.csproj

dotnet pack -p:Configuration=Release -o artifacts src\FSharp.Epoxy.Wpf\FSharp.Epoxy.Wpf.fsproj
dotnet pack -p:Configuration=Release -o artifacts src\FSharp.Epoxy.Avalonia\FSharp.Epoxy.Avalonia.fsproj
dotnet pack -p:Configuration=Release -o artifacts src\FSharp.Epoxy.Avalonia11\FSharp.Epoxy.Avalonia11.fsproj
dotnet pack -p:Configuration=Release -o artifacts src\FSharp.Epoxy.OpenSilver\FSharp.Epoxy.OpenSilver.fsproj

dotnet pack -p:Configuration=Release -o artifacts templates\Epoxy.Templates.csproj
