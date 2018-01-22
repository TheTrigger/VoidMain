using System.Reflection;
using System.Threading.Tasks;

namespace VoidMain.Application.Commands.Execution
{
    public interface IMethodInvoker
    {
        Task<object> Invoke(object instance, MethodInfo method, object[] arguments);
    }
}
