rem .nuget\NuGet.exe setapikey e39ea-get-the-full-key-on-nuget.org

Call CreateNuget.cmd
.nuget\NuGet.exe push nuget_packages\CodeFirst.1.0.??-alpha.nupkg
.nuget\NuGet.exe push nuget_packages\CodeFirst.1.0.??-alpha.symbols.nupkg
.nuget\NuGet.exe push nuget_packages\CodeFirstT4Example.1.0.??-alpha.nupkg
.nuget\NuGet.exe push nuget_packages\CodeFirstT4Example.1.0.??-alpha.symbols.nupkg

pause 