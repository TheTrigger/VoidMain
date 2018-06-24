using System;

namespace VoidMain.CommandLineIinterface.IO.Prompt
{
    public class PromptMessageOptions
    {
        public string Message { get; set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }

        public PromptMessageOptions(bool defaults = true)
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
