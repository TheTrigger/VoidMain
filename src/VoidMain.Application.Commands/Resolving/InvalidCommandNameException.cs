using System;

namespace VoidMain.Application.Commands.Resolving
{
    public class InvalidCommandNameException : Exception
    {
        public InvalidCommandNameException() { }
        public InvalidCommandNameException(string message)
            : base(message) { }
        public InvalidCommandNameException(string message, Exception inner)
            : base(message, inner) { }
    }
}
