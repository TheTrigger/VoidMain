namespace Microsoft.Extensions.DependencyInjection
{
    public class ConsoleInterfaceBuilder : IFeatureBuilder
    {
        public IServiceCollection Services { get; }

        public ConsoleInterfaceBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
