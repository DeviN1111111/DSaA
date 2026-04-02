// public class MyIterator<T> : IMyIterator<T>
// {
//     private Node<T>? _head;
//     private Node<T>? _current;

//     public MyIterator(Node<T>? head)
//     {
//         _head = head;
//         _current = head;
//     }

//     public bool HasNext()
//     {
//         return _current != null;
//     }

//     public T Next()
//     {
//         if (_current == null)
//             throw new InvalidOperationException();

//         T value = _current.Value;
//         _current = _current.Next;
//         return value;
//     }

//     public void Reset()
//     {
//         _current = _head;
//     }
// }

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