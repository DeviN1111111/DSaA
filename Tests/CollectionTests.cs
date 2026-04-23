using System;
using Xunit;

public abstract class CollectionTestsBase
{
    protected abstract IMyCollection<int> CreateCollection();

    protected IMyCollection<int> CreateCollectionWithItems(params int[] items)
    {
        var collection = CreateCollection();

        foreach (var item in items)
        {
            collection.Add(item);
        }

        return collection;
    }

    // Add
    [Fact] // Hij moet altijd true zijn, anders faalt de test
    public void Add_ShouldAddItem()
    {
        var collection = CreateCollection();

        collection.Add(10);

        Assert.Equal(1, collection.Count);
        Assert.Contains(10, collection.ToArray());
    }

    // Remove
    [Fact]
    public void Remove_ShouldRemoveItem()
    {
        var collection = CreateCollectionWithItems(1, 2, 3);

        collection.Remove(2);

        Assert.Equal(2, collection.Count);
        Assert.DoesNotContain(2, collection.ToArray());
    }

    // FindBy
    [Fact]
    public void FindBy_ShouldReturnMatchingItem()
    {
        var collection = CreateCollectionWithItems(1, 2, 3);

        var result = collection.FindBy(2, (item, key) => item == key);

        Assert.Equal(2, result);
    }

    // Filter
    [Fact]
    public void Filter_ShouldReturnMatchingItems()
    {
        var collection = CreateCollectionWithItems(1, 2, 3, 4);

        var result = collection.Filter(x => x % 2 == 0);

        var filteredItems = result.ToArray();

        Assert.Equal(2, result.Count);
        Assert.Contains(2, filteredItems);
        Assert.Contains(4, filteredItems);
        Assert.DoesNotContain(3, filteredItems);
        Assert.DoesNotContain(1, filteredItems);
    }

    // Sort
    [Fact]
    public void Sort_ShouldOrderItemsAscending()
    {
        var collection = CreateCollectionWithItems(6, 5, 4, 3, 2, 1);

        collection.Sort((a, b) => a.CompareTo(b));

        var result = collection.ToArray();

        Assert.Equal(new[] { 1, 2, 3, 4, 5, 6 }, result);
    }

    // Count
    [Fact]
    public void Count_ShouldReturnCorrectCount()
    {
        var collection = CreateCollectionWithItems(1, 2, 3);

        Assert.Equal(3, collection.Count);
    }

    // Reduce
    [Fact]
    public void Reduce_ShouldReturnCombinedValue()
    {
        var collection = CreateCollectionWithItems(1, 2, 3);

        var result = collection.Reduce<int>((item, acc) => acc + item);

        Assert.Equal(6, result);
    }

    // GetIterator
    [Fact]
    public void GetIterator_ShouldReturnIterator()
    {
        var collection = CreateCollectionWithItems(1, 2);

        var iterator = collection.GetIterator();

        Assert.NotNull(iterator);
    }

    // GetEnumerator
    [Fact]
    public void GetEnumerator_ShouldReturnAllItems()
    {
        var collection = CreateCollectionWithItems(1, 2, 3);

        var enumerator = collection.GetEnumerator();

        int count = 0;

        while (enumerator.MoveNext())
        {
            count++;
        }

        Assert.Equal(3, count);
    }

    // ToArray
    [Fact]
    public void ToArray_ShouldReturnAllItems()
    {
        var collection = CreateCollectionWithItems(1, 2, 3, 4, 5, 6);

        var result = collection.ToArray();

        Assert.Equal(6, result.Length);
        Assert.Contains(1, result);
        Assert.Contains(2, result);
        Assert.Contains(3, result);
        Assert.Contains(4, result);
        Assert.Contains(5, result);
        Assert.Contains(6, result);
    }
}


// MyArray Tests
public class MyArrayTests : CollectionTestsBase
{
    protected override IMyCollection<int> CreateCollection()
    {
        return new MyArray<int>();
    }
}


// MyLinkedList Tests
public class MyLinkedListTests : CollectionTestsBase
{
    protected override IMyCollection<int> CreateCollection()
    {
        return new MyLinkedList<int>();
    }
}


// MyHashMap Tests
public class MyHashMapTests : CollectionTestsBase
{
    protected override IMyCollection<int> CreateCollection()
    {
        return new MyHashMap<int, int>(x => x);
    }
}