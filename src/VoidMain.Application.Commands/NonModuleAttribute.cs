using System;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class NonModuleAttribute : Attribute
    {
    }
}
