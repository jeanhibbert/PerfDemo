using BenchmarkDotNet.Attributes;

namespace PerfDemo.Examples;

[MemoryDiagnoser]
public class ParserBenchmarks
{
    private const string Input = "(1.1, 2.2, 3.3)";

    [Benchmark(Baseline = true)]
    public Point3D TheSlowWay()
    {
        return Point3D.Parse(Input);
    }

    [Benchmark()]
    public Point3D TheFastWay()
    {
        return Point3D.ParseFast(Input);
    }
}