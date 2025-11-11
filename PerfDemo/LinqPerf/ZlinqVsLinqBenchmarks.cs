using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace PerfDemo.LinqPerf;

[SimpleJob]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ZlinqVsLinqBenchmarks
{

}
