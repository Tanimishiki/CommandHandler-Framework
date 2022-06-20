using System.Collections;

namespace CommandHandler.Components;

public class ComandPrefixes : ICollection<string>
{
    private List<string> _list = new List<string>();

    public bool IsReadOnly => true;
    public int Count => _list.Count;

    public void Add(string prefix) =>
        _list.Add(prefix);
    
    public bool Remove(string prefix) =>
        _list.Remove(prefix);

    public IEnumerator<string> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    void ICollection<string>.Clear() { }
    bool ICollection<string>.Contains(string item) => default;
    void ICollection<string>.CopyTo(string[] array, int arrayIndex) { }
}