using System.Reflection;

namespace VoidMain.Application.Commands.Execution
{
    public interface IMethodInvokerProvider
    {
        IMethodInvoker GetInvoker(MethodInfo method);
    }
}
