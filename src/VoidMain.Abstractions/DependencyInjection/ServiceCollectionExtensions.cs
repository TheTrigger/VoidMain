using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Configure<TOptions>(
            this IServiceCollection services, TOptions options)
            where TOptions : class
        {
            services.AddTransient(s => options);
            return services;
        }

        public static IServiceCollection Configure<TOptions>(
            this IServiceCollection services, Action<TOptions> configure)
            where TOptions : class
        {
            services.AddTransient(s =>
            {
                var options = ActivatorUtilities.CreateInstance<TOptions>(s);
                configure(options);
                return options;
            });
            return services;
        }
    }
}
