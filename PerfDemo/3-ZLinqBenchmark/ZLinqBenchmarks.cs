using BenchmarkDotNet.Attributes;
using ZLinq;

namespace PerfDemo.ZLinqBenchmark;

[MemoryDiagnoser]
public class ZLinqBenchmarks
{
    private static Random Random = new Random(420);

    public static readonly int[] Sources = Enumerable.Range(0, 10000)
        .Select(i => Random.Next(0, 10000))
        .ToArray();

    
    [Benchmark]
    public long Linq_Where_Select_Sum()
    {
        return Sources.Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum(x => (long)x);
    }
    
    [Benchmark]
    public long ZLinq_Where_Select_Sum()
    {
        return Sources.AsValueEnumerable()
            .Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum(x => (long)x);
    }

    private readonly int[] _numbers = Enumerable.Range(1, 1000).ToArray();

    [Benchmark]
    public int Linq_Where() => _numbers.Where(static n => n % 2 == 0).Sum();

    [Benchmark]
    public int ZLinq_Where() => _numbers.AsValueEnumerable().Where(static n => n % 2 == 0).Sum();
}

/*
 wow!!!
net9.0
| Method                 | Mean        | Error     | StdDev    | Gen0   | Allocated |
|----------------------- |------------:|----------:|----------:|-------:|----------:|
| Linq_Where_Select_Sum  | 20,642.6 ns | 295.21 ns | 276.14 ns |      - |     104 B |
| ZLinq_Where_Select_Sum | 18,904.4 ns |  71.22 ns |  63.13 ns |      - |         - |
| Linq_Where             |    644.3 ns |   8.79 ns |   7.79 ns | 0.0038 |      48 B |
| ZLinq_Where            |    449.2 ns |   3.00 ns |   2.81 ns |      - |         - |
 
 */