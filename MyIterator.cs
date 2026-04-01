public class MyIterator<T> : IMyIterator<T>
{
    private Node<T>? _head;
    private Node<T>? _current;

    public MyIterator(Node<T>? head)
    {
        _head = head;
        _current = head;
    }

    public bool HasNext()
    {
        return _current != null;
    }

    public T Next()
    {
        if (_current == null)
            throw new InvalidOperationException();

        T value = _current.Value;
        _current = _current.Next;
        return value;
    }

    public void Reset()
    {
        _current = _head;
    }
}