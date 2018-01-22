using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Commands.Model;
using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands.Execution
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ITypeActivator _typeActivator;
        private readonly IMethodInvokerProvider _invokerProvider;

        public CommandExecutor(IServiceProvider services,
            ITypeActivator typeActivator, IMethodInvokerProvider invokerProvider)
        {
            _typeActivator = typeActivator ?? throw new ArgumentNullException(nameof(typeActivator));
            _invokerProvider = invokerProvider ?? throw new ArgumentNullException(nameof(invokerProvider));
        }

        public async Task<object> ExecuteAsync(CommandModel command, object[] arguments,
            IServiceProvider services, CancellationToken token = default(CancellationToken))
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var moduleType = command.Module.Type;
            var method = command.Method;
            var moduleInstance = _typeActivator.CreateInstance(services, moduleType);

            try
            {
                if (moduleInstance is ICommandsModule standard)
                {
                    standard.Output = services.GetRequiredService<ICommandLineOutput>();
                }

                var invoker = _invokerProvider.GetInvoker(method);
                var result = await invoker.Invoke(moduleInstance, method, arguments)
                    .ConfigureAwait(false);
                return result;
            }
            finally
            {
                (moduleInstance as IDisposable)?.Dispose();
            }
        }
    }
}
