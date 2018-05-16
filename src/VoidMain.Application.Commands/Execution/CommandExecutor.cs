using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Execution
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IModuleFactory _moduleFactory;
        private readonly IMethodInvokerProvider _invokerProvider;

        public CommandExecutor(IModuleFactory moduleFactory, IMethodInvokerProvider invokerProvider)
        {
            _moduleFactory = moduleFactory ?? throw new ArgumentNullException(nameof(moduleFactory));
            _invokerProvider = invokerProvider ?? throw new ArgumentNullException(nameof(invokerProvider));
        }

        private void Validate(CommandModel command, object[] arguments, IServiceProvider services)
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
        }

        public async Task<object> ExecuteAsync(CommandModel command, object[] arguments,
            IServiceProvider services, CancellationToken token = default(CancellationToken))
        {
            Validate(command, arguments, services);

            token.ThrowIfCancellationRequested();
            var moduleType = command.Module.Type;
            var method = command.Method;
            var moduleInstance = _moduleFactory.Create(moduleType, services);

            try
            {
                var invoker = _invokerProvider.GetInvoker(method);
                token.ThrowIfCancellationRequested();
                var result = await invoker.Invoke(moduleInstance, method, arguments)
                    .ConfigureAwait(false);
                return result;
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw; // To suppress error CS0161: not all code paths return a value.
            }
            finally
            {
                (moduleInstance as IDisposable)?.Dispose();
            }
        }
    }
}
