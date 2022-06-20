using CommandHandler.Enums;

namespace CommandHandler;

public class CommandResponse
{
    public string CommandUsed => _CommandUsed;
    internal string _CommandUsed { get; set; } = "";

    public string PrefixUsed => _PrefixUsed;
    internal string _PrefixUsed { get; set; } = "";

    public CommandStatus Status => _Status;
    internal CommandStatus _Status { get; set; }
}