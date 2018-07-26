using System;
using VoidMain.CommandLineInterface.IO.InputHandlers;
using VoidMain.CommandLineInterface.UndoRedo;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UndoRedoConsoleInterfaceBuilderExtensions
    {
        public static ConsoleInterfaceBuilder AddUndoRedo(
               this ConsoleInterfaceBuilder builer, Action<UndoRedoOptions> options = null)
        {
            var services = builer.Services;
            services.AddTransient<IInputHandler, UndoRedoInputHandler>();
            services.AddSingleton<IUndoRedoManager, UndoRedoManager>();

            if (options != null)
            {
                services.Configure(options);
            }

            return builer;
        }
    }
}
