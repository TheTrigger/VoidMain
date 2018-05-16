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
        private readonly CommandsHistoryOptions _options;
        private readonly object _commandsWriteLock;
        private PushOutCollection<string> _commands;
        private int _isSchedulled = 0;
        private int _current;

        public int MaxCount => _options.MaxCount;
        public int Count
        {
            get
            {
                EnsureCommandsLoaded();
                return _commands.Count;
            }
        }

        public CommandsHistoryManager(
            ICommandsHistoryStorage storage = null,
            CommandsHistoryOptions options = null)
        {
            _storage = storage;
            _options = options ?? new CommandsHistoryOptions();
            _options.Validate();
            _commandsWriteLock = new object();
        }

        private void EnsureCommandsLoaded()
        {
            if (_commands != null) return;

            lock (_commandsWriteLock)
            {
                if (_storage == null)
                {
                    _commands = new PushOutCollection<string>(MaxCount);
                }
                else
                {
                    IReadOnlyList<string> loaded = null;
                    lock (_storage)
                    {
                        loaded = _storage.Load();
                    }
                    loaded = loaded.Where(_ => !String.IsNullOrWhiteSpace(_)).ToArray();

                    if (loaded.Count > MaxCount)
                    {
                        _commands = new PushOutCollection<string>(loaded.Skip(loaded.Count - MaxCount));
                    }
                    else
                    {
                        _commands = new PushOutCollection<string>(MaxCount);
                        for (int i = 0; i < loaded.Count; i++)
                        {
                            _commands.Add(loaded[i]);
                        }
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
                Task.Delay(_options.SavePeriod).ContinueWith(task =>
                {
                    Interlocked.CompareExchange(ref _isSchedulled, 0, 1);
                    SaveCommands();
                });
            }
        }

        private void SaveCommands()
        {
            if (_storage == null) return;

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
                && _options.CommandsComparer.Equals(command, _commands[_commands.Count - 1]);
        }
    }
}
