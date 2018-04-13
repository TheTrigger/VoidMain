using System;

namespace VoidMain.CommandLineIinterface.IO.Prompt
{
    public class ConsolePromptMessageOptions
    {
        public string Message { get; set; }

        public ConsolePromptMessageOptions(bool defaults = true)
        {
            if (defaults)
            {
                Message = "CMD> ";
            }
        }

        public void Validate()
        {
            if (Message == null)
            {
                throw new ArgumentNullException(nameof(Message));
            }
        }
    }
}
