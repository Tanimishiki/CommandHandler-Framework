namespace CommandHandlerFramework;

#pragma warning disable CS8618
internal sealed class CommandInfo
{
    public ICommand Command { get; set; }
    public int Args { get; set; }
}