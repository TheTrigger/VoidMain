using System;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class NonCommandAttribute : Attribute
    {
    }
}
