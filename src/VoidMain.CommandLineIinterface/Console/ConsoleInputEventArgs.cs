using System;

namespace VoidMain.CommandLineIinterface.Console
{
    public class ConsoleInputEventArgs
    {
        /// <summary>
        /// This flag can hint the following input handlers to omit handling.
        /// It can be ignored if you want to handle input anyway.
        /// </summary>
        public bool IsHandledHint { get; set; }

        public ConsoleKeyInfo Input { get; set; }

        public ICommandLineView LineView { get; set; }
    }
}
