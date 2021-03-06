﻿using VoidMain.Application;
using VoidMain.Application.Commands.Arguments;
using VoidMain.Application.Commands.Arguments.CollectionConstructors;
using VoidMain.Application.Commands.Arguments.ValueParsers;
using VoidMain.Application.Commands.Builder;
using VoidMain.Application.Commands.Builder.Validation;
using VoidMain.Application.Commands.Execution;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;
using VoidMain.Application.Commands.Resolving;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CommandsServiceCollectionExtensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddTransient<ICommandsApplicationBuilder, CommandsApplicationBuilder>();
            services.AddTransient<IModuleModelConstructor, ModuleModelConstructor>();
            services.AddTransient<ICommandModelConstructor, CommandModelConstructor>();
            services.AddTransient<IArgumentModelConstructor, ArgumentModelConstructor>();
            services.AddTransient<ICommandNameParser, CommandNameParser>();
            services.AddTransient<IModuleConfigurationFactory, ModuleConfigurationFactory>();
            services.AddTransient<ICommandModelValidator, CommandModelValidator>();
            services.AddSingleton<ApplicationModel>();
            services.AddTransient<ICommandResolver, CommandResolver>();
            services.AddTransient<IArgumentsParser, ArgumentsParser>();
            services.AddTransient<ICommandExecutor, CommandExecutor>();
            services.AddTransient<ITypeActivator, CachedTypeActivator>();
            services.AddTransient<IModuleInstanceFactory, ModuleInstanceFactory>();
            services.AddTransient<IMethodInvokerProvider, MethodInvokerProvider>();
            services.AddSingleton<ICollectionConstructorProvider, CollectionConstructorProvider>();
            services.AddSingleton<IValueParserProvider, ValueParserProvider>();
            services.AddSingleton<IMultiValueParser, MultiValueParser>();
            services.AddTransient<ISemanticModel, CommandsSemanticModel>();
            return services;
        }
    }
}
