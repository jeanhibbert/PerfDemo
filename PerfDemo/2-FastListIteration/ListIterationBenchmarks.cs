using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace FastListIteration;

[SimpleJob]
[MemoryDiagnoser(false)]
public class ListIterationBenchmarks
{
    private static readonly Random Rng = new(80085);

    [Params(100, 100_000, 1_000_000)] public int Size { get; set; }

    private List<int> _items;

    [GlobalSetup]
    public void Setup()
    {
        _items = Enumerable.Range(1, Size).Select(x => Rng.Next()).ToList();
    }
    
    [Benchmark]
    public void For()
    {
        for (var i = 0; i < _items.Count; i++)
        {
            var item = _items[i];
        }
    }
    
    [Benchmark]
    public void Foreach()
    {
        foreach (var item in _items)
        {
        }
    }
    
    [Benchmark]
    public void For_Span()
    {
        var asSpan = CollectionsMarshal.AsSpan(_items);
        for (var i = 0; i < asSpan.Length; i++)
        {
            var item = asSpan[i];
        }
    }
    
    [Benchmark]
    public void Unsafe_For_Span()
    {
        var asSpan = CollectionsMarshal.AsSpan(_items);
        ref var searchSpace = ref MemoryMarshal.GetReference(asSpan);
        for (var i = 0; i < asSpan.Length; i++)
        {
            var item = Unsafe.Add(ref searchSpace, i);
        }
    }
}

/* 
 
Net8.0
| Method          | Size    | Mean          | Error      | StdDev     | Allocated |
|---------------- |-------- |--------------:|-----------:|-----------:|----------:|
| For             | 100     |      37.66 ns |   0.116 ns |   0.103 ns |         - |
| Foreach         | 100     |      31.95 ns |   0.168 ns |   0.157 ns |         - |
| For_Span        | 100     |      25.30 ns |   0.194 ns |   0.172 ns |         - |
| Unsafe_For_Span | 100     |      25.26 ns |   0.075 ns |   0.071 ns |         - |
| For             | 100000  |  27,988.90 ns |  64.884 ns |  54.181 ns |         - |
| Foreach         | 100000  |  18,858.79 ns | 105.568 ns |  98.748 ns |         - |
| For_Span        | 100000  |  18,675.55 ns |  32.246 ns |  26.927 ns |         - |
| Unsafe_For_Span | 100000  |  18,721.54 ns |  39.543 ns |  33.020 ns |         - |
| For             | 1000000 | 279,850.41 ns | 778.911 ns | 690.484 ns |         - |
| Foreach         | 1000000 | 188,007.36 ns | 243.008 ns | 215.420 ns |         - |
| For_Span        | 1000000 | 187,331.49 ns | 323.151 ns | 269.846 ns |         - |
| Unsafe_For_Span | 1000000 | 186,661.09 ns | 196.650 ns | 174.325 ns |         - |

Net9.0
| Method          | Size    | Mean          | Error        | StdDev       | Allocated |
|---------------- |-------- |--------------:|-------------:|-------------:|----------:|
| For             | 100     |      38.24 ns |     0.100 ns |     0.089 ns |         - |
| Foreach         | 100     |      29.01 ns |     0.106 ns |     0.089 ns |         - |
| For_Span        | 100     |      26.44 ns |     0.130 ns |     0.121 ns |         - |
| Unsafe_For_Span | 100     |      26.45 ns |     0.045 ns |     0.040 ns |         - |
| For             | 100000  |  28,175.30 ns |   133.880 ns |   125.232 ns |         - |
| Foreach         | 100000  |  19,217.39 ns |    97.537 ns |    81.448 ns |         - |
| For_Span        | 100000  |  18,686.39 ns |    25.591 ns |    21.370 ns |         - |
| Unsafe_For_Span | 100000  |  18,718.48 ns |    53.303 ns |    49.860 ns |         - |
| For             | 1000000 | 279,979.04 ns |   385.080 ns |   321.559 ns |         - |
| Foreach         | 1000000 | 189,960.51 ns | 1,888.449 ns | 1,766.457 ns |         - |
| For_Span        | 1000000 | 187,478.48 ns |   518.024 ns |   484.560 ns |         - |
| Unsafe_For_Span | 1000000 | 186,405.46 ns |   191.092 ns |   159.570 ns |         - |

*/
