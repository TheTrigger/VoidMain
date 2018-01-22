using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace VoidMain.Application.Commands.Execution
{
    public class MethodInvokerProvider : IMethodInvokerProvider
    {
        private readonly TypeInfo NotifyCompletionType = typeof(INotifyCompletion).GetTypeInfo();
        private const string AwaiterMethodName = nameof(Task.GetAwaiter);
        private readonly Type TaskType = typeof(Task);
        private readonly Type GenericTaskType = typeof(Task<>);

        private readonly IMethodInvoker TaskInvoker = new TaskMethodInvoker();
        private readonly IMethodInvoker TaskResultInvoker = new TaskResultMethodInvoker();
        private readonly IMethodInvoker SyncInvoker = new SyncMethodInvoker();

        public IMethodInvoker GetInvoker(MethodInfo method)
        {
            var returnType = method.ReturnType;

            if (returnType == TaskType)
            {
                return TaskInvoker;
            }
            if (returnType.GetTypeInfo().IsGenericType &&
               returnType.GetGenericTypeDefinition() == GenericTaskType)
            {
                return TaskResultInvoker;
            }

            if (IsAwaitable(returnType))
            {
                throw new NotSupportedException(
                    "Async methods returning non-task values are not supported.");
            }

            return SyncInvoker;
        }

        private bool IsAwaitable(Type valueType)
        {
            var getAwaiterMethod = valueType.GetTypeInfo().GetMethod(
                AwaiterMethodName, BindingFlags.Public | BindingFlags.Instance);

            if (getAwaiterMethod == null) return false;

            var awaiterType = getAwaiterMethod.ReturnType;
            return NotifyCompletionType.IsAssignableFrom(awaiterType);
        }
    }
}
