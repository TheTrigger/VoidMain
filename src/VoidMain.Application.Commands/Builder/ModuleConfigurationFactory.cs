namespace VoidMain.Application.Commands.Builder
{
    public class ModuleConfigurationFactory : IModuleConfigurationFactory
    {
        public IModuleConfiguration<TModule> Create<TModule>()
        {
            return new ModuleConfiguration<TModule>();
        }
    }
}
