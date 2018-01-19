using System;
using System.Collections.Generic;
using System.Reflection;

namespace VoidMain.Application.Commands.Model
{
    public class CommandModel
    {
        public MethodInfo Method { get; set; }
        public IReadOnlyList<ArgumentModel> Arguments { get; set; }
        public CommandName Name { get; set; }
        public string Description { get; set; }
        public ModuleModel Module { get; set; }

        public override string ToString()
        {
            string arguments = Arguments == null
                ? String.Empty
                : String.Join(", ", Arguments);
            return $"{Name} ({arguments})";
        }
    }
}
