public interface IMyCollection<T>
{
    void Add(T item); // CHECK
    void Remove(T item);  // CHECK
    T FindBy<K>(K key, Func<T, K, bool> comparer);   // CHECK
    IMyCollection<T> Filter(Func<T, bool> predicate); // CHECK
    void Sort(Comparison<T> comparison); // 1/2 CHECK
    int Count { get; } // CHECK
    bool Dirty {get; set;} 
    R Reduce<R>(Func<T, R, R> accumulator); // CHECK
    IMyIterator<T> GetIterator();
    IEnumerator<T> GetEnumerator();
}