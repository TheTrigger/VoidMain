using System;
using System.Collections.Generic;

namespace VoidMain.Application.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static void Run(this IApplicationBuilder app, CommandDelegate handler)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            app.Use(_ => handler);
        }

        public static IApplicationBuilder UseWhen(this IApplicationBuilder app,
            Predicate<Dictionary<string, object>> predicate, Action<IApplicationBuilder> configureApplication)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }
            if (configureApplication == null)
            {
                throw new ArgumentNullException(nameof(configureApplication));
            }

            // This is performed during Configure stage.
            var branchBuilder = app.New();
            configureApplication(branchBuilder);

            app.Use(mainBranch =>
            {
                // This is performed during Build stage.
                var conditionalBranch = branchBuilder.Build();

                return context =>
                {
                    return predicate(context)
                        ? conditionalBranch(context)
                        : mainBranch(context);
                };
            });

            return app;
        }
    }
}
