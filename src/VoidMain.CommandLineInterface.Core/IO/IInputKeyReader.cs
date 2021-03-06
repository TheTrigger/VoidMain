﻿using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineInterface.IO
{
    public interface IInputKeyReader
    {
        Task<InputKeyInfo> ReadKeyAsync(bool intercept = false, CancellationToken token = default);
    }
}
