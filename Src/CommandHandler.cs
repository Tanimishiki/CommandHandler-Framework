using System.Reflection;

namespace CommandHandlerFramework;

#pragma warning disable CS8618, CS8600, CS8604, CS8601
public sealed class CommandHandler
{
    private readonly Assembly _assembly;
    private readonly List<string> _commands;
    private readonly Dictionary<string, List<string>> _categories;
    private readonly Dictionary<string[], CommandInfo> _exeCommands;
    private readonly bool _withSpacePrefix;
    private readonly string[] _prefixes;
    
    /// <param name="assembly">
    /// The current assembly where the commands are declared.
    /// </param>
    /// <param name="withSpacePrefix">
    /// If the value is set to true the prefix and command name can have a whitespace which if we split the message, the first element of array is the prefix.
    /// </param>
    /// <param name="prefixes">
    /// You can set multiple prefixes any length.
    /// </param>
    public CommandHandler(Assembly assembly, bool withSpacePrefix, string[] prefixes)
    {
        _assembly = assembly;
        _commands = new List<string>();
        _categories = new Dictionary<string, List<string>>();
        _exeCommands = new Dictionary<string[], CommandInfo>();
        _withSpacePrefix = withSpacePrefix;
        _prefixes = prefixes.Select(s => s.ToLower()).ToArray();
    }

    /// <summary>
    /// This method should be called first to start the command handler.
    /// </summary>
    public void Start()
    {
        if (_assembly == null) return;

        foreach (Type type in _assembly.GetTypes())
        {
            if (!type.IsAssignableTo(typeof(ICommand))) continue;

            foreach (MethodInfo method in type.GetMethods())
            {
                CommandAttribute? cmdAtt = method.GetCustomAttribute<CommandAttribute>();
                if (cmdAtt == null) continue;

                var list = new List<string>();
                list.Add(cmdAtt.Name.ToLower());
                
                if (cmdAtt.Aliases != null)
                    list.AddRange(cmdAtt.Aliases.Select(s => s.ToLower()));
                _commands.Add(cmdAtt.Name);

                string categoryName = cmdAtt.Category ?? "Category1";
                if (_categories.TryGetValue(categoryName, out List<string> cmdsList))
                    cmdsList.Add(cmdAtt.Name);
                else
                    _categories.Add(categoryName, new List<string>() { cmdAtt.Name });

                var cmdInfo = new CommandInfo()
                {
                    Command = (ICommand) Activator.CreateInstance(type),
                    Args = cmdAtt.Args
                };
                _exeCommands.Add(list.ToArray(), cmdInfo);
            }
        }
    }

    /// <summary>
    /// Get the list of commands.
    /// </summary>
    public string[] GetCommands() => _commands.ToArray();

    /// <summary>
    /// Get the description of a specific command.
    /// </summary>
    public object? GetCommandDescription(string commandName) =>
        GetCommand(commandName)?.Command.GetDescription();
    
    /// <summary>
    /// Get the list of commands from category.
    /// </summary>
    public string[] GetCommandsFromCategory(string categoryName)
    {
        if (_categories == null) return new string[0];
        _categories.TryGetValue(categoryName, out List<string> cmds);
        return cmds?.ToArray() ?? new string[0];
    }

    /// <summary>
    /// By calling this method, this will going to find the command in the specific assembly and call it.
    /// </summary>
    /// <param name="content">
    /// The message that contains a prefix with command.
    /// </param>
    public void Execute(string content) =>
        Execute(content, null);
    /// <summary>
    /// By calling this method, this will going to find the command in the specific assembly and call it.
    /// </summary>
    /// <param name="content">
    /// The message that contains a prefix with command.
    /// </param>
    /// <param name="data">
    /// This is just an extra argument if you want to pass something to the command that you can use.
    /// </param>
    public void Execute(string content, params object[]? data)
    {
        if (string.IsNullOrEmpty(content)) return;

        string? prefix = FindPrefix(content);
        CommandInfo? cmdInfo = default;
        List<string> args = content.Split(" ").ToList();
        string[] msgArr = args.Select(s => s.ToLower()).ToArray();

        if (prefix == null) return;
        if (_withSpacePrefix)
        {
            if (prefix != msgArr[0]) return;
            if (msgArr.Length > 1)
                cmdInfo = GetCommand(msgArr[1]);
        }
        else
        {
            if (!content.StartsWith(prefix)) return;
            cmdInfo = GetCommand(msgArr[0].Substring(prefix.Length));
        }

        if (cmdInfo == null) return;

        List<string> newArgs;
        if (cmdInfo.Args > 0)
        {
            newArgs = args.GetRange(0, cmdInfo.Args);
            args.RemoveRange(0, cmdInfo.Args);
            newArgs.Add(string.Join(" ", args));
        }
        else
            newArgs = args;
        var cmdData = new CommandData()
        {
            Data = data
        };
        cmdInfo.Command.Execute(newArgs.ToArray(), cmdData);
    }

    private string? FindPrefix(string content)
    {
        foreach (string p in _prefixes)
        {
            if (content.ToLower().Contains(p) && (p[0] == content[0])) return p;
        }
        return null;
    }
    private CommandInfo? GetCommand(string commandName)
    {
        foreach (string[] commandNames in _exeCommands.Keys)
        {
            if (commandNames.Contains(commandName)) return _exeCommands[commandNames];
        }
        return null;
    }
}