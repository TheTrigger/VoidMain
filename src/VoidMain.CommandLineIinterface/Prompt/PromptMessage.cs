using System;

namespace VoidMain.CommandLineIinterface.IO
{
    public class PromptMessage : ICommandLinePrompt
    {
        private readonly string _message;

        public PromptMessage(string message)
        {
            _message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string GetMessage() => _message;
    }
}
