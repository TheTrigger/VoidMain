﻿using System;
using System.Linq;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryInMemoryStorage : ICommandsHistoryStorage
    {
        private string[] _commands;

        public CommandsHistoryInMemoryStorage()
        {
            // TODO: Allow to configure commands.
            _commands = Array.Empty<string>();
        }

        public string[] Load()
        {
            return _commands.ToArray();
        }

        public void Save(string[] commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }
            _commands = commands.ToArray();
        }
    }
}