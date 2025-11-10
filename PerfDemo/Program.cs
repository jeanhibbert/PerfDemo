using BenchmarkDotNet.Running;
using PerfDemo;

// Uncomment the line below to run benchmarks
// BenchmarkMenu.Run();

BenchmarkRunner.Run<StructVsClassBenchmark>();