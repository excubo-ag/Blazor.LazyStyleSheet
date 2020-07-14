# Component Library for use in consumer apps

This example handles how you can create component libraries that use Excubo.Blazor.LazyStyleSheet where your users can define their custom colors.
This folder includes the component library (which get's published as `nupkg`) and a sample app consuming that component library.

## Relevant files in ComponentLibrary

### ComponentLibrary/ComponentLibrary/ComponentWithUserDefinedColor.razor

```html
<div class="component-style">
    This Blazor component is defined in the <strong>ComponentLibrary</strong> package.
</div>
```

- we use a custom style called `component-style`.


### ComponentLibrary/ComponentLibrary/ComponentWithUserDefinedColor.razor.scss

```css
@import '../component_library_config.scss';
.component-style {
    color: $variable
}
```

- the custom style for the component above. It uses a variable defined in a config file.

### ComponentLibrary/ComponentLibrary/wwwroot/component_library_config.scss

```css
$variable:red;
```

- the default value that users get when using your component library for the very first time.

### ComponentLibrary/ComponentLibrary/build/ComponentLibrary.targets

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <ItemGroup>
    <CssResources Include="$(MSBuildThisFileDirectory)..\staticwebassets\component_library\*" />
    <DefaultValues Include="$(MSBuildThisFileDirectory)..\staticwebassets\component_library_config.scss" />
  </ItemGroup>
  <Target Name="CopyResources" BeforeTargets="BeforeCompile" >
    <Message Importance="high" Text="Copying resources to consumer app!" />
    <Message Importance="high" Text="First up, all resources, except the default config" />
    <Message Importance="high" Text="@(CssResources)" />
    <Copy SourceFiles="@(CssResources)"
          DestinationFolder="$(MsBuildProjectDirectory)\wwwroot\component_library\" />
    <Message Importance="high" Text="Now what's left is the default config, which we only write if it doesn't exist yet" />
    <Message Importance="high" Text="@(DefaultValues)" />
    <Copy SourceFiles="@(DefaultValues)"
          DestinationFolder="$(MsBuildProjectDirectory)\wwwroot\"
          Condition="!Exists('$(MsBuildProjectDirectory)\wwwroot\component_library_config.scss') " />
  </Target>
</Project>
```

This file is responsible for copying the scss files into the users application. There, the user will have the possibility to customize everything in `wwwroot\component_library_config.scss`. The `Message` commands are only for illustration.


### ComponentLibrary/ComponentLibrary/ComponentLibrary.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <RazorLangVersion>3.0</RazorLangVersion>
    <GeneratePackageOnBuild>true</GeneratePackageOnBuild>
    <LazyStyleSheets_ComponentStyleFolder>component_library</LazyStyleSheets_ComponentStyleFolder>
    <LazyStyleSheets_UseWebCompiler>false</LazyStyleSheets_UseWebCompiler>
    
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Excubo.Blazor.LazyStyleSheet" Version="3.1.1" />
    <PackageReference Include="Microsoft.AspNetCore.Components" Version="3.1.3" />
    <PackageReference Include="Microsoft.AspNetCore.Components.Web" Version="3.1.3" />
    <PackageReference Include="Microsoft.Build.Framework" Version="16.5.0" />
    <PackageReference Include="Microsoft.Build.Utilities.Core" Version="16.5.0" />
  </ItemGroup>

  <ItemGroup>
    <Folder Include="build\" />
  </ItemGroup>
  <ItemGroup>
    <Content Include="build\*.targets" PackagePath="build\" />
  </ItemGroup>

</Project>

```

- added `<PackageReference Include="Excubo.Blazor.LazyStyleSheet" Version="3.1.1" />`.
- `LazyStyleSheets` is configured to use a non-default output path `component_library`.
- `LazyStyleSheets` is configured to not use `webcompiler` (because we want to define our own colors in the consumers' app).
- we include the `build` targets in the nuget package.

## What to tell your users

All of the above is what's necessary in your component library. What's left now is what to tell your users to do with the library

### ConsumerApp/BlazorApp/_Imports.razor

```cs
@using System.Net.Http
@using Microsoft.AspNetCore.Authorization
@using Microsoft.AspNetCore.Components.Authorization
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.JSInterop
@using BlazorApp
@using BlazorApp.Shared
@using ComponentLibrary
```

- added `@using ComponentLibrary`

### ConsumerApp/BlazorApp/Pages/Index.razor

```html
@page "/"

<h1>Hello, world!</h1>

Welcome to your new app.

<ComponentWithUserDefinedColor />

<SurveyPrompt Title="How is Blazor working for you?" />
```

- use the `<ComponentWithUserDefinedColor />` component in the index page (so that we see it on the first page load).


### ConsumerApp/BlazorApp/wwwroot/component_library_config.scss

```css
$variable:green;
```

- here your user can define their custom color `variable`, e.g. to be `green`.

### BlazorApp/BlazorApp.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="ComponentLibrary" Version="1.0.0" />
  </ItemGroup>

  <Target Name="CompileResources" AfterTargets="CoreCompile">
    <Exec Command="webcompiler -r wwwroot" />
  </Target>
</Project>

```

- the BlazorApp has a dependency on the nuget package `ComponentLibrary`: `<PackageReference Include="ComponentLibrary" Version="1.0.0" />`

 - Now that we have the style sheets from the component library and have the variables defined (`BlazorApp/wwwroot/component_library_config.scss`), we want `webcompiler` to compile those scss files:
 ```xml
  <Target Name="CompileWithUserDefinedColors" AfterTargets="CoreCompile">
    <Exec Command="webcompiler -r wwwroot/css" />
  </Target>
 ```

The user can of course any other pipeline to compile scss files.

