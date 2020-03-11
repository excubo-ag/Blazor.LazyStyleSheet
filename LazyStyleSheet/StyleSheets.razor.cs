namespace Excubo.Blazor.LazyStyleSheet
{
    /// <summary>
    /// Component that writes link-tags for style sheets on demand.
    /// </summary>
    /// <remarks>
    /// <para>To signal that this component should add another style sheet, inject an <c>IStyleSheetService</c> into your component, then call <c>style_sheet_service.Add(url);</c></para>
    /// <para>It is recommended to place this component in the App.razor component.</para>
    /// </remarks>
    /// <example>
    ///   <code>
    ///   &lt;Router AppAssembly = "@typeof(Program).Assembly"&gt;
    ///       &lt;Found Context="routeData"&gt;
    ///           &lt;AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" /&gt;
    ///       &lt;/Found&gt;
    ///       &lt;NotFound&gt;
    ///           &lt;LayoutView Layout="@typeof(MainLayout)" &gt;
    ///               &lt;p&gt; Sorry, there's nothing at this address.&lt;/p&gt;
    ///           &lt;/LayoutView&gt;
    ///       &lt;/NotFound&gt;
    ///   &lt;/Router&gt;
    ///   &lt;StyleSheets/&gt;
    ///   </code>
    /// </example>
    public partial class StyleSheets
    {
    }
}
