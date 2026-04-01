using System.Collections;

public class MyLinkedList<T> : IMyCollection<T>, IEnumerable<T> where T : IComparable<T>
{
    private Node<T>? _head;
    private int _count;
    public int Count { get {return _count;} }
    public bool Dirty {get; set;}

    public MyLinkedList()
    {
        _head = null;
        _count = 0;
    }

    public MyLinkedList(T[] items)
    {
        _head = null;
        _count = 0;
        if (items != null)
        {
            foreach (var item in items)
                Add(item);
        }
    }

    public void Add(T node)
    {
        var newNode = new Node<T>(node);

        if (_head == null)
        {
            _head = newNode;
        }
        else
        {
            var current = _head;

            while (current.Next != null)
            {
                current = current.Next;
            }

            current.Next = newNode;
        }

        _count++;
    }

    public void Remove(T node)
    {
        if(node == null || _head == null)
        {
            return;
        }
        
        Node<T>? current = _head;
        Node<T>? previous = null;

        while(current != null)
        {
            if(current.Value.Equals(node))
            {
                if(previous == null)
                {
                    _head = current.Next;
                }
                else
                {
                    previous.Next = current.Next;
                }

                _count--;
                return;
            }
            previous = current;
            current = current.Next;
        }
    }
    public int CountInLinkedList()
    {
        return _count;
    }

    public T[] ToArray()
    {
        T[] result = new T[_count];
        int i = 0;
        Node<T>? current = _head;
        while (current != null)
        {
            result[i++] = current.Value;
            current = current.Next;
        }
        return result;
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        Node<T>? current = _head;
        while (current != null)
        {
            if (current.Value != null && comparer(current.Value, key))
                return current.Value;
            current = current.Next;
        }
        return default!;
    }

    public void Sort(Comparison<T> comparison)
    {
        Node<T>? current = _head;

        while(current!.Next != null)
        {
            if (comparison(current!.Value, current.Next!.Value) > 0)
            {
                T temp = current.Value;
                current.Next.Value = current.Value;
                current.Value = temp;
            }
            current = current.Next;
        }
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        MyLinkedList<T> result = new MyLinkedList<T>();
        Node<T>? current = _head;

        while(current != null)
        {
            if(predicate(current.Value))
            {
                result.Add(current.Value);
            }
            current = current.Next;
        }

        return result;
    }

    
    public R Reduce<R>(Func<T, R, R> accumulator)
    {
        R result = default(R)!;
        Node<T> current = _head!;

        while(current != null)
        {
            accumulator(current.Value, result);
            current = current.Next!;
        }

        return result;
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        Node<T>? current = _head;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public IMyIterator<T> GetIterator()
    {
        return new MyIterator<T>(_head);
    }
}