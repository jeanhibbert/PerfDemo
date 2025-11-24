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
    public Point3D TheSlowWay()
    {
        return Point3D.Parse("(1.1, 2.2, 3.3)");
    }

    [Benchmark()]
    public Point3D TheFastWay()
    {
        return Point3D.ParseFast("(1.1, 2.2, 3.3)");
    }

    [Benchmark()]
    public DateTime DateTimeTheSlowWay()
    {
        return DateTime.Parse("2024-06-15T13:45:30");
    }
    
    [Benchmark()]
    public SystemDate DateTimeTheFastWay()
    {
        return SystemDate.ParseFast("2024-06-15T13:45:30");
    }
}

/*
 
| Method             | Mean       | Error     | StdDev    | Rank | Gen0   | Allocated |
|------------------- |-----------:|----------:|----------:|-----:|-------:|----------:|
| DateTimeTheFastWay |   7.185 ns | 0.0487 ns | 0.0456 ns |    1 |      - |         - |
| DateTimeTheSlowWay |  68.066 ns | 0.2288 ns | 0.1911 ns |    2 |      - |         - |
| TheFastWay         | 111.777 ns | 0.5048 ns | 0.4722 ns |    3 |      - |         - |
| TheSlowWay         | 137.743 ns | 2.7085 ns | 3.0105 ns |    4 | 0.0196 |     248 B |
 
 */