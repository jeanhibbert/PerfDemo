using BenchmarkDotNet.Attributes;

namespace PerfDemo._1_DictionaryAddUpdate;

[SimpleJob]
[MemoryDiagnoser]
public class DictionaryBenchmarks
{
    private Dictionary<int, string> _dictionary = null!;
    private int[] _keys = null!;
    private Random _rand = null!;

    [Params(1000, 100_000)]
    public int Size { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _rand = new Random(42);
        _dictionary = new Dictionary<int, string>(Size);
        _keys = new int[Size];
        for (int i = 0; i < Size; i++)
        {
            _dictionary.Add(i, "value_" + i);
            _keys[i] = i;
        }
    }

    [Benchmark]
    public void TryUpdate() => 
        _dictionary.TryUpdate(_rand.Next(Size), "updated_value");

    [Benchmark]
    public void TryUpdate_Optimised() => 
        _dictionary.TryUpdateFast(_rand.Next(Size), "updated_value");

    [Benchmark]
    public void GetOrAdd() =>
        _dictionary.GetOrAdd(_rand.Next(Size), "updated_value");

    [Benchmark]
    public void GetOrAdd_Optimised() =>
        _dictionary.GetOrAddFast(_rand.Next(Size), "updated_value");
}

/*
    | Method              | Size   | Mean      | Error     | StdDev    | Median    | Allocated |
    |-------------------- |------- |----------:|----------:|----------:|----------:|----------:|
    | TryUpdate           | 1000   | 14.302 ns | 1.6339 ns | 4.8177 ns | 18.096 ns |         - |
    | TryUpdate_Optimised | 1000   |  4.699 ns | 0.1205 ns | 0.1238 ns |  4.755 ns |         - |
    | GetOrAdd            | 1000   |  4.567 ns | 0.1029 ns | 0.0963 ns |  4.554 ns |         - |
    | GetOrAdd_Optimised  | 1000   |  4.670 ns | 0.1078 ns | 0.1008 ns |  4.680 ns |         - |
    | TryUpdate           | 100000 | 12.747 ns | 0.1774 ns | 0.1659 ns | 12.762 ns |         - |
    | TryUpdate_Optimised | 100000 |  7.396 ns | 0.1562 ns | 0.1462 ns |  7.393 ns |         - |
    | GetOrAdd            | 100000 |  7.088 ns | 0.1034 ns | 0.0967 ns |  7.077 ns |         - |
    | GetOrAdd_Optimised  | 100000 |  7.475 ns | 0.0673 ns | 0.0630 ns |  7.478 ns |         - |
*/