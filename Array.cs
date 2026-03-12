public class MyArray<T> : IMyCollection<T>
{
    private T[] _items;
    private int _count;

    public MyArray(int capacity = 5)
    {
        array = T[capacity];
        _count = 0;
    }

    public int Count()
    {
        return _count;
    }

    public void Add(T item)
    {
        if(_count => _items.Length)
        {
            return;
        }

        _items[_count] = item;
        _count++;
        return;
    }

    public void Remove(int index)
    {
        
    }
}