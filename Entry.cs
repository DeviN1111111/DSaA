class Entry<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }
    public Entry<TKey, TValue>? Next { get; set; }

    public Entry(TKey key, TValue value)
    {
        Key = key;
        Value = value;
        Next = null;
    }
}