# CommandHandler Framework

An easy command handler for C# .NET.

## Setup of command handler
```cs
using System.Reflection;

using CommandHandlerFramework;

namespace Testing;

class Program
{
    static void Main()
    {
        var handler = new CommandHandler(
            assembly: Assembly.GetExecutingAssembly(),
            withSpacePrefix: false,
            prefixes: new string[] { ">", "=>", "-" }
        );
        handler.Start(); // To start the command handler.

        // To run the command.
        handler.Execute(">test Hello, World!");
        handler.Execute("=>test Goodbye, World!");

        // To get the description of command.
        Console.WriteLine("\nDescription of command:");
        Console.WriteLine(handler.GetCommandDescription("test"));
        Console.WriteLine(handler.GetCommandDescription("t"));

        // To get the commands from category.
        Console.WriteLine($"\nGeneral Commands: {string.Join(" | ", handler.GetCommandsFromCategory("General"))}");

        // To get all of the commands from the assembly.
        Console.WriteLine($"\nCommands: {string.Join(" | ", handler.GetCommands())}");
    }
}
```
## Setup of command
```cs
using CommandHandlerFramework;

namespace Testing.Commands;

class Test : ICommand
{
    [Command(
        Name = "test",
        Args = 1,
        Aliases = new string[] { "t" },
        Category = "General"
    )]
    public void Execute(string[] args, CommandHandlerData data)
    {
        Console.WriteLine("The test command has been triggered.");
        Console.WriteLine($"Args: {{ {string.Join(" | ", args)} }}");
    }

    public object? GetDescription() => "This is just a test.";
}
```