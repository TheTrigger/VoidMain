using Microsoft.Extensions.DependencyInjection;
using VoidMain.Application.Builder;
using VoidMain.Application.Commands.Help;

namespace VoidMain.Application.Commands.Builder
{
    public static class HelpCommandsRewriterApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHelpCommandsRewriter(this IApplicationBuilder app)
        {
            var rewriter = ActivatorUtilities.CreateInstance<HelpCommandsRewriter>(app.Services);
            app.Use(next =>
            {
                return async context =>
                {
                    rewriter.TryRewrite(context);
                    await next(context);
                };
            });
            return app;
        }
    }
}
