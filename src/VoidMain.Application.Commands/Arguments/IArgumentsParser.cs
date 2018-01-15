﻿using System.Collections.Generic;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Arguments
{
    public interface IArgumentsParser
    {
        object[] Parse(IReadOnlyList<ArgumentModel> argsModel,
            Dictionary<string, string[]> options, string[] operands);
    }
}
