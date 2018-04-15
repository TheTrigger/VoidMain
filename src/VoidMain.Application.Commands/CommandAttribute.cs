using System;

namespace VoidMain.Application.Commands
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class CommandAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }

        private bool? _excludeModuleName;
        public bool ExcludeModuleName
        {
            get => _excludeModuleName ?? false;
            set => _excludeModuleName = value;
        }
        public bool IsSetExcludeModuleName => _excludeModuleName.HasValue;

        public CommandAttribute()
        {
        }

        public CommandAttribute(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }
}
