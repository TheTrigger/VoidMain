# VoidMain
VoidMain is a framework for building command-line applications, insired by ASP.NET Core.
Almost every part of the framework can be extended or replaced.

## Work in progress

### Planned features
- Command-line interface
  - Syntax highlight
  - Autocomplete
  - Commands history
- Standard commands
  - Help commands
  - Version command
- Easy configuration
  - with method signature
  - with attributes
  - with expression trees

## How to use?

**C# code**
```csharp
class Program : IStartup
{
    static void Main(string[] args)
    {
        var host = new CommandsHostBuilder()
            .UseSimpleConsole()
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

You can get rid of `greetings` if you set module name to `null` like this:
```csharp
[Module(Name = null)]
```
```
CMD> app.exe hello world
```