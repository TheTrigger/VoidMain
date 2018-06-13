using Microsoft.Extensions.DependencyInjection;
using System;
using VoidMain.Application.Commands.Internal;
using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands.Execution
{
    public class ModuleInstanceFactory : IModuleInstanceFactory
    {
        private readonly ITypeActivator _typeActivator;

        public ModuleInstanceFactory(ITypeActivator typeActivator)
        {
            _typeActivator = typeActivator ?? throw new ArgumentNullException(nameof(typeActivator));
        }

        public object Create(Type moduleType, IServiceProvider services)
        {
            var instance = _typeActivator.CreateInstance(moduleType, services);
            if (instance is ICommandsModule standard)
            {
                try
                {
                    standard.Output = services.GetRequiredService<ICommandLineOutput>();
                }
                catch
                {
                    (instance as IDisposable)?.Dispose();
                    throw;
                }
            }
            return instance;
        }
    }
}
