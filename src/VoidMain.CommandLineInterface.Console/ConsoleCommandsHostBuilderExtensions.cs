﻿using Microsoft.Extensions.DependencyInjection;

namespace VoidMain.Hosting
{
    public static class ConsoleCommandsHostBuilderExtensions
    {
        public static ICommandsHostBuilder UseSimpleConsole(this ICommandsHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                services
                    .AddConsoleInterfaceCore()
                    .AddPromptMessage();
            });
        }

        public static ICommandsHostBuilder UseAdvancedConsole(this ICommandsHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                var interfaceBuilder = services
                    .AddConsoleInterfaceCore()
                    .AddPromptMessage()
                    .AddUndoRedo()
                    .AddSyntaxHighlighting();

                interfaceBuilder
                    .AddCommandsHistory()
                    .AddFileStorage();
            });
        }
    }
}
