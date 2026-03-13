public class MyArray<T> where T :  TaskItem, IComparable<T>
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

    public void Remove(T item)
    {
        
    }
<<<<<<< HEAD
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
=======

    public void Sort() // bubble sort
    {
        for(int i = 0; i < _count - 1; i++)
        {
            for(int j = 0; j < _count - 1 - i; j++)
            {
                if(_items[j].Description.CompareTo(_items[j+1].Description) > 0)
                {
                    T temp = _items[j + 1];
                    _items[j + 1] = _items[j];
                    _items[j] = temp;
                }
            }
        }
>>>>>>> 0aaf3cac7799ce98cf94d33956430ec09ae20d30
    }

    public void PrintAll()
    {
        for(int i = 0; i < _count; i++)
        {
            Console.WriteLine(_items[i].ToString());
        }
    }

    public bool Delete(T item)
    {
        for(int i = 0; i < data.Length; i++)
        {
            if (data[i] != null && data[i].Equals(item))
            {
                Shift(i, false);
                return;
            }
        }

        return;
    }

    public void Shift(int i, bool right = true)
    {
        if(right == false) //left!
        {
            for(; i < data.Length - 1; i++)
            {
                data[i] = data[i + 1];
                data[i + 1] = default;
            }
        }
        else
        {
            for (int j = data.Length - 1; j > i; j--)
            {
                data[j] = data[j - 1];
            }

            data[i] = default;
        }
        return;
    }
}