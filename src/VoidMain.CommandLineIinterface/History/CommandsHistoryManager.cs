using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.CommandLineIinterface.Internal;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryManager : ICommandsHistoryManager, IDisposable
    {
        private readonly ICommandsHistoryStorage _storage;
        private readonly IEqualityComparer<string> _comparer;
        private readonly object _commandsWriteLock;
        private PushOutCollection<string> _commands;
        private int _isSchedulled = 0;
        private int _current;

        private TimeSpan SavePeriod { get; }
        public int MaxCount { get; }
        public int Count
        {
            get
            {
                EnsureCommandsLoaded();
                return _commands.Count;
            }
        }

        public CommandsHistoryManager(
            ICommandsHistoryStorage storage, CommandsHistoryOptions options = null)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _commandsWriteLock = new object();
            _comparer = options?.CommandsComparer ?? CommandsHistoryComparer.OrdinalIgnoreCase;
            SavePeriod = options?.SavePeriod ?? TimeSpan.FromSeconds(10.0);
            MaxCount = options?.MaxCount ?? 10;
            if (SavePeriod.TotalMilliseconds < 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(SavePeriod));
            }
            if (MaxCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxCount));
            }
        }

        private void EnsureCommandsLoaded()
        {
            if (_commands != null) return;

            lock (_commandsWriteLock)
            {
                string[] loaded = null;
                lock (_storage)
                {
                    loaded = _storage.Load();
                }
                loaded = loaded.Where(_ => !String.IsNullOrWhiteSpace(_)).ToArray();

                if (loaded.Length > MaxCount)
                {
                    _commands = new PushOutCollection<string>(loaded.Skip(loaded.Length - MaxCount));
                }
                else
                {
                    _commands = new PushOutCollection<string>(MaxCount);
                    for (int i = 0; i < loaded.Length; i++)
                    {
                        _commands.Add(loaded[i]);
                    }
                }

                _current = _commands.Count;
            }
        }

        public bool TryGetPrevCommand(out string command)
        {
            EnsureCommandsLoaded();

            if (HasPrev())
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
                IsEqualsToLastCommand(command))
            {
                _current = _commands.Count;
                return;
            }

            lock (_commandsWriteLock)
            {
                _commands.Add(command);
                _current = _commands.Count;
            }

            ScheduleSaveCommands();
        }

        public void Clear()
        {
            lock (_commandsWriteLock)
            {
                if (_commands == null)
                {
                    _commands = new PushOutCollection<string>(MaxCount);
                }
                else
                {
                    _commands.Clear();
                }
                _current = 0;
            }

            ScheduleSaveCommands();
        }

        private void ScheduleSaveCommands()
        {
            if (Interlocked.CompareExchange(ref _isSchedulled, 1, 0) == 0)
            {
                Task.Delay(SavePeriod).ContinueWith(task =>
                {
                    Interlocked.CompareExchange(ref _isSchedulled, 0, 1);
                    SaveCommands();
                });
            }
        }

        private void SaveCommands()
        {
            try
            {
                string[] commands = null;
                lock (_commandsWriteLock)
                {
                    commands = _commands.ToArray();
                }
                lock (_storage)
                {
                    _storage.Save(commands);
                }
            }
            catch
            {
                // ignore
            }
        }

        public void Dispose()
        {
            SaveCommands();
        }

        private bool HasPrev()
        {
            return _current > 0;
        }

        private bool IsEqualsToLastCommand(string command)
        {
            return _commands.Count > 0
                && _comparer.Equals(command, _commands[_commands.Count - 1]);
        }
    }
}
