using CommandHandler.Enums;
using CommandHandler.Components;
using System.Reflection;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using CommandHandler.Utilities;

namespace CommandHandler;

public class TextCH
{
    private readonly ComandPrefixes _prefixes;
    private readonly List<CommandData> _commands = new List<CommandData>();
    private CommandMode _mode = CommandMode.ConnectedPrefix;
    private bool _started;

    public TextCH(Assembly assembly, ComandPrefixes prefixes)
    {
        if (assembly == null)
            throw new InvalidOperationException("The 'assembly' or 1st argument cannot be set to null.");
        if (prefixes == null)
            throw new InvalidOperationException("The 'prefixes' or 2nd argument cannot be set to null.");
        _prefixes = prefixes;

        Start(assembly);
    }

    private void Start(Assembly assembly)
    {
        if (_started)
            throw new InvalidOperationException($"The '{this.GetType().Name}.Start' method should be called once.");
        _started = true;

        IEnumerable<Type> types = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ITextCommand)));
        foreach (Type type in types)
        {
            MethodInfo? method = type.GetMethod("Execute");
            TextCommandAttribute? attribute = method?.GetCustomAttribute<TextCommandAttribute>();

            if ((method == null) || (attribute == null) || !method.IsDefined(typeof(TextCommandAttribute)))
            {
                FailedToLoadCommandMessage(type);
                continue;
            }
            
            try
            {
                ITextCommand cmd = (ITextCommand) Activator.CreateInstance(type)!;
                var data = new CommandData()
                {
                    Attribute = attribute,
                    Command = cmd
                };

                _commands.Add(data);
            }
            catch
            {
                FailedToLoadCommandMessage(type);
            }
        }
    }

    private void FailedToLoadCommandMessage(Type type) =>
        Console.WriteLine($"Command Handler: Failed to load '{type.Name}' command.");
    
    /// <summary>
    /// This method is used to run or trigger the specific command.
    /// </summary>
    public CommandResponse Execute(string command, params object[] args)
    {
        if (!_started)
            throw new InvalidOperationException("The Command Handler has not been started.");

        string cmdName = "";
        List<string> strings = command.Split(" ").ToList();

        var response = new CommandResponse();

        if (!HasValidPrefix(command, out string? prefix))
        {
            response._Status = CommandStatus.NO_CALL;
            return response;
        }
        
        string firstElement = strings[0];
        if (_mode == CommandMode.SeparatedPrefix)
            cmdName = strings[1];
        else
            cmdName = firstElement.Substring(prefix.Length, firstElement.Length - prefix.Length);
        response._PrefixUsed = prefix;

        if (!TryGetCommand(cmdName, out CommandData? data))
        {
            response._Status = CommandStatus.INVALID;
            return response;
        }

        if (data.Attribute.Disabled)
        {
            response._Status = CommandStatus.NOT_AVAILABLE;
            return response;
        }

        response._CommandUsed = data.Attribute.Name;
        response._Status = CommandStatus.VALID;

        if (_mode == CommandMode.SeparatedPrefix)
            strings.RemoveAt(0);
        else
        {
            string newCmd = firstElement.Replace(prefix, "");
            strings[0] = newCmd;
        }
        
        int cmdArgs = data.Attribute.Args <= 0 ? 1 : data.Attribute.Args;
        if (cmdArgs > strings.Count)
        {
            int missingCount = cmdArgs - strings.Count;
            for (int i = 0; i < missingCount; i++)
                strings.Add("");
        }

        List<string> argsRes = strings.Splice(0, cmdArgs-1);
        argsRes.Add(string.Join(" ", strings));

        data.Command.Execute(argsRes.ToArray(), new PassedContents(args));
        
        return response;
    }

    private bool TryGetCommand(string name, [NotNullWhen(true)] out CommandData? commandData)
    {
        name = name.ToLower();

        foreach (CommandData data in _commands)
        {
            string cmdName = data.Attribute.Name.ToLower();
            IEnumerable<string> cmdAliases = data.Attribute.Aliases.Select(cmd => cmd.ToLower());

            if ((cmdName == name) || cmdAliases.Contains(name))
            {
                commandData = data;
                return true;
            }
        }

        commandData = null;
        return false;
    }

    /// <summary>
    /// How the prefix should be used.
    /// </summary>
    public void SetMode(CommandMode mode) =>
        _mode = mode;

    private bool HasValidPrefix(string command, [NotNullWhen(true)] out string? prefix)
    {
        string[] stringArr = command.Split(" ");
        string firstElement = stringArr[0];
        bool isValidPrefix = false;
        string? prefixResult = null;

        if (_mode == CommandMode.SeparatedPrefix)
        {
            foreach (string p in _prefixes)
            {
                if (p == firstElement)
                {
                    isValidPrefix = true;
                    prefixResult = p;
                }
            }

            prefix = prefixResult;
            return isValidPrefix;
        }

        var stringBuilder = new StringBuilder();
        
        foreach (string p in _prefixes)
        {
            for (int i = 0; i < firstElement.Length; i++)
            {
                char c = firstElement[i];
                if ((i <= p.Length-1) && (c == p[i]))
                    stringBuilder.Append(c);
            }
            
            if (stringBuilder.ToString() == p)
                prefixResult = p;
            stringBuilder.Clear();
        }

        if (prefixResult?.Length > 0)
            isValidPrefix = true;
        else
            isValidPrefix = false;
        
        prefix = prefixResult;
        return isValidPrefix;
    }
}
