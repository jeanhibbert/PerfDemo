using System.Collections.Frozen;
using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;

namespace FrozenCollections;

[MemoryDiagnoser]
public class DictionaryBenchmarks
{
    private Random _random = null!;
    private List<int> _list = null!;
    
    private Dictionary<int, string> _dictionary = null!;
    private ImmutableDictionary<int, string> _immutableDictionary = null!;
    private FrozenDictionary<int, string> _frozenDictionary = null!;
    
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
    }

    // Init
    [Benchmark]
    public ImmutableDictionary<int, string> ImmutableDictionary_Init() 
        => _dictionary.ToImmutableDictionary();
    
    [Benchmark]
    public FrozenDictionary<int, string> FrozenDictionary_Init() 
        => _dictionary.ToFrozenDictionary();

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
}
