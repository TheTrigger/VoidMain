using System;
using System.Linq;
using VoidMain.Application;
using VoidMain.CommandLineIinterface.IO.Views;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.Parser.Syntax;
using VoidMain.CommandLineIinterface.SyntaxHighlight;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SyntaxHighlightingConsoleInterfaceBuilderExtensions
    {
        public static ConsoleInterfaceBuilder AddSyntaxHighlighting(
            this ConsoleInterfaceBuilder builder,
            Action<ConsoleSyntaxHighlightingOptions> options = null)
        {
            var services = builder.Services;
            services.AddSingleton<ISyntaxHighlighter<ConsoleTextStyle>, SyntaxHighlighter<ConsoleTextStyle>>();
            services.AddSingleton<ILineViewProvider, ConsoleHighlightedLineViewProvider>();

            var parserService = services.FirstOrDefault(_ => _.ServiceType == typeof(ICommandLineParser));
            if (parserService == null)
            {
                services.AddSingleton<ICommandLineParser, CommandLineParser>();
                services.AddSingleton<ICommandLineLexer, CommandLineLexer>();
                services.AddSingleton<ISemanticModel, EmptySemanticModel>();
                services.AddSingleton<SyntaxFactory, SyntaxFactory>();
            }

            if (options != null)
            {
                services.Configure(options);
            }

            return builder;
        }
    }
}
