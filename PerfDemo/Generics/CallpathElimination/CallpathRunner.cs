using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace PerfDemo.Generics.CallpathElimination;
internal class CallpathRunner
{
    public static void Run()
    {
        var summary = BenchmarkRunner.Run<BenchElimination>();
    }
}

[MemoryDiagnoser]
public class BenchElimination
{
    [Benchmark]
    public int ObjectIs() => ObjectService.Is(5);

    [Benchmark]
    public int ObjectGetType() => ObjectService.GetType(5);

    [Benchmark]
    public int GenericIs() => GenericService.Is(5);

    [Benchmark]
    public int GenericTypeOf() => GenericService.TypeOf(5);

    [Benchmark]
    public int GenericGetType() => GenericService.GetType(5);

}

/*
 | Method         | Mean      | Error     | StdDev    | Median    | Gen0   | Allocated |
|--------------- |----------:|----------:|----------:|----------:|-------:|----------:|
| ObjectIs       | 0.0141 ns | 0.0073 ns | 0.0069 ns | 0.0136 ns |      - |         - |
| ObjectGetType  | 1.5006 ns | 0.0405 ns | 0.0359 ns | 1.5128 ns | 0.0019 |      24 B |
| GenericIs      | 0.0115 ns | 0.0139 ns | 0.0116 ns | 0.0058 ns |      - |         - |
| GenericTypeOf  | 0.0188 ns | 0.0072 ns | 0.0060 ns | 0.0217 ns |      - |         - |
| GenericGetType | 0.0060 ns | 0.0111 ns | 0.0114 ns | 0.0000 ns |      - |         - |
 */