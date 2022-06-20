using System.Diagnostics.CodeAnalysis;

namespace CommandHandler;

public class PassedContents
{
    private object[] _contents;
    
    public PassedContents(object[] contents) =>
        _contents = contents;
    
    /// <summary>
    /// How many items have been passed to the command.
    /// </summary>
    public int Length => _contents.Length;
    
    /// <summary>
    /// Get the item or content based on T.
    /// </summary>
    public T? Get<T>()
    {
        foreach(object content in _contents)
        {
            if (content is T) return (T) content;
        }
        return default;
    }
    
    /// <summary>
    /// Get the specific item or content based on T and index or position.
    /// </summary>
    public T? Get<T>(int index)
    {
        if ((index >= _contents.Length) || (index < 0)) return default;

        object content = _contents[index];
        if (content is T) return (T) content;
        return default;
    }

    /// <summary>
    /// Trying getting the item or content based on T.
    /// </summary>
    /// <param name="result">
    /// Where the item or content would be passed.
    /// </param>
    /// <returns>
    /// True if the item or content is present, otherwise false.
    /// </returns>
    public bool TryGet<T>([NotNullWhen(true)] out T? result)
    {
        foreach(object content in _contents)
        {
            if (content is T)
            {
                result = (T) content;
                return true;
            }
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Trying getting the specific item or content based on T and index or position.
    /// </summary>
    /// <param name="result">
    /// Where the item or content would be passed.
    /// </param>
    /// <returns>
    /// True if the item or content is present, otherwise false.
    /// </returns>
    public bool TryGet<T>(int index, [NotNullWhen(true)] out T? result)
    {
        if ((index >= _contents.Length) || (index < 0))
        {
            result = default;
            return false;
        }

        object content = _contents[index];
        if (content is T)
        {
            result = (T) content;
            return true;
        }

        result = default;
        return false;
    }
}