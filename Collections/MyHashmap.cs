class MyHashMap<Tkey, TValue> : IMyCollection<TValue>
{
    private Entry<Tkey, TValue>?[] buckets;
    private int Capacity;
    private int count;
    private Func<TValue, Tkey>? keySelector;
    public bool Dirty { get; set; } = false;
    public int Count => count;
    private int defaultCapacity = 5;

    public MyHashMap(int capacity = 5)
    {
        Capacity = capacity;
        buckets = new Entry<Tkey, TValue>[capacity];
        count = 0;
        keySelector = null;
    }
    public MyHashMap(Func<TValue, Tkey> keySelector, TValue[] items)
    {
        int initialCapacity = Math.Max(defaultCapacity, items.Length);

        buckets = new Entry<Tkey, TValue>[initialCapacity];
        Capacity = initialCapacity;
        count = 0;
        this.keySelector = keySelector;

        foreach (var item in items)
        {
            if(item == null) continue;
            Add(item);
        }
    }
    public MyHashMap(Func<TValue, Tkey> keySelector) : this(5)
    {
        this.keySelector = keySelector;
    }

    private int GetIndex(Tkey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));
        int hash = key.GetHashCode(); // Hash the key into an integer -> "35421" -> 35421 % Capacity("5") -> 1 so it goes to index 1
        int index = Math.Abs(hash) % Capacity; // hash has to be postive
        return index;
    }

    public void Add(TValue item)
    {
        if(keySelector == null)
            throw new InvalidOperationException("keySelector must be set to add items");

        if(count >= Capacity / 1.25) // Load factor of 80% (Blijkbaar is dit sneller dan 100% load factor)
        {
            Resize();
        }

        Tkey key = keySelector(item);
        var entry = new Entry<Tkey, TValue>(key, item);
        int index = GetIndex(key);
    
        if(buckets[index] == null)
        {
            buckets[index] = entry;
            count++;
            return;
        }

        var current = buckets[index];
        
        while(current != null)
        {
            if(current.Key.Equals(key))
            {
                current.Value = item; // Update the value if the key already exists
                return;
            }
            if(current.Next == null)
            {
                current.Next = entry; // Add the new entry at the end of the node chain
                count++;
                return;
            }
            current = current.Next;
        }
    }
    public void Remove(TValue item)
    {
        if(keySelector == null)
            throw new InvalidOperationException("keySelector must be set to remove items");

        Tkey key = keySelector(item);
        int index = GetIndex(key);
        var current = buckets[index];
        Entry<Tkey, TValue>? previous = null;
        while(current != null)
        {
            if(current.Key.Equals(key))
            {
                if(previous == null)
                {
                    buckets[index] = current.Next;
                }
                else
                {
                    previous.Next = current.Next;
                }
                count--;
                return;
            }
            previous = current;
            current = current.Next;
        }
    }
    private void Resize()
    {
        int newCapacity = Capacity * 2;
        var newBuckets = new Entry<Tkey, TValue>[newCapacity];
        // Hele buckets array moet opnieuw gehashed worden 
        for(int i = 0; i < buckets.Length; i++)
        {
            var current = buckets[i];
            while(current != null)
            {
                var next = current.Next; // Store the next entry before rehashing 
                int newIndex = Math.Abs(current.Key.GetHashCode()) % newCapacity; 

                current.Next = newBuckets[newIndex];
                newBuckets[newIndex] = current;
                current = next; 
            }
        }
        buckets = newBuckets;
        Capacity = newCapacity;
    }
    public TValue FindBy<K>(K key, Func<TValue, K, bool> comparer)
    {
        for(int i = 0; i < buckets.Length; i++)
        {
            var current = buckets[i];
            while(current != null)
            {
                if(comparer(current.Value, key))
                {
                    return current.Value;
                }
                current = current.Next;
            }
        }
        return default!;
    }

    public TValue[] ToArray()
    {
        int totalEntries = 0;
        for(int i = 0; i < buckets.Length; i++)
        {
            var current = buckets[i];
            while(current != null)
            {
                totalEntries++;
                current = current.Next;
            }
        }

        TValue[] result = new TValue[totalEntries];
        int index = 0;
        for(int i = 0; i < buckets.Length; i++)
        {
            var current = buckets[i];
            while(current != null)
            {
                result[index++] = current.Value;
                current = current.Next;
            }
        }
        return result;
    }

    public IMyCollection<TValue> Filter(Func<TValue, bool> predicate)
    {
        if(keySelector == null)
            throw new InvalidOperationException("keySelector must be set to filter items");

        var filteredMap = new MyHashMap<Tkey, TValue>(keySelector);

        var items = ToArray();

        for(int i = 0; i < items.Length; i++)
        {
            if(predicate(items[i]))
            {
                filteredMap.Add(items[i]);
            }
        }
        return filteredMap;
    }
    public R Reduce<R>(Func<TValue, R, R> fx)
    {
        var items = ToArray();
        R acc = default!;

        for (int i = 0; i < items.Length; i++)
        {
            acc = fx(items[i], acc);
        }

        return acc;
    }

    public void Sort(Comparison<TValue> comparison)
    {
        var items = ToArray();

        // Bubble sort
        for (int i = 0; i < items.Length - 1; i++)
        {
            for (int j = 0; j < items.Length - 1 - i; j++)
            {
                if (comparison(items[j], items[j + 1]) > 0)
                {
                    var temp = items[j];
                    items[j] = items[j + 1];
                    items[j + 1] = temp;
                }
            }
        }

        buckets = new Entry<Tkey, TValue>[Capacity];
        count = 0;

        for (int i = 0; i < items.Length; i++)
        {
            Add(items[i]);
        }
    }
    public IEnumerator<TValue> GetEnumerator()
    {
        for (int i = 0; i < buckets.Length; i++)
        {
            var current = buckets[i];

            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
    }
    public IMyIterator<TValue> GetIterator()
    {
        return new MyHashMapIterator<Tkey, TValue>(buckets!);
    }

}