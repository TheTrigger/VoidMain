using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Builder;
using VoidMain.CommandLineIinterface.Parser;

namespace VoidMain.CommandLineIinterface.Console
{
    public class SimpleConsoleCommandLineIinterface : ICommandLineIinterface
    {
        private readonly IConsole _console;
        private readonly ICommandLineParser _parser;
        private Task _workingLoop;
        public bool IsRunning { get; private set; }

        public SimpleConsoleCommandLineIinterface(IConsole console, ICommandLineParser parser)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            IsRunning = false;
        }

        public Task StartAsync(CommandDelegate application, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            if (IsRunning)
            {
                throw new InvalidOperationException("Command-line interface is already started.");
            }
            IsRunning = true;
            _workingLoop = Task.Run(async () => await WorkingLoop(application).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            IsRunning = false;
            return WaitForShutdownAsync(token);
        }

        public Task WaitForShutdownAsync(CancellationToken token = default(CancellationToken))
        {
            if (_workingLoop == null)
            {
                return Task.CompletedTask;
            }

            if (!token.CanBeCanceled)
            {
                return _workingLoop;
            }

            var canceled = new TaskCompletionSource<object>();
            token.Register(canceled.SetCanceled);
            return Task.WhenAny(_workingLoop, canceled.Task);
        }

        private async Task WorkingLoop(CommandDelegate application)
        {
            var comparer = StringComparer.OrdinalIgnoreCase;

            while (IsRunning)
            {
                _console.Write("CMD> ");
                string commandLine = _console.ReadLine();

                if (String.IsNullOrEmpty(commandLine))
                {
                    _console.WriteLine();
                    continue;
                }

                if (comparer.Equals(commandLine, "q") ||
                    comparer.Equals(commandLine, "quit"))
                {
                    break;
                }

                try
                {
                    var context = new Dictionary<string, object>();
                    _parser.ParseToContext(commandLine, context);

                    await application(context).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _console.WriteLine("Error: " + ex.Message);
                    _console.WriteLine();
                }
            }
            IsRunning = false;
        }
    }
}
