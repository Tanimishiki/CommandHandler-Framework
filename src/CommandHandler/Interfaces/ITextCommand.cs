namespace CommandHandler;

public interface ITextCommand
{
    void Execute(string[] args, PassedContents data);
}