using System.Reflection;
using System.Threading.Tasks;

namespace VoidMain.Application.Commands.Execution
{
    public class SyncMethodInvoker : IMethodInvoker
    {
        public Task<object> Invoke(object instance, MethodInfo method, object[] arguments)
        {
            var result = method.Invoke(instance, arguments);
            return Task.FromResult(result);
        }
    }
}
