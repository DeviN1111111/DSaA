public interface IMyCollection<T>
{
    void Add(T item); // CHECK
    void Remove(T item);  // CHECK
    T FindBy<K>(K key, Func<T, K, bool> comparer);   // CHECK
    IMyCollection<T> Filter(Func<T, bool> predicate);
    void Sort(Comparison<T> comparison);
    int Count { get; } // CHECK
    bool Dirty {get; set;} // MOET NOG NIET //CHECK
    R Reduce<R>(Func<T, R, R> accumulator);
    IMyIterator<T> GetIterator();
    IEnumerator<T> GetEnumerator(); // CHECK
    T[] ToArray();
}