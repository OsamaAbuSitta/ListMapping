dotnet build --configuration Release
dotnet pack --configuration Release





dotnet nuget push bin/Release/ListMapping.1.0.1.nupkg --api-key {key} --source https://api.nuget.org/v3/index.json

NuGet\Install-Package ListMapping -Version 1.0.0

dotnet add package ListMapping --version 1.0.0