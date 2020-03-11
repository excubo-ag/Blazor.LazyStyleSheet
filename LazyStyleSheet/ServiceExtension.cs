using Microsoft.Extensions.DependencyInjection;

namespace Excubo.Blazor.LazyStyleSheet
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddStyleSheetLazyLoading(this IServiceCollection services)
        {
            return services
                .AddScoped<IStyleSheetService, StyleSheetService>();
        }
    }
}
