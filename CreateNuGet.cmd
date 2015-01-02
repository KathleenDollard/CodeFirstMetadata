set version=1.0.20-alpha
if not exist .\nuget_packages mkdir nuget_packages
del /Q .\nuget_packages\*.*
.nuget\NuGet.exe pack CodeFirstMetadata.nuspec -OutputDirectory .\nuget_packages -Version %version% -symbols

REM .nuget\NuGet.exe pack CodeFirstMetadata\CodeFirstMetadata.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols
REM .nuget\NuGet.exe pack CodeFirstCSharp\CodeFirstCSharp.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols
REM .nuget\NuGet.exe pack CodeFirstMetadataT4Support\CodeFirstMetadataT4Support.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols
REM .nuget\NuGet.exe pack CodeFirstMetadataProvider\CodeFirstMetadataProvider.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols
REM .nuget\NuGet.exe pack SqlSchema\SqlSchema.csproj -IncludeReferencedProjects -OutputDirectory .\nuget_packages -Version %version% -symbols

copy nuget_packages\*.* ..\LocalNuGet
pause 