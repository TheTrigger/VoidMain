using System;
using VoidMain.CommandLineIinterface.IO.InputHandlers;
using VoidMain.CommandLineIinterface.UndoRedo;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UndoRedoConsoleInterfaceBuilderExtensions
    {
        public static ConsoleInterfaceBuilder AddUndoRedo(
               this ConsoleInterfaceBuilder builer, Action<UndoRedoOptions> options = null)
        {
            var services = builer.Services;
            services.AddTransient<IConsoleInputHandler, UndoRedoInputHandler>();
            services.AddSingleton<IUndoRedoManager, UndoRedoManager>();

            if (options != null)
            {
                services.Configure(options);
            }
            return builer;
        }
    }
}
