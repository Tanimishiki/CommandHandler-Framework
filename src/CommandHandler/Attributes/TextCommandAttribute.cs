namespace CommandHandler;

[AttributeUsage(AttributeTargets.Method)]
public class TextCommandAttribute : Attribute
{
    public string Name { get; set; } = "";
    public string[] Aliases { get; set; } = new string[0];
    public int Args { get; set; }
    public bool Disabled { get; set; }
}