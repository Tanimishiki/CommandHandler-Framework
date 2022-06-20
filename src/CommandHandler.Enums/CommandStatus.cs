namespace CommandHandler.Enums;

public enum CommandStatus
{
    /// <summary>
    /// When the command does exist.
    /// </summary>
    VALID,
    /// <summary>
    /// When the command does not exist.
    /// </summary>
    INVALID,
    /// <summary>
    /// When the command is disabled.
    /// </summary>
    NOT_AVAILABLE,
    /// <summary>
    /// When the prefix is not defined or stated in the text.
    /// </summary>
    NO_CALL
}