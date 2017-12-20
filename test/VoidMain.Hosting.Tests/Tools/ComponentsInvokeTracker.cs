using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoidMain.Application.Builder;

namespace VoidMain.Hosting.Tests.Tools
{
    public class ComponentsInvokeTracker
    {
        private enum ComponentType
        {
            Log,
            Throw,
            Terminator
        }

        public List<string> Invoked { get; } = new List<string>();

        private CommandDelegate Component_N(CommandDelegate next, string name, ComponentType type)
        {
            return async context =>
            {
                Invoked.Add(name);
                if (type == ComponentType.Throw)
                {
                    throw new Exception();
                }
                if (type != ComponentType.Terminator)
                {
                    await next(context);
                }
            };
        }

        public CommandDelegate Component_A(CommandDelegate next)
        {
            return Component_N(next, "A", ComponentType.Log);
        }

        public CommandDelegate Component_B(CommandDelegate next)
        {
            return Component_N(next, "B", ComponentType.Log);
        }

        public CommandDelegate Component_C(CommandDelegate next)
        {
            return Component_N(next, "C", ComponentType.Log);
        }

        public CommandDelegate Component_Throw(CommandDelegate next)
        {
            return Component_N(next, "Throw", ComponentType.Throw);
        }

        public CommandDelegate Component_Terminator(CommandDelegate next)
        {
            return Component_N(next, "Terminator", ComponentType.Terminator);
        }

        public Task Component_Run(Dictionary<string, object> context)
        {
            Invoked.Add("Run");
            return Task.CompletedTask;
        }

        public Predicate<Dictionary<string, object>> Condition(string name, bool satisfy)
        {
            return context =>
            {
                Invoked.Add(name);
                return satisfy;
            };
        }
    }
}
