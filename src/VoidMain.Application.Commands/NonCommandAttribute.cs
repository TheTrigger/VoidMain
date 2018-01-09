using System;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NonCommandAttribute : Attribute
    {
    }
}
