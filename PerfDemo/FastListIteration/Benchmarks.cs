using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace FastListIteration;

[MemoryDiagnoser(false)]
public class Benchmarks
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


/* Results:
 
| Method          | Size    | Mean          | Error      | StdDev     | Allocated |
|---------------- |-------- |--------------:|-----------:|-----------:|----------:|
| For             | 100     |      38.63 ns |   0.323 ns |   0.269 ns |         - |
| Foreach         | 100     |      33.11 ns |   0.547 ns |   0.485 ns |         - |
| For_Span        | 100     |      25.65 ns |   0.158 ns |   0.148 ns |         - |
| Unsafe_For_Span | 100     |      25.52 ns |   0.062 ns |   0.058 ns |         - |
| For             | 100000  |  28,293.78 ns |  49.450 ns |  46.255 ns |         - |
| Foreach         | 100000  |  19,004.94 ns |  29.836 ns |  27.909 ns |         - |
| For_Span        | 100000  |  18,940.81 ns |  64.291 ns |  60.138 ns |         - |
| Unsafe_For_Span | 100000  |  18,896.26 ns |  99.547 ns |  88.246 ns |         - |
| For             | 1000000 | 283,937.92 ns | 550.602 ns | 515.033 ns |         - |
| Foreach         | 1000000 | 191,470.79 ns | 497.762 ns | 465.607 ns |         - |
| For_Span        | 1000000 | 188,751.36 ns | 313.208 ns | 292.975 ns |         - |
| Unsafe_For_Span | 1000000 | 188,927.59 ns | 615.305 ns | 575.557 ns |         - |

 */
