using System;
using System.Threading;

namespace VoidMain.CommandLineIinterface
{
    public class ReadingLineCanceledException : OperationCanceledException
    {
        /// <summary>
        /// Determines whether there was a user input before the reading was canceled.
        /// </summary>
        public bool HadUserInput { get; }

        public ReadingLineCanceledException(
            CancellationToken token, bool hadUserInput)
            : base(token)
        {
            HadUserInput = hadUserInput;
        }
    }
}
