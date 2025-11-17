using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ZLinq;

public class LinqBMRunner
{
    public static void Run()
    {
        var summary = BenchmarkDotNet.Running.BenchmarkRunner.Run<BM>();
    }
}

[ShortRunJob]
[MemoryDiagnoser(false), HideColumns("StdDev", "RatioSD", "Alloc Ratio")]
public class BM {

    private readonly int[] _numbers = Enumerable.Range(1, 1000).ToArray();

    [Benchmark]
    public int Linq() => _numbers.Where(static n => n % 2 == 0).Sum();

    [Benchmark(Baseline = true)]
    public int ZLinq() => _numbers.AsValueEnumerable().Where(static n => n % 2 == 0).Sum();
}