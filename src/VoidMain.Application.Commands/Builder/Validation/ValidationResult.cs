using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Builder.Validation
{
    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public IReadOnlyList<string> Errors { get; }

        public ValidationResult(IReadOnlyList<string> errors)
        {
            Errors = errors ?? Array.Empty<string>();
        }
    }
}
