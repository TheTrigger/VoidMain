using System.Reflection;
using System.Threading.Tasks;

namespace VoidMain.Application.Commands.Execution
{
    public class TaskMethodInvoker : IMethodInvoker
    {
        public async Task<object> Invoke(object instance, MethodInfo method, object[] arguments)
        {
            await (Task)method.Invoke(instance, arguments);
            return null;
        }
    }
}
