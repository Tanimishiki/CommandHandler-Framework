namespace CommandHandlerFramework;

public interface ICommand
{
    void Execute(string[] args, CommandData data);
    object? GetDescription();
}