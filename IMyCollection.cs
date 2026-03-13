public interface IMyCollection<T>
{
    void Add(T item);
    void Remove(T item);
    T FindBy<K>(K key, Func<T, K, bool> comparer);
    IMyCollection<T> Filter(Func<T, bool> predicate);
    void Sort(Comparison<T> comparison);
    int Count { get; }
    bool Dirty {get; set;}
    R Reduce<R>(Func<T, R, R> accumulator);
    // IMyIterator<T> GetIterator();
    IEnumerator<T> GetEnumerator();
}