namespace CommandHandler.Utilities;

internal static class SpliceMethod
{
    public static List<T> Splice<T>(this List<T> source, int index, int count)
    {
        List<T> result = source.GetRange(index, count);
        source.RemoveRange(index, count);
        return result;
    }
}