using System.Collections;
using System.Collections.Generic;
public class MyArray<T> : IEnumerable<T> where T : TaskItem, IComparable<T>
{
    private T[] _items;
    private int _count;
    public int Count { get; }
    public bool Dirty { get; set; } = false;

    public MyArray(int capacity = 5)
    {
        _items = new T[capacity];
        _count = 0;
    }

    public int CountInArray()
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
    }

    public void PrintAll()
    {
        for(int i = 0; i < _count; i++)
        {
            Console.WriteLine(_items[i].ToString());
        }
    }

    public void Remove(T item)
    {
        for(int i = 0; i < _items.Length; i++)
        {
            if (_items[i] != null && _items[i].Equals(item))
            {
                Shift(i, true);
                _count--;
                return;
            }
        }

        return;
    }

    public void Shift(int i, bool right = true)
    {
        if(right == false) //left!
        {
            for(; i < _items.Length - 1; i++)
            {
                _items[i] = _items[i + 1];
                _items[i + 1] = default;
            }
        }
        else
        {
            for (int j = _items.Length - 1; j > i; j--)
            {
                _items[j] = _items[j - 1];
            }

            _items[i] = default;
        }
        return;
    }


    // K = int,
    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        foreach(T taskItem in _items)
        {
            if(taskItem != null)
            {
                if(comparer(taskItem, key))
                {
                    return taskItem;
                }
            }
        }
        return default!;
    }
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
        {
            yield return _items[i];
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}