namespace CommandHandlerFramework;

public sealed class CommandHandlerData
{
    internal object[]? Data { get; set; }

    /// <summary>
    /// Get the specific element based on the type.
    /// </summary>
    public T? GetData<T>()
    {
        if (Data == null) return default;
        
        foreach (object data in Data)
        {
            if (data is T) return (T) data;
        }
        return default;
    }

    /// <returns>
    /// True if the type exists, otherwise false.
    /// </returns>
    public bool Exists<T>()
    {
        if (Data == null) return false;

        foreach (object data in Data)
        {
            if (data is T) return true;
        }
        return false;
    }

    /// <param name="data">
    /// This is the output if the type exists, otherwise the default value of a type.
    /// </param>
    /// <returns>
    /// True if the type exists, otherwise false.
    /// </returns>
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

    /// <param name="index">
    /// Get the specific element from 2nd argument of Execute method from <see href="CommandHandler"/>.
    /// </param>
    public T? GetElement<T>(int index)
    {
        if (Data == null) return default;
        if (index >= Data.Length) return default;
        if (Data[index] is T) return (T) Data[index];
        return default;
    }

    /// <param name="index">
    /// Position of element from 2nd argument of Execute method from <see href="CommandHandler"/>
    /// </param>
    /// <returns>
    /// True if the type exists based on index argument, otherwise false.
    /// </returns>
    public bool ElementExists<T>(int index)
    {
        if (Data == null) return false;
        if (index >= Data.Length) return false;
        return Data[index] is T;
    }

    /// <param name="index">
    /// Get the specific element from 2nd argument of Execute method from <see href="CommandHandler"/>.
    /// </param>
    /// <param name="data">
    /// This is the output if the type exists, otherwise the default value of a type.
    /// </param>
    /// <returns>
    /// True if the type exists based on index argument, otherwise false.
    /// </returns>
    public bool TryGetElement<T>(int index, out T? data)
    {
        if (Data == null)
        {
            data = default;
            return false;
        }
        if (index >= Data.Length)
        {
            data = default;
            return false;
        }
        if (Data[index] is T)
        {
            data = (T) Data[index];
            return true;
        }

        data = default;
        return false;
    }
}