namespace CommandHandlerFramework;

#pragma warning disable CS8618

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class CommandAttribute : Attribute
{
    public string Name { get; set; }
    public int Args { get; set; }
    public string[]? Aliases { get; set; }
    public string? Category { get; set; }
}