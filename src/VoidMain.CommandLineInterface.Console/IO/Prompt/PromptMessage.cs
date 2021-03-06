﻿namespace VoidMain.CommandLineInterface.IO.Prompt
{
    public class PromptMessage : IPromptMessage
    {
        private readonly PromptMessageOptions _options;

        public PromptMessage(PromptMessageOptions options = null)
        {
            _options = options ?? new PromptMessageOptions();
            _options.Validate();
        }

        public void Write(IColoredTextWriter writer)
        {
            writer.Write(_options.Foreground, _options.Background, _options.Message);
        }
    }
}
