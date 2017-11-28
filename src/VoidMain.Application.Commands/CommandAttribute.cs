using System;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public CommandAttribute()
        {
        }

        public CommandAttribute(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class NonCommandAttribute : Attribute
    {
    }
}
