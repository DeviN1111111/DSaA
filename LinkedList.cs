public class MyLinkedList<T>
{
    private Node<T>? _head;
    private int _count;

    public MyLinkedList()
    {
        _head = null;
        _count = 0;
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

    public T FindBy<K>(K key, System.Func<T, K, bool> comparer)
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
}
