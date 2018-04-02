using VoidMain.CommandLineIinterface.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CmdPromptConsoleInterfaceBuilderExtensions
    {
        public static ConsoleInterfaceBuilder AddPromptMessage(
            this ConsoleInterfaceBuilder builder, string message = null)
        {
            builder.Services.AddTransient<ICommandLinePrompt>(s => new PromptMessage(message ?? "CMD> "));
            return builder;
        }
    }
}
