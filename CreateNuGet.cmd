set version=1.0.13-alpha
if not exist .\nuget_packages mkdir nuget_packages
del /Q .\nuget_packages\*.*
.nuget\NuGet.exe pack CodeFirstMetadata\CodeFirstMetadata.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols
.nuget\NuGet.exe pack CodeFirstCSharp\CodeFirstCSharp.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols
.nuget\NuGet.exe pack CodeFirstMetadataT4Support\CodeFirstMetadataT4Support.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols
.nuget\NuGet.exe pack CodeFirstMetadataProvider\CodeFirstMetadataProvider.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols
.nuget\NuGet.exe pack SqlSchema\SqlSchema.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols
pause 