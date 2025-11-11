using System.Collections.Frozen;
using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;

namespace FrozenCollections;

[MemoryDiagnoser]
public class SetBenchmarks
{
    private Random _random = null!;
    private List<int> _list = null!;
    
    private HashSet<int> _hashSet = null!;
    private ImmutableHashSet<int> _immutableHashSet = null!;
    private FrozenSet<int> _frozenSet = null!;
    
    private int _middle;
    
    [Params(1000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _random = new Random(420);
        _list = Enumerable.Range(0, Size).Select(_ => _random.Next()).ToList();
        
        _middle = _list[Size / 2];

        _hashSet = _list.ToHashSet();
        _immutableHashSet = _list.ToImmutableHashSet();
        _frozenSet = _list.ToFrozenSet();
    }
    
    // Init
    [Benchmark]
    public HashSet<int> HashSet_Init() 
        => _list.ToHashSet();
    
    [Benchmark]
    public ImmutableHashSet<int> ImmutableHashSet_Init() 
        => _list.ToImmutableHashSet();
    
    [Benchmark]
    public FrozenSet<int> FrozenSet_Init() 
        => _list.ToFrozenSet();

    // Contains
    [Benchmark]
    public bool HashSet_Contains() 
        => _hashSet.Contains(_middle);
    
    [Benchmark]
    public bool ImmutableHashSet_Contains() 
        => _immutableHashSet.Contains(_middle);
    
    [Benchmark]
    public bool FrozenSet_Contains() 
        => _frozenSet.Contains(_middle);
}
