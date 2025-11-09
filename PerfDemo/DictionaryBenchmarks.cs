using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace PerfDemo;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class DictionaryBenchmarks
{
    private Dictionary<string, int?> _dictionary = null!;
    private readonly string[] _keys = { "key1", "key2", "key3", "key4", "key5", "key6", "key7", "key8", "key9", "key10" };
    private const string ExistingKey = "key5";
    private const string NonExistingKey = "keyNotFound";

    [GlobalSetup]
    public void Setup()
    {
        _dictionary = new Dictionary<string, int?>();

        // Pre-populate dictionary with some data
        for (int i = 0; i < _keys.Length; i++)
        {
            _dictionary[_keys[i]] = i % 2 == 0 ? i : null;
        }
    }

    [Benchmark]
    public void AddItems()
    {
        var dict = new Dictionary<string, int?>();
        for (int i = 0; i < 100; i++)
        {
            dict[$"item{i}"] = i % 3 == 0 ? null : i;
        }
    }

    [Benchmark]
    public void AddItemsWithCapacity()
    {
        var dict = new Dictionary<string, int?>(100);
        for (int i = 0; i < 100; i++)
        {
            dict[$"item{i}"] = i % 3 == 0 ? null : i;
        }
    }

    [Benchmark]
    public int? RetrieveExistingItem()
    {
        return _dictionary[ExistingKey];
    }

    [Benchmark]
    public bool TryGetExistingItem()
    {
        return _dictionary.TryGetValue(ExistingKey, out var value);
    }

    [Benchmark]
    public bool TryGetNonExistingItem()
    {
        return _dictionary.TryGetValue(NonExistingKey, out var value);
    }

    [Benchmark]
    public bool ContainsKeyExisting()
    {
        return _dictionary.ContainsKey(ExistingKey);
    }

    [Benchmark]
    public bool ContainsKeyNonExisting()
    {
        return _dictionary.ContainsKey(NonExistingKey);
    }

    [Benchmark]
    public int IterateAllItems()
    {
        int count = 0;
        foreach (var kvp in _dictionary)
        {
            if (kvp.Value.HasValue)
            {
                count += kvp.Value.Value;
            }
        }
        return count;
    }

    [Benchmark]
    public void UpdateExistingItems()
    {
        for (int i = 0; i < _keys.Length; i++)
        {
            _dictionary[_keys[i]] = i * 2;
        }
    }

    [Benchmark]
    public void AddOrUpdateItems()
    {
        for (int i = 0; i < 20; i++)
        {
            var key = $"key{i}";
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = i * 2;
            }
            else
            {
                _dictionary[key] = i;
            }
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _dictionary.Clear();
    }
}
