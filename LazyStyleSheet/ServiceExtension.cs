using Excubo.Analyzers.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Excubo.Blazor.LazyStyleSheet
{
    public static class ServiceExtension
    {
        [Exposes(typeof(StyleSheetService)), As(typeof(IStyleSheetService))]
        [Exposes(typeof(StyleSheets))]
        public static IServiceCollection AddStyleSheetLazyLoading(this IServiceCollection services)
        {
            return services
                .AddScoped<IStyleSheetService, StyleSheetService>();
        }
    }
}
