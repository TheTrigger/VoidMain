using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.CommandLineIinterface.Internal;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryManager : ICommandsHistoryManager
    {
        private readonly ICommandsHistoryStorage _storage;
        private int _isSchedulled = 0;
        private int _savePeriod;
        private PushOutCollection<string> _commands;
        private readonly int _maxCount;
        private int _current;

        public int Count { get { EnsureCommandsLoaded(); return _commands.Count; } }
        public int MaxCount => _maxCount;

        public CommandsHistoryManager(ICommandsHistoryStorage storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _savePeriod = 10_000; // TODO: Configure save period.
            _maxCount = 10; // TODO: Configure count.
        }

        private void EnsureCommandsLoaded()
        {
            if (_commands != null) return;

            var loaded = _storage.Load()
                .Where(_ => !String.IsNullOrWhiteSpace(_))
                .ToArray();

            if (loaded.Length > _maxCount)
            {
                _commands = new PushOutCollection<string>(loaded.Skip(loaded.Length - _maxCount));
            }
            else
            {
                _commands = new PushOutCollection<string>(_maxCount);
                for (int i = 0; i < loaded.Length; i++)
                {
                    _commands.Add(loaded[i]);
                }
            }

            _current = _commands.Count;
        }

        public bool TryGetPrevCommand(out string command)
        {
            EnsureCommandsLoaded();

            if (_current > 0)
            {
                _current--;
                command = _commands[_current];
                return true;
            }

            command = null;
            return false;
        }

        public bool TryGetNextCommand(out string command)
        {
            EnsureCommandsLoaded();

            if (_current < _commands.Count)
            {
                _current++;
                if (_current < _commands.Count)
                {
                    command = _commands[_current];
                    return true;
                }
            }

            command = null;
            return false;
        }

        public void AddCommand(string command)
        {
            EnsureCommandsLoaded();

            if (String.IsNullOrWhiteSpace(command) ||
                (_commands.Count > 0 && command == _commands[_commands.Count - 1]))
            {
                _current = _commands.Count;
                return;
            }

            lock (_commands)
            {
                _commands.Add(command);
            }
            _current = _commands.Count;

            ScheduleSaveCommands();
        }

        private void ScheduleSaveCommands()
        {
            if (Interlocked.CompareExchange(ref _isSchedulled, 1, 0) == 0)
            {
                Task.Delay(_savePeriod).ContinueWith(task =>
                {
                    Interlocked.CompareExchange(ref _isSchedulled, 0, 1);
                    try
                    {
                        string[] commands = null;
                        lock (_commands)
                        {
                            commands = _commands.ToArray();
                        }
                        _storage.Save(commands);
                    }
                    catch
                    {
                        // ignore
                    }
                });
            }
        }
    }
}
