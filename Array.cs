public class MyArray<T> : IMyCollection<T>
{
    private T[] _items;
    private int _count;

    public MyArray(int capacity = 5)
    {
        _items = new T[capacity];
        _count = 0;
    }

    public int Count()
    {
        return _count;
    }

    public void Add(T item)
    {
        if(_count >= _items.Length)
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
    public R Reduce<R>(Func<T, R, R> fx)
    {
        var acc = default(R);

        for (int i = 0; i < _count; i++)
            acc = fx(_items[i], acc);
        
        return acc;
    }
    public T[] Filter(Func<T, bool> predicate)
    { 
        var size = 0;
        for(int i = 0; i < _items.Length; ++i)
            size += predicate(_items[i]) ? 1 : 0; // Checks a condition and add the true results to the size of new array
            // Example: If array size = 5 and 3 are true; new size = 3;

        var result = new T[size]; // Creates new array with the new size
        for(int i = 0, j = 0; i < _items.Length && j < size; ++i) 
        {
            if(predicate(_items[i])) // Does same check as before
            result[j++] = _items[i]; // Populate new array with the value of the old array
        }
        return result;
    }
}