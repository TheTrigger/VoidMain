using VoidMain.Application.Commands.Help;

namespace VoidMain.Application.Commands.Builder
{
    public static class HelpCommandsApplicationBuilderExtensions
    {
        public static ICommandsApplicationBuilder AddHelpCommands(this ICommandsApplicationBuilder builder)
        {
            builder.AddModule<HelpModule>();
            return builder;
        }
    }
}
