namespace Microsoft.Extensions.DependencyInjection
{
    public interface IFeatureBuilder
    {
        IServiceCollection Services { get; }
    }
}
