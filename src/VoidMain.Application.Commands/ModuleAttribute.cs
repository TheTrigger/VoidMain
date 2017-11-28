using System;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ModuleAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ModuleAttribute()
        {
        }

        public ModuleAttribute(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class NonModuleAttribute : Attribute
    {
    }
}
