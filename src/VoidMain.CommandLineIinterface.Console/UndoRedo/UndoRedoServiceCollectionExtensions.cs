using System;
using VoidMain.CommandLineIinterface.IO.Console.InputHandlers;
using VoidMain.CommandLineIinterface.UndoRedo;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UndoRedoServiceCollectionExtensions
    {
        public static IServiceCollection AddUndoRedo(
               this IServiceCollection services, Action<UndoRedoOptions> options = null)
        {
            services.AddTransient<IConsoleInputHandler, UndoRedoInputHandler>();
            services.AddSingleton<IUndoRedoManager, UndoRedoManager>();

            if (options != null)
            {
                services.Configure(options);
            }
            return services;
        }
    }
}
