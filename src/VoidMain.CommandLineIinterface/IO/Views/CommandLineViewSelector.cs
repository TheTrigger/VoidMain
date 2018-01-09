using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class CommandLineViewSelector : ICommandLineViewSelector
    {
        private readonly Dictionary<CommandLineViewType, ICommandLineViewProvider> _providers;

        public CommandLineViewSelector(IEnumerable<ICommandLineViewProvider> providers)
        {
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }

            _providers = new Dictionary<CommandLineViewType, ICommandLineViewProvider>();
            foreach (var provider in providers)
            {
                _providers[provider.ViewType] = provider;
            }
        }

        public ICommandLineView SelectView(CommandLineViewOptions options)
        {
            if (!_providers.TryGetValue(options.ViewType, out var provider))
            {
                throw new NotSupportedException($"{options.ViewType} view is not supported.");
            }

            return provider.GetView(options);
        }
    }
}
