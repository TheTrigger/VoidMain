﻿using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Model
{
    public class ModuleModel
    {
        public Type Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CommandModel> Commands { get; set; }
        public bool ExcludeFromCommandName { get; set; }

        public override string ToString() => $"{Name} [{Type.FullName}]";
    }
}
