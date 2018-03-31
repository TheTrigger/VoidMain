using System;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.IO.InputHandlers
{
    public class ConsoleInputEventArgs
    {
        public ConsoleKeyInfo Input { get; set; }

        public bool IsNextKeyAvailable { get; set; }

        /// <summary>
        /// This flag can hint the following input handlers to omit handling.
        /// It can be ignored if you want to handle input anyway.
        /// </summary>
        public bool IsHandledHint { get; set; }

        public ICommandLineView LineView { get; set; }
    }
}
