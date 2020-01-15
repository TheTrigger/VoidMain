# VoidMain
VoidMain is a framework for building command-line applications. Inspired by ASP.NET Core.
Almost every part of the framework can be extended or replaced.

![demo](demo.gif)

## This is a work in progress

The framework is still in early development. The API is not final and some features are missing or incomplete. The work on the documentation and performance is not started yet.

**The current state of planned features:**
- Command-line interface
  - [x] Masked/hidden input
  - [x] Undo/redo
  - [x] Commands history
  - [x] Custom prompt message
  - [x] Syntax highlighting*
  - [ ] Autocomplete
- Standard commands
  - [x] Clear output
  - [x] Close application
  - [x] Show help
  - [ ] Show version
- Configuration
  - [x] Method signature
  - [x] Attributes
  - [ ] Expression trees

**\*** *Errors highlighting is not supported yet.*

## How to use it 

**Simple configuration**

```csharp
public class ExampleModule : CommandsModule
{
    public void Hello([Operand] string name)
    {
        Output.WriteLine($"Hello, {name}!");
    }

    [Command(Name = "command name")]
    public void Command(string[] operands, string option, bool flag = false)
    {
        Output.WriteLine("Command was executed.");
    }
}
```

```csharp
class Program : IStartup
{
    static void Main(string[] args)
    {
        CliApp.Run<Program>();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddConsoleInterface();
        services.AddCommands();
    }

    public void ConfigureApplication(IApplicationBuilder app)
    {
        app.UseHelpCommandsRewriter();
        app.RunCommands(commands =>
        {
            commands.AddStandardCommands();
            commands.AddHelpCommands();
            commands.AddModule<ExampleModule>();
        });
    }
}
```

```
CMD> example hello world
```

You can get rid of the `example` command name part if you configure the `ExcludeFromCommandName` property for the module.
```csharp
[Module(ExcludeFromCommandName = true)]
public class ExampleModule : CommandsModule { }
```
```
CMD> hello world
```

**Advanced configuration**
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddConsoleInterfaceCore()
        .AddPromptMessage("CMD> ")
        .AddUndoRedo(options =>
        {
            options.MaxCount = 10;
            options.SnapshotsComparer = LineViewSnapshotComparer.IgnoreCursor;
        })
        .AddSyntaxHighlighting(options =>
        {
            options.Palette = new CommandLineHighlightingPalette()
            {
                { CommandLineStyleName.CommandName, Color.Yellow },
                { CommandLineStyleName.OptionName, Color.Blue, Color.Yellow },
                { CommandLineStyleName.Operand, new TextStyle(Color.DarkCyan) }
                // OptionNameMarker, OptionValueMarker, OptionValue, EndOfOptions
            };
            // or
            options.Palette = CommandLineHighlightingPalette.Default;
        })
        .AddCommandsHistory(options =>
        {
            options.MaxCount = 10;
            options.SavePeriod = TimeSpan.FromSeconds(10);
            options.CommandsComparer = StringComparer.Ordinal;
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
MIT License. See [LICENSE](LICENSE) file for more details.