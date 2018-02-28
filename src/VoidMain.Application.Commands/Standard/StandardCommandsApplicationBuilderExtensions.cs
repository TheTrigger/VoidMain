using VoidMain.Application.Commands.Standard;

namespace VoidMain.Application.Commands.Builder
{
    public static class StandardCommandsApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds commands: 'quit', 'clear'
        /// </summary>
        public static ICommandsApplicationBuilder AddStandardCommands(this ICommandsApplicationBuilder builder)
        {
            builder.AddModule<AppModule>();
            builder.AddModule<OutputModule>();
            return builder;
        }

        /// <summary>
        /// Adds commands: 'quit'
        /// </summary>
        public static ICommandsApplicationBuilder AddAppCommands(this ICommandsApplicationBuilder builder)
        {
            builder.AddModule<AppModule>();
            return builder;
        }

        /// <summary>
        /// Adds commands: 'clear'
        /// </summary>
        public static ICommandsApplicationBuilder AddOutputCommands(this ICommandsApplicationBuilder builder)
        {
            builder.AddModule<OutputModule>();
            return builder;
        }
    }
}
