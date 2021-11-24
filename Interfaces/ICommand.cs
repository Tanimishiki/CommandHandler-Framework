namespace CommandHandlerFramework;

public interface ICommand
{
    void Execute(string[] args, params object[]? data);
    object? GetDescription();
}