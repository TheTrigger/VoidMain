namespace VoidMain.Application.Commands.Builder
{
    public interface IModuleConfigurationFactory
    {
        IModuleConfiguration<TModule> Create<TModule>();
    }
}
