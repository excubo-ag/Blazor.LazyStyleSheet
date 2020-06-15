## Excubo.Blazor.LazyStyleSheet

![Nuget](https://img.shields.io/nuget/v/Excubo.Blazor.LazyStyleSheet)
![Nuget](https://img.shields.io/nuget/dt/Excubo.Blazor.LazyStyleSheet)
![GitHub](https://img.shields.io/github/license/excubo-ag/Blazor.LazyStyleSheet)

A major issue on websites is slow page load. In part, this is due to enormous payloads that need to be downloaded in full before a page can be rendered correctly. Minimization and compression help to some degree, but it ignores the awkward fact that many style sheets are only used to a tiny fraction.

With HTTP/2, loading small files rather than one large one is less of a performance concern than with HTTP/1.1. Since Blazor uses HTTP/2 by default, we can make use of this and split style sheets into smaller chunks. Those chunks can then be loaded lazily, i.e. only when a component actually needs it.

Excubo.Blazor.LazyStyleSheet enables you to write dedicated style sheet for each component.

## Breaking changes

### Version 2.X.Y

`Excubo.Blazor.LazyStyleSheet` now contains build tasks to automatically inject the `IStyleSheetService` when you write your component style as a `Component.razor.css` or `Component.razor.scss` file. That means, if you previously manually inserted `IStyleSheetService` into your component, you now have to remove that.

## How to use

### 1. Install the nuget package Excubo.Blazor.LazyStyleSheet

Excubo.Blazor.LazyStyleSheet is distributed [via nuget.org](https://www.nuget.org/packages/Excubo.Blazor.LazyStyleSheet/).
![Nuget](https://img.shields.io/nuget/v/Excubo.Blazor.LazyStyleSheet)

#### Package Manager:
```ps
Install-Package Excubo.Blazor.LazyStyleSheet -Version 2.0.9
```

#### .NET Cli:
```cmd
dotnet add package Excubo.Blazor.LazyStyleSheet --version 2.0.9
```

#### Package Reference
```xml
<PackageReference Include="Excubo.Blazor.LazyStyleSheet" Version="2.0.9" />
```

### 2. Add the service in your `Startup.cs` file

```cs
   //...
   services.AddStyleSheetLazyLoading(); // Tip: Use Excubo.Analyzers.DependencyInjectionValidation for warnings when you forget such a dependency
   //...
```

### 3. Add the `StyleSheets` component to your `App` component

<pre>
&lt;Router AppAssembly="@typeof(Program).Assembly"&gt;
    &lt;Found Context="routeData"&gt;
        &lt;AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" /&gt;
    &lt;/Found&gt;
    &lt;NotFound&gt;
        &lt;LayoutView Layout="@typeof(MainLayout)"&gt;
            &lt;p&gt;Sorry, there's nothing at this address.&lt;/p&gt;
        &lt;/LayoutView&gt;
    &lt;/NotFound&gt;
&lt;/Router&gt;
<b>&lt;Excubo.Blazor.LazyStyleSheet.StyleSheets /&gt;</b>
</pre>

### 4. Write your style sheets and put them next to your component

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

## Remarks

 - The `<StyleSheets />` component should only be used exactly once. However, except for a slight performance penalty, there is likely no issue by doing so. Of course there's no benefit to multiple `<StyleSheets />` components either.
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
            CacheCommonResources(env, headers);
        }
    });
    app.UseStaticFiles();

    /// ...
    /// ...
}

private static void CacheCommonResources(IWebHostEnvironment env, IHeaderDictionary headers)
{
    if ((string)headers["Content-Type"] == "application/javascript" ||
        (string)headers["Content-Type"] == "text/css")
    {
        var cache_period = env.IsDevelopment() ? 10 * 60 : 2 * 7 * 24 * 60 * 60; // development: 10m, production: 2w
        headers.Add("Cache-Control", $"public, max-age={cache_period}");
    }
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

## Known issues

### Custom namespace

The automatic injection of the `IStyleSheetService` does not work if the namespace for your component is overridden. You then need to manually add this feature. The recommended code snippet is

```cs
namespace Custom
{
    public partial class MyComponent
    {
        private IStyleSheetService style_sheet_service;
        [Inject]
        private IStyleSheetService
        {
            get => style_sheet_service;
            set
            {
                style_sheet_service = value;
                style_sheet_service?.Add("path/to/your/stylesheet.min.css");
            }
        }
    }
 }
```

As this hardcodes the path to your stylesheet, it is strongly not recommended to use overriden namespaces, as it is not stable under refactorings.

## Contribute

If you encounter any issues or have ideas for new features, please raise an issue in this repository.
