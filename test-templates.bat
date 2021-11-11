@echo off

rmdir /s/q test-templates
mkdir test-templates
cd test-templates

echo "Install Epoxy.Templates..."

rem dotnet new -i Epoxy.Templates
dotnet new -i Epoxy.Templates --nuget-source https://www.myget.org/F/epoxy/api/v2

rem ===========================================================
echo "Testing epoxy-wpf"

mkdir epoxy_wpf
cd epoxy_wpf
dotnet new epoxy-wpf

copy /y ..\..\test-templates-nuget.config nuget.config

dotnet restore
dotnet build

cd ..

rem ===========================================================
echo "Testing epoxy-wpf F#"

mkdir epoxy_wpf_fsharp
cd epoxy_wpf_fsharp
dotnet new epoxy-wpf -lang=F#

copy /y ..\..\test-templates-nuget.config nuget.config

dotnet restore
dotnet build

cd ..

rem ===========================================================
echo "Testing epoxy-avalonia"

mkdir epoxy_avalonia
cd epoxy_avalonia
dotnet new epoxy-avalonia

copy /y ..\..\test-templates-nuget.config nuget.config

dotnet restore
dotnet build

cd ..

rem ===========================================================
echo "Testing epoxy-avalonia F#"

mkdir epoxy_avalonia_fsharp
cd epoxy_avalonia_fsharp
dotnet new epoxy-avalonia -lang=F#

copy /y ..\..\test-templates-nuget.config nuget.config

dotnet restore
dotnet build

cd ..

rem ===========================================================
echo "Testing epoxy-opensilver"

mkdir epoxy_opensilver
cd epoxy_opensilver
dotnet new epoxy-opensilver

copy /y ..\..\test-templates-nuget.config nuget.config

dotnet restore
dotnet build

cd ..

rem ===========================================================
echo "Testing epoxy-xamarin-forms"

mkdir epoxy_xamarin_forms
cd epoxy_xamarin_forms
dotnet new epoxy-xamarin-forms

copy /y ..\..\test-templates-nuget.config nuget.config

dotnet restore
rem msbuild -t:build

cd ..

rem ===========================================================
echo "Testing epoxy-uwp"

mkdir epoxy_uwp
cd epoxy_uwp
dotnet new epoxy-uwp

copy /y ..\..\test-templates-nuget.config nuget.config

dotnet restore
rem msbuild -t:build

cd ..

cd ..
