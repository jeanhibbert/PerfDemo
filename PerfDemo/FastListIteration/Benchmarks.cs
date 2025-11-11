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
