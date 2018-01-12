using System;

namespace VoidMain.Application.Commands.Builder
{
    public interface ICommandsApplicationBuilder
    {
        void AddModule<TModule>(Action<IModuleConfiguration<TModule>> configure = null);
        ICommandsApplication Build();
    }
}
