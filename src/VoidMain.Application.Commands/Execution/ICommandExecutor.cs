using System;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Execution
{
    public interface ICommandExecutor
    {
        Task<object> ExecuteAsync(CommandModel command, object[] arguments,
            IServiceProvider services, CancellationToken token = default(CancellationToken));
    }
}
