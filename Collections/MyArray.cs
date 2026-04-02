using System.Collections;

public class MyArray<T> : IMyCollection<T>, IEnumerable<T> where T : IComparable<T>
{
    private T[] _items;
    private int _count;
    public int Count { get { return _count; } }
    public bool Dirty { get; set; } = false;

    public MyArray(int capacity = 5)
    {
        _items = new T[capacity];
        _count = 0;
    }

    //Extra constructor
    public MyArray(T[] items)
    {
        var newArray = CloneData(items);
        _items = newArray;
        _count = items.Length;
    }

    //public toArray maken 
    public T[] ToArray()
    {
        T[] result = CloneData(_items);
        return result;
    }

    public int CountInArray()
    {
        return Count;
    }

    public void Add(T item)
    {
        if(_count >= _items.Length)
        {
            T[] newArray = new T[_items.Length + 1];
            for (int i = 0; i < _items.Length; i++)
            {
                newArray[i] = _items[i];
            }
            _items = newArray;
        }
        _items[_count] = item;
        _count++;
    }

    public R Reduce<R>(Func<T, R, R> fx)
    {
        var acc = default(R);

        for (int i = 0; i < _count; i++)
            acc = fx(_items[i], acc);
        
        return acc;
    }
    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new MyArray<T>(_count);

        for (int i = 0; i < _count; i++)
        {
            if (predicate(_items[i]))
            {
                result.Add(_items[i]);
            }
        }

        return result;
    }

    public void Sort(Comparison<T> comparison) // bubble sort
    {
        for(int i = 0; i < _count - 1; i++)
        {
            for(int j = 0; j < _count - 1 - i; j++)
            {
                if(comparison(_items[j], _items[j + 1]) > 0)
                {
                    T temp = _items[j + 1];
                    _items[j + 1] = _items[j];
                    _items[j] = temp;
                }
            }
        }
    }

    public void Remove(T key)
    {
        for(int i = 0; i < _items.Length; i++)
        {
            if (_items[i] != null && _items[i].Equals(key))
            {
                Shift(i, false);
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
            for(; i < _count - 1; i++)
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
    
    public T[] CloneData(T[] items)
    {
        var copy = new T[items.Length];

        for (int i = 0; i < Count; i++)
        {
            if(items[i] != null)
            {
                copy[i] = items[i];
            }
        }

        return copy;
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
    public IMyIterator<T> GetIterator()
    {
        return new MyArrayIterator<T>(_items, _count);
    }
}