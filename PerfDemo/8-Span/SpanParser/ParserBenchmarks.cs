using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Globalization;

namespace PerfDemo.Examples;

[SimpleJob]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ParserBenchmarks
{
    [Benchmark]
    public RigidBodyState TheSlowWay()
    {
        return RigidBodyState.Parse("1.23,-45.6,0,9.81,0,-2.45");
    }

    [Benchmark()]
    public RigidBodyState TheFastWay()
    {
        return RigidBodyState.ParseFast("1.23,-45.6,0,9.81,0,-2.45");
    }
}

/*
 
| Method             | Mean       | Error     | StdDev    | Rank | Gen0   | Allocated |
|------------------- |-----------:|----------:|----------:|-----:|-------:|----------:|
| TheFastWay         | 166.453 ns | 1.2720 ns | 1.1276 ns |    3 |      - |         - |
| TheSlowWay         | 199.125 ns | 3.3526 ns | 3.9910 ns |    4 | 0.0196 |     248 B |
 
 */