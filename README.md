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
    - Undo/Redo **(done)**
  - Prompt message provider **(done)**
  - Commands history **(done)**
    - In-memory storage **(done)**
    - File storage **(done)**
  - Command-line parser **(done)**
  - Syntax highlighting **(done\*)**
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

**\*** *Errors highlighting is not supported yet.*

## How to use it (as soon as it's ready)? 

**C# code**
```csharp
class Program : IStartup
{
    static void Main(string[] args)
    {
        var host = new CommandsHostBuilder()
            .UseStartup<Program>()
            .Build();

        host.Run();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Advanced console interface includes all features.
        services.AddAdvancedConsoleInterface();
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
CMD> greetings hello world
```

You can get rid of `greetings` if you set module name to `null` like this: `[Module(Name = null)]`
```
CMD> hello world
```

**More advanced configuration**
```csharp
public void ConfigureServices(IServiceCollection services)
{
    var interfaceBuilder = services
        .AddConsoleInterface()
        .AddCmdPrompt()
        .AddUndoRedo(options =>
        {
            options.MaxCount = 10;
            options.SnapshotsComparer = CommandLineViewSnapshotComparer.IgnoreCursor;
        })
        .AddSyntaxHighlighting(options =>
        {
            options.Pallete = new ConsoleSyntaxHighlightingPallete()
                {
                    { SyntaxClass.CommandName, ConsoleColor.Yellow },
                    { SyntaxClass.OptionName, ConsoleColor.Blue, ConsoleColor.Yellow },
                    { SyntaxClass.Operand, new ConsoleTextStyle(ConsoleColor.DarkCyan) }
                };
            // or
            options.Pallete = ConsoleSyntaxHighlightingPallete.Default;
        });

    interfaceBuilder
        .AddCommandsHistory(options =>
        {
            options.MaxCount = 10;
            options.SavePeriod = TimeSpan.FromSeconds(10);
            options.CommandsComparer = CommandsHistoryComparer.OrdinalIgnoreCase;
        })
        .AddFileStorage(options =>
        {
            options.FilePath = "history.txt";
            options.Encoding = Encoding.UTF8;
        });

    services.AddCommands();
}
```

## License
MIT License. See LICENSE file for more details.