## Why Excubo.Blazor.LazyStyleSheet?

A major issue on websites is slow page load. In part, this is due to enormous payloads that need to be downloaded in full before a page can be rendered correctly. Minimization and compression help to some degree, but it ignores the awkward fact that many style sheets are only used to a tiny fraction.

With HTTP/2, loading small files rather than one large one is less of a performance concern than with HTTP/1.1. Since Blazor uses HTTP/2 by default, we can make use of this and split style sheets into smaller chunks. Those chunks can then be loaded lazily, i.e. only when a component actually needs it.

Excubo.Blazor.LazyStyleSheet enables you to write dedicated style sheet for each component.

## How to use

### 1. Install the nuget package Excubo.Blazor.LazyStyleSheet

#### Package Manager:
```ps
Install-Package Excubo.Blazor.LazyStyleSheet
```

#### .NET Cli:
```cmd
dotnet add package Excubo.Blazor.LazyStyleSheet
```

#### Package Reference
```xml
<PackageReference Include="Excubo.Blazor.LazyStyleSheet" />
```

### 2. Add the service in your `Startup.cs` file

```cs
   //...
   services.AddStyleSheetLazyLoading();
   //...
```

### 3. Add the `StyleSheets` component to your `App` component

```razor
<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
        <AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
    </Found>
    <NotFound>
        <LayoutView Layout="@typeof(MainLayout)">
            <p>Sorry, there's nothing at this address.</p>
        </LayoutView>
    </NotFound>
</Router>
<Excubo.Blazor.LazyStyleSheet.StyleSheets />
```

### 4. Add style sheets where necessary

```razor
@page "/hello"
@inject Excubo.Blazor.LazyStyleSheet.IStyleSheetService StyleSheetService

<div class="my-style">My styled component</div>

@code {
    protected override void OnInitialized()
    {
        StyleSheetService.Add("css/my_style.css"); // the StyleSheetService prevents styles from being added more than once
        base.OnInitialized();
    }
}

```

If you're writing a component library, don't forget to prefix the URL with `_content/My.Package.Name/`. You also need to instruct your users to follow step 2 and 3, or wrap this on behalf of the user.

## Remarks

 - The `<StyleSheets />` component should only be used exactly once. However, except for a slight performance penalty, there is likely no issue by doing so. Of course there's no benefit to multiple `<StyleSheets />` components either.
 - Style sheet urls may be added any number of times, and will only be added to the DOM once (as duplicate `<link>` tags don't achieve anything). This only applies if the url string matches exactly, i.e. there is a difference between `https://localhost/css/style.css` and `css/style.css`.


## Tips & tricks

For development it might be more convenient to have the `css` close to the `razor` component in the file hierarchy. However, for deployment, the `css` must be part of the static file assets, usually in `wwwroot`. You can create `Component.razor.css` files and automatically move them to the `wwwroot` folder with the following addition to your `csproj` file:

```xml
  <ItemGroup>
    <ComponentStyles Include="**\*.razor.css" Exclude="wwwroot\**" />
  </ItemGroup>
  
  <Target Name="CopyStyles" BeforeTargets="AfterBuild">
    <RemoveDir Directories="wwwroot\css\components" />
    <Copy SourceFiles="@(ComponentStyles)" DestinationFiles="@(ComponentStyles->'wwwroot\css\components\%(RecursiveDir)%(Filename)%(Extension)')" />
  </Target>
```

Remember to adjust the referenced path: `StyleSheetService.Add("/css/components/path/to/Component.razor.css");`

## Contribute

If you encounter any issues or have ideas for new features, please raise an issue in this repository.
