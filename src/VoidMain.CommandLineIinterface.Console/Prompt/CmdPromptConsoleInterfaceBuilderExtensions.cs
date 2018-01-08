using VoidMain.CommandLineIinterface.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CmdPromptConsoleInterfaceBuilderExtensions
    {
        public static ConsoleInterfaceBuilder AddCmdPrompt(
            this ConsoleInterfaceBuilder builder)
        {
            var services = builder.Services;
            services.AddTransient<ICommandLinePrompt, CmdPrompt>();
            return builder;
        }
    }
}
