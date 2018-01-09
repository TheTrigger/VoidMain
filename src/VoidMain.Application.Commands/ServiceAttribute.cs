using System;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class ServiceAttribute : ParameterAttribute
    {
    }
}
