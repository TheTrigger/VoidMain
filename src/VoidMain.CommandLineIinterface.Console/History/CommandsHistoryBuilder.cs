namespace Microsoft.Extensions.DependencyInjection
{
    public class CommandsHistoryBuilder : IFeatureBuilder
    {
        public IServiceCollection Services { get; }

        public CommandsHistoryBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
