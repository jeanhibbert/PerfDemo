using BenchmarkDotNet.Running;
using PerfDemo;
using PerfDemo.Examples;
using PerfDemo.Generics.Boxing;

// Uncomment the line below to run benchmarks
// BenchmarkMenu.Run();

// Run struct vs class benchmark
//BenchmarkRunner.Run<StructVsClassBenchmark>();

//BenchmarkRunner.Run<ParserBenchmarks>();

BoxingRunner.Run();