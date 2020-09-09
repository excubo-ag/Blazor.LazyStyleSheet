## Excubo.Blazor.LazyStyleSheet

[![Nuget](https://img.shields.io/nuget/v/Excubo.Blazor.LazyStyleSheet)](https://www.nuget.org/packages/Excubo.Blazor.LazyStyleSheet/)
[![Nuget](https://img.shields.io/nuget/dt/Excubo.Blazor.LazyStyleSheet)](https://www.nuget.org/packages/Excubo.Blazor.LazyStyleSheet/)
[![GitHub](https://img.shields.io/github/license/excubo-ag/Blazor.LazyStyleSheet)](https://github.com/excubo-ag/Blazor.LazyStyleSheet)

A major issue on websites is slow page load. In part, this is due to enormous payloads that need to be downloaded in full before a page can be rendered correctly. Minimization and compression help to some degree, but it ignores the awkward fact that many style sheets are only used to a tiny fraction.

With HTTP/2, loading small files rather than one large one is less of a performance concern than with HTTP/1.1. Since Blazor uses HTTP/2 by default, we can make use of this and split style sheets into smaller chunks. Those chunks can then be loaded lazily, i.e. only when a component actually needs it.

Excubo.Blazor.LazyStyleSheet enables you to write dedicated style sheet for each component.

## Breaking changes

### Version 3.X.Y

Good news! Adding lazy-loaded style sheets to your component just became a whole lot easier. Simply add `<Stylesheet Src="path/to/my/style.css" />` to any component.
If you write your styles as a `Component.razor.css` file, you don't need to do anything.
And you can now also remove the `<StyleSheets />` component in your `App.razor`, as well as the dependency injection code in your `Startup.cs`.

### Version 2.X.Y

`Excubo.Blazor.LazyStyleSheet` now contains build tasks to automatically inject the `IStyleSheetService` when you write your component style as a `Component.razor.css` or `Component.razor.scss` file. That means, if you previously manually inserted `IStyleSheetService` into your component, you now have to remove that.

## How to use

### 1. Install the nuget package Excubo.Blazor.LazyStyleSheet

Excubo.Blazor.LazyStyleSheet is distributed [via nuget.org](https://www.nuget.org/packages/Excubo.Blazor.LazyStyleSheet/).
[![Nuget](https://img.shields.io/nuget/v/Excubo.Blazor.LazyStyleSheet)]((https://www.nuget.org/packages/Excubo.Blazor.LazyStyleSheet/))

#### Package Manager:
```ps
Install-Package Excubo.Blazor.LazyStyleSheet -Version 3.1.5
```

#### .NET Cli:
```cmd
dotnet add package Excubo.Blazor.LazyStyleSheet --version 3.1.5
```

#### Package Reference
```xml
<PackageReference Include="Excubo.Blazor.LazyStyleSheet" Version="3.1.5" />
```

### 2a. Write your style sheets and put them next to your component

`MyComponent.razor`:
```razor
@page "/hello"

<div class="mystyle">My styled component</div>

```

`MyComponent.razor.css` / `MyComponent.razor.scss`: 
```css
.mystyle {
    color: purple
}
```

### 2b. Load any stylesheet in your component

`MyComponent.razor`:
```razor
@page "/hello"

<Stylesheet Src="path/to/my/style.min.css" />
<div class="mystyle">My styled component</div>

```

## Remark

 - Style sheet urls may be added any number of times, and will only be added to the DOM once (as duplicate `<link>` tags don't achieve anything). This only applies if the url string matches exactly, i.e. there is a difference between `https://localhost/css/style.css` and `css/style.css`.


## Tips & tricks

### Integration with webcompiler

`Excubo.Blazor.LazyStyleSheet` integrates seemlessly with [`Excubo.WebCompiler`](https://github.com/excubo-ag/WebCompiler). If you have webcompiler installed, a build task will take care of scss/sass compilation, minification, and compression. The use of webcompiler is strictly optional, but recommended and active by default.

### Configuration options

This library can be configured by adding values to your `csproj` file:

```xml
  <PropertyGroup>
    <LazyStyleSheets_StaticAssetFolder>wwwroot</LazyStyleSheets_StaticAssetFolder>
    <LazyStyleSheets_ComponentStyleFolder>css/components</LazyStyleSheets_ComponentStyleFolder>
    <LazyStyleSheets_UseMinifiedStyleSheets>true</LazyStyleSheets_UseMinifiedStyleSheets>
    <LazyStyleSheets_UseGzippedStyleSheets>false</LazyStyleSheets_UseGzippedStyleSheets>
    <LazyStyleSheets_UseWebCompiler>true</LazyStyleSheets_UseWebCompiler>
    <LazyStyleSheets_AutoInject>true</LazyStyleSheets_AutoInject>
  </PropertyGroup>
```

#### StaticAssetFolder

The static asset folder should be set to the name of the folder where all your static assets are. By default, that's `wwwroot` and does not need to be changed.

#### ComponentStyleFolder

`Excubo.Blazor.LazyStyleSheet` puts all `*.razor.css` and `*.razor.scss` files into a subfolder of the static asset folder, to separate them from other styles. The default location is `css/components`, so the full path becomes `wwwroot/css/components` by default.

#### UseMinifiedStyleSheets

`Excubo.Blazor.LazyStyleSheet` uses [`Excubo.WebCompiler`](https://github.com/excubo-ag/WebCompiler), if installed. It then generates minified, and compressed versions of your style sheet automatically. By default, `Excubo.Blazor.LazyStyleSheet` then uses the minified version to dynamically and lazily load the style sheet.
If you do not have `Excubo.WebCompiler` installed, and you do not generate minified versions of your style sheets any other way, you need to set UseMinifiedStyleSheets to `false`

```xml
  <PropertyGroup>
    <!-- set these values, if you do not use Excubo.WebCompiler or any other minification and compression pipeline -->
    <LazyStyleSheets_UseMinifiedStyleSheets>false</LazyStyleSheets_UseMinifiedStyleSheets>
    <LazyStyleSheets_UseGzippedStyleSheets>false</LazyStyleSheets_UseGzippedStyleSheets>
    <LazyStyleSheets_UseWebCompiler>false</LazyStyleSheets_UseWebCompiler>
  </PropertyGroup>
```

#### UseGzippedStyleSheets

Same as with `UseMinifiedStyleSheets`, serving compressed version of your style sheets is also supported, but deactivated by default. This is because Kestrel does not handle gzipped style sheets correctly by default.

To enable this, add the following to your `Startup.cs` file:

```cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    /// ...
    /// ...
    
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = context =>
        {
            var headers = context.Context.Response.Headers;
            HandleCompressedResourced(context, headers);
        }
    });
    app.UseStaticFiles();

    /// ...
    /// ...
}

private static void HandleCompressedResourced(StaticFileResponseContext context, IHeaderDictionary headers)
{
    if (context.File == null)
    {
        return;
    }
    if ((string)headers["Content-Type"] != "application/x-gzip")
    {
        return;
    }
    headers.Add("Content-Encoding", "gzip");
    if (context.File.Name.EndsWith("js.gz", System.StringComparison.InvariantCultureIgnoreCase))
    {
        headers["Content-Type"] = "application/javascript";
    }
    if (context.File.Name.EndsWith("css.gz", System.StringComparison.InvariantCultureIgnoreCase))
    {
        headers["Content-Type"] = "text/css";
    }
}
```

Activate use of compressed resources in your `csproj` file:

```xml
  <PropertyGroup>
    <LazyStyleSheets_UseGzippedStyleSheets>true</LazyStyleSheets_UseGzippedStyleSheets>
  </PropertyGroup>
```

#### Auto Inject

A convenient way of writing styled components is to put the scss/css file in the same folder as the component and name the style file according to the component, e.g. `Component.razor` and `Component.razor.css`.
That way the css file gets grouped with the component in Visual Studio (and other IDEs with file nesting capability).

This library goes one step further, by making sure that components developed this way automatically have the style injected. If you want to opt out of this feature, simply set `<LazyStyleSheets_AutoInject>false</LazyStyleSheets_AutoInject>`.

:warning: If you use a custom namespace for your component (i.e. you have an `@namespace SomeNamespace` directive in your component), then AutoInject won't work for you. Use a `<Stylesheet Src="..." />` instead.

## Contribute

If you encounter any issues or have ideas for new features, please raise an issue in this repository.
