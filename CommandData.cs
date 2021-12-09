namespace CommandHandlerFramework;

public sealed class CommandData
{
    internal object[]? Data { get; set; }

    public T? GetData<T>()
    {
        if (Data == null) return default;
        
        foreach (object data in Data)
        {
            if (data is T) return (T) data;
        }
        return default;
    }

    public bool Exists<T>()
    {
        if (Data == null) return false;

        foreach (object data in Data)
        {
            if (data is T) return true;
        }
        return false;
    }

    public bool TryGetData<T>(out T? data)
    {
        if (Data == null)
        {
            data = default;
            return false;
        }

        foreach (object d in Data)
        {
            if (!(d is T)) continue;

            data = (T) d;
            return true;
        }
        
        data = default;
        return false;
    }
}