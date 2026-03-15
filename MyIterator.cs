public class MyIterator<T> : IMyIterator<T>
{
    private T[] _items;
    private int _count;
    private int _index;

    public MyIterator(T[] items, int count)
    {
        _items = items;
        _count = count;
        _index = 0;
    }

    public bool HasNext()
    {
        return _index < _count;
    }

    public T Next()
    {
        return _items[_index++];
    }

    public void Reset()
    {
        _index = 0;
    }
}