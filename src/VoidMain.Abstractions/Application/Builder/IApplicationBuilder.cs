using System;

namespace VoidMain.Application.Builder
{
    public interface IApplicationBuilder
    {
        IServiceProvider Services { get; }
        IApplicationBuilder New();
        IApplicationBuilder Use(Func<CommandDelegate, CommandDelegate> component);
        CommandDelegate Build();
    }
}
