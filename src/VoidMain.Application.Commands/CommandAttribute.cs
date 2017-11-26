using System;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString() => Name;
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NonCommandAttribute : Attribute
    {
    }
}
