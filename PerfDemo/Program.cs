using BenchmarkDotNet.Running;
using PerfDemo;

// Uncomment the line below to run benchmarks
// BenchmarkMenu.Run();

// Run struct vs class benchmark
BenchmarkRunner.Run<StructVsClassBenchmark>();