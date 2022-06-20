using System.Reflection;

namespace CommandHandler;

internal class CommandData
{
    public TextCommandAttribute Attribute { get; set; } = null!;
    public ITextCommand Command { get; set; } = null!;
}