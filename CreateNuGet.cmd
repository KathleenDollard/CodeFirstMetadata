set version=1.0.2-alpha
if not exist .\nuget_packages mkdir nuget_packages
del /Q .\nuget_packages\*.*
.nuget\NuGet.exe pack CodeFirstMetadata\CodeFirstMetadata.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols
.nuget\NuGet.exe pack CodeFirstT4Example\CodeFirstT4Example.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols
pause 