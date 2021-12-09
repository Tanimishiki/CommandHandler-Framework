namespace CommandHandlerFramework;

public interface ICommand
{
    void Execute(string[] args, CommandHandlerData data);
    object? GetDescription();
}