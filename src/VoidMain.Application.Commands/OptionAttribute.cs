using System;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public sealed class OptionAttribute : ArgumentAttribute
    {
        public OptionAttribute() { }
        public OptionAttribute(string name) => Name = name;
    }
}
