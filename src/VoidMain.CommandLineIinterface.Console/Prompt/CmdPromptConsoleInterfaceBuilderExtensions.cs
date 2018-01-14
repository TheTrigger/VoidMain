using VoidMain.CommandLineIinterface.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CmdPromptConsoleInterfaceBuilderExtensions
    {
        public static ConsoleInterfaceBuilder AddPromptMessage(
            this ConsoleInterfaceBuilder builder, string message = null)
        {
            var services = builder.Services;
            if (message == null)
            {
                message = "CMD> ";
            }
            services.AddTransient<ICommandLinePrompt>(s => new PromptMessage(message));
            return builder;
        }
    }
}
