﻿using System;
using System.Linq;
using VoidMain.Application;
using VoidMain.CommandLineIinterface.Highlighting;
using VoidMain.CommandLineIinterface.Highlighting.CommandLine;
using VoidMain.CommandLineIinterface.IO.Views;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SyntaxHighlightingConsoleInterfaceBuilderExtensions
    {
        public static ConsoleInterfaceBuilder AddSyntaxHighlighting(
            this ConsoleInterfaceBuilder builder,
            Action<CommandLineHighlightingOptions> options = null)
        {
            var services = builder.Services;
            services.AddSingleton<ITextHighlighter<TextStyle>, CommandLineSyntaxHighlighter>();
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