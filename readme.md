# Package the webapi for Veracode

```shell
dotnet publish -c Debug -o OutputProject -p:UseAppHost=false -p:SatelliteResourceLanguages='en' ./webapi/poc-proj-api-01.csproj
```