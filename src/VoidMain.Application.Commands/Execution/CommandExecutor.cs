﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;
using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands.Execution
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly ITypeActivator _typeActivator;
        private readonly IMethodInvokerProvider _invokerProvider;

        public CommandExecutor(ITypeActivator typeActivator, IMethodInvokerProvider invokerProvider)
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

            token.ThrowIfCancellationRequested();
            var moduleType = command.Module.Type;
            var method = command.Method;
            var moduleInstance = _typeActivator.CreateInstance(moduleType, services);

            try
            {
                if (moduleInstance is ICommandsModule standard)
                {
                    standard.Output = services.GetRequiredService<ICommandLineOutput>();
                }

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
