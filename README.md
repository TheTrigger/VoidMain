# VoidMain
VoidMain is a framework for building command-line applications, inspired by ASP.NET Core.
Almost every part of the framework can be extended or replaced.

## This is a work in progress.

**The current state of planned features:**
- Command-line interface
  - Command-line reader **(done)**
    - Async and cancellable **(done)**
    - Masked input **(done)**
    - Hidden input **(done)**
  - Prompt message provider **(done)**
  - Commands history **(done)**
    - In-memory storage **(done)**
    - File storage
  - Command-line parser
  - Syntax highlight
  - Autocomplete
    - Command name provider
    - Option name provider
    - Enum value provider
    - File path provider
- Easy configuration with
  - Method signature
  - Attributes
  - Expression trees
- Commands execution
  - Command resolver
  - Value parsers
  - Arguments binder
- Standard commands
  - Help commands
  - Version command

## How to use?

**C# code**
```csharp
class Program : IStartup
{
    static void Main(string[] args)
    {
        var host = new CommandsHostBuilder()
            .UseConsole()
            .UseStartup<Program>()
            .Build();

        host.Run();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCommands();
    }

    public void ConfigureApplication(IApplicationBuilder app)
    {
        app.RunCommands();
    }
}

public class GreetingsModule : Module
{
    public void Hello(string name)
    {
        Output.WriteLine($"Hello, {name}!");
    }
}
```

**Command line**
```
CMD> app.exe greetings hello world
```

You can get rid of `greetings` if you set module name to `null` like this: `[Module(Name = null)]`
```
CMD> app.exe hello world
```