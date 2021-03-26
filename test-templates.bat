@echo off

del /s/q test-templates
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

cd ..
