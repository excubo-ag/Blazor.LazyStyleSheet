# Standalone Blazor App

This project shows how to use Excubo.Blazor.LazyStyleSheet. It demonstrates both automatic and manual injection.

See BlazorApp/Pages/Index.razor for manual injection. Make your life easier by using auto-injectic, see BlazorApp/Pages/Counter.razor and BlazorApp/Pages/Counter.razor.scss.

## Relevant files

All the following files have been modified slightly from the plain Blazor sample project. All changes are detailed below.


### BlazorApp/Startup.cs

```cs
using Excubo.Blazor.LazyStyleSheet;
// other imports omitted here

namespace BlazorApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddStyleSheetLazyLoading();
            services.AddSingleton<WeatherForecastService>();
        }

        // Omitted here, because no change: public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    }
}
```

- added service `services.AddStyleSheetLazyLoading();` (uses namespace `Excubo.Blazor.LazyStyleSheet`)

### BlazorApp/App.razor

```html

<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
    </Found>
    <NotFound>
        <LayoutView Layout="@typeof(MainLayout)">
            <p>Sorry, there's nothing at this address.</p>
        </LayoutView>
    </NotFound>
</Router>

<Excubo.Blazor.LazyStyleSheet.StyleSheets />
```

-  added the `<Excubo.Blazor.LazyStyleSheet.StyleSheets />` component.

### BlazorApp/Pages/Index.razor

```html
@page "/"
@inject Excubo.Blazor.LazyStyleSheet.IStyleSheetService StyleSheetService
@code {
    protected override void OnInitialized()
    {
        StyleSheetService.Add("css/manual_injection.min.css");
        base.OnInitialized();
    }
}

<h1 class="italicised">Hello, world!</h1>

Welcome to your new app.

<SurveyPrompt Title="How is Blazor working for you?" />
```

- Apply a style to the heading. This style is defined in BlazorApp/wwwroot/css/manual_injection.scss
- Inject the IStyleSheetService
- Add the required css in the `OnInitialized` method. Note that we add the minified version of the compiled css, not the scss itself.


### BlazorApp/Pages/Counter.razor

```html
@page "/counter"

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class="btn btn-primary colored" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }
}
```

- Added the custom style `colored` to the button, which is defined in BlazorApp/Pages/Counter.razor.scss

### BlazorApp/Pages/Counter.razor.scss

```css
@import '../../../default_values.scss';
.colored {
    color: $variable
}
```

- Defines the style colored, which takes its value from a file (see /BlazorApp/wwwroot/default_values.scss). 
- As this file is put with the razor component, it will be *auto-injected* into the component.

### BlazorApp/wwwroot/default_values.scss

```css
$variable:blue;
```

- define our custom color `variable` to be `blue`.

### BlazorApp/BlazorApp.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
  </PropertyGroup>

  <Target Name="CompileWebAssets" AfterTargets="CoreCompile">
    <Exec Command="webcompiler -r wwwroot/css" />
  </Target>

</Project>
```

 - We want to have our custom scss files to be compiled:
 ```xml
  <Target Name="CompileWebAssets" AfterTargets="CoreCompile">
    <Exec Command="webcompiler -r wwwroot/css" />
  </Target>
 ```


