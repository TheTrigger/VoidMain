using System;

namespace VoidMain.Application.Commands.Builder
{
    public interface ICommandsApplicationBuilder
    {
        IServiceProvider Services { get; }
        void AddModule<TModule>(Action<IModuleConfiguration<TModule>> configure = null);
        ICommandsApplication Build();
    }
}
