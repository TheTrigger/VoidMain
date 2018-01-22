using System.Reflection;
using System.Threading.Tasks;

namespace VoidMain.Application.Commands.Execution
{
    public class TaskResultMethodInvoker : IMethodInvoker
    {
        public async Task<object> Invoke(object instance, MethodInfo method, object[] arguments)
        {
            var task = (Task)method.Invoke(instance, arguments);
            await task;

            var resultProperty = task.GetType().GetTypeInfo()
                .GetProperty(nameof(Task<object>.Result));

            var result = resultProperty.GetValue(task);
            return result;
        }
    }
}
