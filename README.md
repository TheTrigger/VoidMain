# VoidMain
VoidMain is a framework for building command-line applications, inspired by ASP.NET Core.
Almost every part of the framework can be extended or replaced.

## Work in progress

**Planned features**
- Command-line interface
  - Async and cancellable input **(done)**
  - Masked input **(done)**
  - Prompt message **(done)**
  - Commands history **(done)**
  - Autocomplete
  - Syntax highlight
  - Command-line parser
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