using System;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class NonModuleAttribute : Attribute
    {
    }
}
