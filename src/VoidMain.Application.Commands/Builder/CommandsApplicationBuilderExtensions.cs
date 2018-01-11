using Microsoft.Extensions.DependencyInjection;
using System;
using VoidMain.Application.Builder;

namespace VoidMain.Application.Commands.Builder
{
    public static class CommandsApplicationBuilderExtensions
    {
        public static void RunCommands(
            this IApplicationBuilder app, Action<ICommandsApplicationBuilder> configure)
        {
            var builder = app.Services.GetRequiredService<ICommandsApplicationBuilder>();
            configure(builder);
            var commandsApplication = builder.Build();
            app.Run(commandsApplication.ExecuteCommand);
        }
    }
}
