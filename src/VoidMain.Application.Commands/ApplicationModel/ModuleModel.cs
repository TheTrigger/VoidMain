using System;

namespace VoidMain.Application.Commands.ApplicationModel
{
    public class ModuleModel
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString() => $"{Name} [{Type.FullName}]";
    }
}
