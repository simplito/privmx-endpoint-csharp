## Adding/Removing support for native platform
To add support for new native platform do:
- create/delete new subdirectory in `native` directory, folder name must correspond with [RID](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog) of target platform
- remove platform `RID` from `RuntimeIdentifiers` in [PrivmxEndpointCsharp.csproj](./PrivmxEndpointCsharp/PrivmxEndpointCsharp.csproj)
- add/remove configuration responsible for packing native libraries to nuget in [PrivmxEndpointCsharp.csproj](./PrivmxEndpointCsharp/PrivmxEndpointCsharp.csproj) 

## Creating nuget release with native packages
1. Update version in [PrivmEndpointCsharp.csproj](./PrivmxEndpointCsharp/PrivmxEndpointCsharp.csproj)
2. Add native libraries to subdirectories in `native` directory. If you have doubts on what kind of files should go to the directories check:
   - [Including native libraries in .NET packages](https://learn.microsoft.com/en-us/nuget/create-packages/native-files-in-net-packages)
   - [.NET RID Catalog](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)
3. change PrivmxEndpointCsharp license fields in [PrivmEndpointCsharp.csproj](./PrivmxEndpointCsharp/PrivmxEndpointCsharp.csproj):
   - replace 
     ```
     <PropertyGroup>
         <PackageLicenseExpression>MIT</PackageLicenseExpression>
     </PropertyGroup>
     ``` 
     with
     ```
     <PropertyGroup>
         <PackageLicenseFile>LICENSE</PackageLicenseFile>
         <PackageRequireLicenseAcceptance>true</PackageRequireLicenseAcceptance>
     </PropertyGroup>
     ```
4. Get latest `PrivMX Free License` file from `https://cdn.privmx.dev/cdn/privmx-cms/download/privmx_www/assets/PrivMX%20Free%20License.pdf` and add it to project files as `./PrivmxEndpointCsharp/LICENSE`
5. execute `dotnet pack ./PrivmxEndpointCsharp`

**Remember to clear nuget cache when testing the package**