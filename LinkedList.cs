public class MyLinkedList<T> : IMyCollection<T>, IEnumerable<T>, IMyIterator<T> where T : TaskItem, IComparable<T>
{
    private MyIterator<T>? _head;
    private int _count;

    public MyLinkedList()
    {
        _head = null;
        _count = 0;
    }

    public void Add(T node)
    {
        var newNode = new MyIterator<T>(node);

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
        MyIterator<T>? current = _head;
        MyIterator<T>? previous = null;

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
}
