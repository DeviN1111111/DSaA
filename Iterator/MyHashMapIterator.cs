class MyHashMapIterator<TKey, TValue> : IMyIterator<TValue>
{
    private Entry<TKey, TValue>[] buckets;
    private int bucketIndex;
    private Entry<TKey, TValue>? current;

    public MyHashMapIterator(Entry<TKey, TValue>[] buckets)
    {
        this.buckets = buckets;
        bucketIndex = 0;
        current = null;
    }

    public bool HasNext()
    {
        if (current != null && current.Next != null)
            return true;

        for (int i = bucketIndex; i < buckets.Length; i++)
        {
            if (buckets[i] != null)
                return true;
        }

        return false;
    }

    public TValue Next()
    {
        if (current != null && current.Next != null)
        {
            current = current.Next;
            return current.Value;
        }

        while (bucketIndex < buckets.Length)
        {
            if (buckets[bucketIndex] != null)
            {
                current = buckets[bucketIndex++];
                return current.Value;
            }
            bucketIndex++;
        }

        throw new Exception("No more elements");
    }

    public void Reset()
    {
        bucketIndex = 0;
        current = null;
    }
}