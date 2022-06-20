# CommandHandler Framework

An easy command handler for C# .NET.

## Setup of command handler
```cs
using CommandHandler;
using CommandHandler.Components;
using CommandHandler.Enums;
using System.Reflection;

namespace Main;

class Program
{
    static void Main(string[] args)
    {
        var text = new TextCH(
            assembly: Assembly.GetExecutingAssembly(),
            prefixes: new ComandPrefixes() { ">", ">/" }
        );
        text.SetMode(CommandMode.ConnectedPrefix);

        CommandResponse response = text.Execute(">t", "Hello!", "Goodbye!");

        Console.WriteLine($"Status: {response.Status}");
        Console.WriteLine($"Prefix Used: {response.PrefixUsed}");
        Console.WriteLine($"Command Used: {response.CommandUsed}");
    }
}
```
## Setup of command
```cs
using CommandHandler;

namespace Testing.Commands;

class Test : ITextCommand
{
    [TextCommand(
        Name = "test",
        Aliases = new string[] { "t" },
        Args = 1,
        Disabled = false
    )]
    public void Execute(string[] args, PassedContents data)
    {
        // Expected output:
        //      Message: Hello!
        Console.WriteLine($"Message: {data.Get<string>(index: 0)}");
    }
}
```