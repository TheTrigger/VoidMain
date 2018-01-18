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
  - Method signature **(done)**
  - Attributes **(done)**
  - Expression trees
- Commands execution
  - Command resolver
  - Arguments parsers **(done)**
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
        app.RunCommands(commands =>
        {
            commands.AddModule<GreetingsModule>();
        });
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

You can get rid of `greetings` if you set the module name to an empty string like this: `[Module(Name = "")]`
```
CMD> hello world
```

**More advanced configuration**
```csharp
public void ConfigureServices(IServiceCollection services)
{
    var interfaceBuilder = services
        .AddConsoleInterface()
        .AddPromptMessage()
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
                // OptionNameMarker, OptionValueMarker, OptionValue, OperandsSectionMarker
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

## Known issues

- **Application closes instead of canceling the current operation after pressing `Ctrl+C` if it was started with `dotnet run`.**<br>This is due to the [issue](https://github.com/dotnet/cli/issues/812) in the .NET CLI. Use `dotnet publish` and run the compiled executable instead.
- **Command line reader is not working on Linux as expected (at all).**<br>Terminal is working differently than the Windows console. .NET team tried to make a `Console` API to behave the same way on all platforms with many hacks and compromises, but it is still have differences and bugs. `PowerShell` have some native calls to make it work on Linux. I hope, someday I can make it work too.

## License
MIT License. See LICENSE file for more details.