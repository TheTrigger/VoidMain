using System;
using VoidMain.CommandLineInterface.IO.Prompt;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CmdPromptConsoleInterfaceBuilderExtensions
    {
        public static ConsoleInterfaceBuilder AddPromptMessage(
            this ConsoleInterfaceBuilder builder, string message)
        {
            return builder.AddPromptMessage(options => options.Message = message);
        }

        public static ConsoleInterfaceBuilder AddPromptMessage(
            this ConsoleInterfaceBuilder builder, Action<PromptMessageOptions> options = null)
        {
            builder.Services.AddTransient<IPromptMessage, PromptMessage>();
            if (options != null)
            {
                builder.Services.Configure(options);
            }
            return builder;
        }
    }
}
