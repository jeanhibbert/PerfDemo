using BenchmarkDotNet.Attributes;
using ZLinq;

namespace PerfDemo.ZLinqBenchmark;

[MemoryDiagnoser]
internal class Benchmarks
{
    private static Random Random = new Random(420);

    public static readonly int[] Sources = Enumerable.Range(0, 10000)
        .Select(i => Random.Next(0, 10000))
        .ToArray();

    
    [Benchmark(Baseline = true)]
    public long NormalLinq()
    {
        return Sources.Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum(x => (long)x);
    }
    
    [Benchmark]
    public long ZLinq_Sum()
    {
        return Sources.AsValueEnumerable()
            .Where(x => x % 2 == 0)
            .Select(x => x * 2)
            .Sum(x => (long)x);
    }
}
