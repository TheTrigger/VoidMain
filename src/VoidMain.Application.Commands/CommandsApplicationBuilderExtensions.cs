using Microsoft.Extensions.DependencyInjection;
using VoidMain.Application.Commands;

namespace VoidMain.Application.Builder
{
    public static class CommandsApplicationBuilderExtensions
    {
        public static void RunCommands(this IApplicationBuilder app)
        {
            var cmdApp = app.Services.GetRequiredService<ICommandsApplication>();
            app.Run(cmdApp.ExecuteCommand);
        }
    }
}
