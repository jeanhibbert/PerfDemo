using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Collections.Concurrent;

namespace PerfDemo;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class CollectionTypesBenchmarks
{
    private const int ItemCount = 1000;
    private int[] _data = null!;

    [GlobalSetup]
    public void Setup()
    {
        _data = Enumerable.Range(0, ItemCount).ToArray();
    }

    // Adding items benchmarks
    [Benchmark]
    public List<int> AddToList()
    {
        var list = new List<int>();
        for (int i = 0; i < ItemCount; i++)
        {
            list.Add(_data[i]);
        }
        return list;
    }

    [Benchmark]
    public List<int> AddToListWithCapacity()
    {
        var list = new List<int>(ItemCount);
        for (int i = 0; i < ItemCount; i++)
        {
            list.Add(_data[i]);
        }
        return list;
    }

    [Benchmark]
    public LinkedList<int> AddToLinkedList()
    {
        var list = new LinkedList<int>();
        for (int i = 0; i < ItemCount; i++)
        {
            list.AddLast(_data[i]);
        }
        return list;
    }

    [Benchmark]
    public HashSet<int> AddToHashSet()
    {
        var set = new HashSet<int>();
        for (int i = 0; i < ItemCount; i++)
        {
            set.Add(_data[i]);
        }
        return set;
    }

    [Benchmark]
    public Queue<int> AddToQueue()
    {
        var queue = new Queue<int>();
        for (int i = 0; i < ItemCount; i++)
        {
            queue.Enqueue(_data[i]);
        }
        return queue;
    }

    [Benchmark]
    public Stack<int> AddToStack()
    {
        var stack = new Stack<int>();
        for (int i = 0; i < ItemCount; i++)
        {
            stack.Push(_data[i]);
        }
        return stack;
    }

    [Benchmark]
    public SortedSet<int> AddToSortedSet()
    {
        var set = new SortedSet<int>();
        for (int i = 0; i < ItemCount; i++)
        {
            set.Add(_data[i]);
        }
        return set;
    }

    [Benchmark]
    public ConcurrentBag<int> AddToConcurrentBag()
    {
        var bag = new ConcurrentBag<int>();
        for (int i = 0; i < ItemCount; i++)
        {
            bag.Add(_data[i]);
        }
        return bag;
    }

    // Iteration benchmarks
    private List<int> _list = null!;
    private LinkedList<int> _linkedList = null!;
    private HashSet<int> _hashSet = null!;
    private int[] _array = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _list = _data.ToList();
        _linkedList = new LinkedList<int>(_data);
        _hashSet = new HashSet<int>(_data);
        _array = _data.ToArray();
    }

    [Benchmark]
    public int IterateArray()
    {
        int sum = 0;
        for (int i = 0; i < _array.Length; i++)
        {
            sum += _array[i];
        }
        return sum;
    }

    [Benchmark]
    public int IterateList()
    {
        int sum = 0;
        for (int i = 0; i < _list.Count; i++)
        {
            sum += _list[i];
        }
        return sum;
    }

    [Benchmark]
    public int IterateListForEach()
    {
        int sum = 0;
        foreach (var item in _list)
        {
            sum += item;
        }
        return sum;
    }

    [Benchmark]
    public int IterateLinkedList()
    {
        int sum = 0;
        foreach (var item in _linkedList)
        {
            sum += item;
        }
        return sum;
    }

    [Benchmark]
    public int IterateHashSet()
    {
        int sum = 0;
        foreach (var item in _hashSet)
        {
            sum += item;
        }
        return sum;
    }

    // Random access benchmarks
    [Benchmark]
    public int RandomAccessArray()
    {
        int sum = 0;
        for (int i = 0; i < 100; i++)
        {
            sum += _array[i * 10 % ItemCount];
        }
        return sum;
    }

    [Benchmark]
    public int RandomAccessList()
    {
        int sum = 0;
        for (int i = 0; i < 100; i++)
        {
            sum += _list[i * 10 % ItemCount];
        }
        return sum;
    }

    // Contains/Search benchmarks
    [Benchmark]
    public bool ArrayContains()
    {
        bool result = false;
        for (int i = 0; i < 10; i++)
        {
            result |= Array.IndexOf(_array, i * 100) >= 0;
        }
        return result;
    }

    [Benchmark]
    public bool ListContains()
    {
        bool result = false;
        for (int i = 0; i < 10; i++)
        {
            result |= _list.Contains(i * 100);
        }
        return result;
    }

    [Benchmark]
    public bool HashSetContains()
    {
        bool result = false;
        for (int i = 0; i < 10; i++)
        {
            result |= _hashSet.Contains(i * 100);
        }
        return result;
    }

    // Dictionary vs SortedDictionary vs ConcurrentDictionary
    [Benchmark]
    public Dictionary<int, int> AddToDictionary()
    {
        var dict = new Dictionary<int, int>();
        for (int i = 0; i < ItemCount; i++)
        {
            dict[i] = i * 2;
        }
        return dict;
    }

    [Benchmark]
    public SortedDictionary<int, int> AddToSortedDictionary()
    {
        var dict = new SortedDictionary<int, int>();
        for (int i = 0; i < ItemCount; i++)
        {
            dict[i] = i * 2;
        }
        return dict;
    }

    [Benchmark]
    public ConcurrentDictionary<int, int> AddToConcurrentDictionary()
    {
        var dict = new ConcurrentDictionary<int, int>();
        for (int i = 0; i < ItemCount; i++)
        {
            dict[i] = i * 2;
        }
        return dict;
    }

    // Remove benchmarks
    private List<int> _listForRemoval = null!;
    private LinkedList<int> _linkedListForRemoval = null!;
    private HashSet<int> _hashSetForRemoval = null!;

    [IterationSetup(Target = nameof(RemoveFromList))]
    public void SetupListRemoval()
    {
        _listForRemoval = _data.ToList();
    }

    [Benchmark]
    public void RemoveFromList()
    {
        for (int i = 0; i < 10; i++)
        {
            _listForRemoval.Remove(i * 100);
        }
    }

    [IterationSetup(Target = nameof(RemoveFromHashSet))]
    public void SetupHashSetRemoval()
    {
        _hashSetForRemoval = new HashSet<int>(_data);
    }

    [Benchmark]
    public void RemoveFromHashSet()
    {
        for (int i = 0; i < 10; i++)
        {
            _hashSetForRemoval.Remove(i * 100);
        }
    }

    [IterationSetup(Target = nameof(RemoveFromLinkedList))]
    public void SetupLinkedListRemoval()
    {
        _linkedListForRemoval = new LinkedList<int>(_data);
    }

    [Benchmark]
    public void RemoveFromLinkedList()
    {
        for (int i = 0; i < 10; i++)
        {
            _linkedListForRemoval.Remove(i * 100);
        }
    }
}
