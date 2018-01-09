using System;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class OperandAttribute : ArgumentAttribute
    {
        public OperandAttribute() { }
        public OperandAttribute(string name) => Name = name;
    }
}
