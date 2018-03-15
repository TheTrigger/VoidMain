using System;
using System.Threading;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArgumentsServiceProvider : IServiceProvider
    {
        private static readonly Type CancelTokenType = typeof(CancellationToken);
        private readonly IServiceProvider _services;
        private readonly CancellationToken _token;

        public ArgumentsServiceProvider(IServiceProvider services, CancellationToken token)
        {
            _services = services;
            _token = token;

        }

        public object GetService(Type serviceType)
        {
            if (serviceType == CancelTokenType)
            {
                return _token;
            }

            return _services.GetService(serviceType);
        }
    }
}
