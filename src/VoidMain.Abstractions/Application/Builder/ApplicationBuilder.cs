using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoidMain.Application.Builder
{
    public class ApplicationBuilder : IApplicationBuilder
    {
        private readonly IList<Func<CommandDelegate, CommandDelegate>> _components;
        public IServiceProvider Services { get; }

        public ApplicationBuilder(IServiceProvider services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            _components = new List<Func<CommandDelegate, CommandDelegate>>();
        }

        public IApplicationBuilder New() => new ApplicationBuilder(Services);

        public IApplicationBuilder Use(Func<CommandDelegate, CommandDelegate> component)
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }
            _components.Add(component);
            return this;
        }

        public CommandDelegate Build()
        {
            CommandDelegate handler = DefaultHandler;

            foreach (var component in _components.Reverse())
            {
                handler = component(handler);
            }

            return handler;
        }

        private static Task DefaultHandler(Dictionary<string, object> context)
        {
            throw new InvalidOperationException("Unknown command.");
        }
    }
}
