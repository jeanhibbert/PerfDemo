using System.Collections.Frozen;
using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;

namespace FrozenCollections;

[SimpleJob]
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


/*
 | Method                    | Size | Mean          | Error       | StdDev      | Gen0   | Gen1   | Allocated |
|-------------------------- |----- |--------------:|------------:|------------:|-------:|-------:|----------:|
| HashSet_Init              | 1000 |  4,273.862 ns |  83.0363 ns | 107.9707 ns | 1.4191 | 0.0839 |   17808 B |
| ImmutableHashSet_Init     | 1000 | 88,718.709 ns | 897.6200 ns | 839.6343 ns | 4.3945 | 0.7324 |   56088 B |
| FrozenSet_Init            | 1000 | 10,890.501 ns | 187.4583 ns | 175.3486 ns | 3.5248 | 0.3510 |   44320 B |
| HashSet_Contains          | 1000 |      1.346 ns |   0.0543 ns |   0.0667 ns |      - |      - |         - |
| ImmutableHashSet_Contains | 1000 |      5.897 ns |   0.1438 ns |   0.1345 ns |      - |      - |         - |
| FrozenSet_Contains        | 1000 |      2.024 ns |   0.0651 ns |   0.0639 ns |      - |      - |         - |
 */