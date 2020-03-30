@echo off
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\artifacts Epoxy.Wpf\Epoxy.Wpf.csproj
msbuild -t:pack -p:Configuration=Release -p:PackageOutputPath=..\artifacts Epoxy.Xamarin.Forms\Epoxy.Xamarin.Forms.csproj
msbuild -p:Configuration=Release Epoxy.Uwp\Epoxy.Uwp.csproj
.nuget\nuget.exe pack Epoxy.Uwp\Epoxy.Uwp.nuspec -Properties Configuration=Release -OutputDirectory artifacts -Properties version=0.0.1
