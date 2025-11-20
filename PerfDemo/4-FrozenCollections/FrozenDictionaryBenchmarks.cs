using BenchmarkDotNet.Attributes;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FrozenCollections;

[SimpleJob]
[MemoryDiagnoser]
public class FrozenDictionaryBenchmarks
{
    private Random _random = null!;
    private List<int> _list = null!;
    
    private Dictionary<int, string> _dictionary = null!;
    private ImmutableDictionary<int, string> _immutableDictionary = null!;
    private FrozenDictionary<int, string> _frozenDictionary = null!;
    private ConcurrentDictionary<int, string> _concurrentDictionary = null!;


    private int _middle;
    
    [Params(1000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _random = new Random(420);
        _list = Enumerable.Range(0, Size).Select(_ => _random.Next()).ToList();
        
        _middle = _list[Size / 2];

        _dictionary = _list.ToDictionary(x => x, x => x.ToString());
        _immutableDictionary = _dictionary.ToImmutableDictionary();
        _frozenDictionary = _dictionary.ToFrozenDictionary();
        _concurrentDictionary = new ConcurrentDictionary<int, string>(_dictionary);
    }

    // Init
    [Benchmark]
    public ImmutableDictionary<int, string> ImmutableDictionary_Init() 
        => _dictionary.ToImmutableDictionary();
    
    [Benchmark]
    public FrozenDictionary<int, string> FrozenDictionary_Init() 
        => _dictionary.ToFrozenDictionary();

    [Benchmark]
    public ConcurrentDictionary<int, string> ConcurrentDictionary_Init()
        => new ConcurrentDictionary<int, string>(_dictionary);

    // Contains
    [Benchmark]
    public bool Dictionary_ContainsKey() 
        => _dictionary.ContainsKey(_middle);
    
    [Benchmark]
    public bool ImmutableDictionary_ContainsKey() 
        => _immutableDictionary.ContainsKey(_middle);
    
    [Benchmark]
    public bool FrozenDictionary_ContainsKey() 
        => _frozenDictionary.ContainsKey(_middle);

    [Benchmark]
    public bool ConcurrentDictionary_ContainsKey()
        => _concurrentDictionary.ContainsKey(_middle);

    // Get
    [Benchmark]
    public string Dictionary_Get() 
        => _dictionary[_middle];
    
    [Benchmark]
    public string ImmutableDictionary_Get() 
        => _immutableDictionary[_middle];
    
    [Benchmark]
    public string FrozenDictionary_Get() 
        => _frozenDictionary[_middle];

    [Benchmark]
    public string ConcurrentDictionary_Get()
        => _concurrentDictionary[_middle];
}

/*
 net9.0
 | Method                           | Size | Mean           | Error       | StdDev      | Gen0   | Gen1   | Allocated |
|--------------------------------- |----- |---------------:|------------:|------------:|-------:|-------:|----------:|
| ImmutableDictionary_Init         | 1000 | 115,469.981 ns | 989.2329 ns | 925.3290 ns | 5.0049 | 0.8545 |   64096 B |
| FrozenDictionary_Init            | 1000 |   9,821.279 ns | 185.5898 ns | 173.6008 ns | 4.0131 | 0.5646 |   50560 B |
| ConcurrentDictionary_Init        | 1000 |  25,960.198 ns | 518.2130 ns | 532.1666 ns | 8.3923 | 2.7771 |  105544 B |
| Dictionary_ContainsKey           | 1000 |       1.211 ns |   0.0469 ns |   0.0439 ns |      - |      - |         - |
| ImmutableDictionary_ContainsKey  | 1000 |       9.692 ns |   0.1443 ns |   0.1350 ns |      - |      - |         - |
| FrozenDictionary_ContainsKey     | 1000 |       1.143 ns |   0.0498 ns |   0.0466 ns |      - |      - |         - |
| ConcurrentDictionary_ContainsKey | 1000 |       2.597 ns |   0.0651 ns |   0.0609 ns |      - |      - |         - |
| Dictionary_Get                   | 1000 |       1.802 ns |   0.0850 ns |   0.0795 ns |      - |      - |         - |
| ImmutableDictionary_Get          | 1000 |       7.085 ns |   0.1935 ns |   0.1901 ns |      - |      - |         - |
| FrozenDictionary_Get             | 1000 |       1.683 ns |   0.0845 ns |   0.1038 ns |      - |      - |         - |
| ConcurrentDictionary_Get         | 1000 |       2.998 ns |   0.1004 ns |   0.0940 ns |      - |      - |         - |
 */