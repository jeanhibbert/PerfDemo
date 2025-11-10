using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Collections.Frozen;
using System.Collections.Immutable;

namespace PerfDemo;

[SimpleJob]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class FrozenCollectionBenchmarks
{
    private Dictionary<string, int> _dictionary = null!;
    private FrozenDictionary<string, int> _frozenDictionary = null!;
    private ImmutableDictionary<string, int> _immutableDictionary = null!;

    private List<string> _list = null!;
    private FrozenSet<string> _frozenSet = null!;
    private ImmutableList<string> _immutableList = null!;
    private HashSet<string> _hashSet = null!;
    private ImmutableHashSet<string> _immutableHashSet = null!;

    private const int ItemCount = 100;
    private readonly string[] _searchKeys = { "key10", "key50", "key90", "keyNotFound" };

    [GlobalSetup]
    public void Setup()
    {
        // Setup dictionaries
        var data = Enumerable.Range(0, ItemCount)
            .ToDictionary(i => $"key{i}", i => i);

        _dictionary = data;
        _frozenDictionary = data.ToFrozenDictionary();
        _immutableDictionary = data.ToImmutableDictionary();

        // Setup lists/sets
        var listData = Enumerable.Range(0, ItemCount)
     .Select(i => $"item{i}")
         .ToList();

        _list = listData;
        _frozenSet = listData.ToFrozenSet();
        _immutableList = listData.ToImmutableList();
        _hashSet = listData.ToHashSet();
        _immutableHashSet = listData.ToImmutableHashSet();
    }

    // Dictionary lookup benchmarks
    [Benchmark]
    public int DictionaryLookup()
    {
        int sum = 0;
        foreach (var key in _searchKeys)
        {
            if (_dictionary.TryGetValue(key, out var value))
            {
                sum += value;
            }
        }
        return sum;
    }

    [Benchmark]
    public int FrozenDictionaryLookup()
    {
        int sum = 0;
        foreach (var key in _searchKeys)
        {
            if (_frozenDictionary.TryGetValue(key, out var value))
            {
                sum += value;
            }
        }
        return sum;
    }

    [Benchmark]
    public int ImmutableDictionaryLookup()
    {
        int sum = 0;
        foreach (var key in _searchKeys)
        {
            if (_immutableDictionary.TryGetValue(key, out var value))
            {
                sum += value;
            }
        }
        return sum;
    }

    // Dictionary iteration benchmarks
    [Benchmark]
    public int DictionaryIteration()
    {
        int sum = 0;
        foreach (var kvp in _dictionary)
        {
            sum += kvp.Value;
        }
        return sum;
    }

    [Benchmark]
    public int FrozenDictionaryIteration()
    {
        int sum = 0;
        foreach (var kvp in _frozenDictionary)
        {
            sum += kvp.Value;
        }
        return sum;
    }

    [Benchmark]
    public int ImmutableDictionaryIteration()
    {
        int sum = 0;
        foreach (var kvp in _immutableDictionary)
        {
            sum += kvp.Value;
        }
        return sum;
    }

    // Contains benchmarks
    [Benchmark]
    public bool ListContains()
    {
        bool result = false;
        foreach (var key in _searchKeys)
        {
            result |= _list.Contains(key);
        }
        return result;
    }

    [Benchmark]
    public bool HashSetContains()
    {
        bool result = false;
        foreach (var key in _searchKeys)
        {
            result |= _hashSet.Contains(key);
        }
        return result;
    }

    [Benchmark]
    public bool FrozenSetContains()
    {
        bool result = false;
        foreach (var key in _searchKeys)
        {
            result |= _frozenSet.Contains(key);
        }
        return result;
    }

    [Benchmark]
    public bool ImmutableHashSetContains()
    {
        bool result = false;
        foreach (var key in _searchKeys)
        {
            result |= _immutableHashSet.Contains(key);
        }
        return result;
    }

    // Enumeration benchmarks
    [Benchmark]
    public int ListEnumeration()
    {
        int count = 0;
        foreach (var item in _list)
        {
            count++;
        }
        return count;
    }

    [Benchmark]
    public int ImmutableListEnumeration()
    {
        int count = 0;
        foreach (var item in _immutableList)
        {
            count++;
        }
        return count;
    }

    [Benchmark]
    public int FrozenSetEnumeration()
    {
        int count = 0;
        foreach (var item in _frozenSet)
        {
            count++;
        }
        return count;
    }

    // Count property access
    [Benchmark]
    public int DictionaryCount()
    {
        int sum = 0;
        for (int i = 0; i < 100; i++)
        {
            sum += _dictionary.Count;
        }
        return sum;
    }

    [Benchmark]
    public int FrozenDictionaryCount()
    {
        int sum = 0;
        for (int i = 0; i < 100; i++)
        {
            sum += _frozenDictionary.Count;
        }
        return sum;
    }

    [Benchmark]
    public int ImmutableDictionaryCount()
    {
        int sum = 0;
        for (int i = 0; i < 100; i++)
        {
            sum += _immutableDictionary.Count;
        }
        return sum;
    }

    // Keys/Values enumeration
    [Benchmark]
    public int DictionaryKeys()
    {
        int count = 0;
        foreach (var key in _dictionary.Keys)
        {
            count++;
        }
        return count;
    }

    [Benchmark]
    public int FrozenDictionaryKeys()
    {
        int count = 0;
        foreach (var key in _frozenDictionary.Keys)
        {
            count++;
        }
        return count;
    }

    [Benchmark]
    public int ImmutableDictionaryKeys()
    {
        int count = 0;
        foreach (var key in _immutableDictionary.Keys)
        {
            count++;
        }
        return count;
    }
}
