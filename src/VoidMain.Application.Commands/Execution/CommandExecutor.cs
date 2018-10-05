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
        private readonly IModuleInstanceFactory _moduleInstanceFactory;
        private readonly IMethodInvokerProvider _methodInvokerProvider;

        public CommandExecutor(IModuleInstanceFactory moduleInstanceFactory, IMethodInvokerProvider methodInvokerProvider)
        {
            _moduleInstanceFactory = moduleInstanceFactory ?? throw new ArgumentNullException(nameof(moduleInstanceFactory));
            _methodInvokerProvider = methodInvokerProvider ?? throw new ArgumentNullException(nameof(methodInvokerProvider));
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
            IServiceProvider services, CancellationToken token = default)
        {
            Validate(command, arguments, services);

            token.ThrowIfCancellationRequested();
            var moduleType = command.Module.Type;
            var method = command.Method;
            var moduleInstance = _moduleInstanceFactory.Create(moduleType, services);

            try
            {
                var invoker = _methodInvokerProvider.GetInvoker(method);
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
