using Excubo.Analyzers.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Excubo.Blazor.LazyStyleSheet
{
    public static class ServiceExtension
    {
        [Exposes(typeof(StyleSheetService)), As(typeof(IStyleSheetService))]
        [Exposes(typeof(StyleSheets))]
        [Obsolete("This can be removed! Adding style sheets just became a whole lot easier. See https://github.com/excubo-ag/Blazor.LazyStyleSheet for more details.")]
        public static IServiceCollection AddStyleSheetLazyLoading(this IServiceCollection services)
        {
            return services
                .AddScoped<IStyleSheetService, StyleSheetService>();
        }
    }
}
