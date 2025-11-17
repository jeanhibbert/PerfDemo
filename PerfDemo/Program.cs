using BenchmarkDotNet.Running;
using PerfDemo;
using PerfDemo.Examples;
using PerfDemo.Generics.Boxing;
using PerfDemo.Generics.CallpathElimination;
using PerfDemo.Generics.CallpathFactory;
using PerfDemo.Generics.GenericList;

// Uncomment the line below to run benchmarks
// BenchmarkMenu.Run();

// Run struct vs class benchmark
//BenchmarkRunner.Run<StructVsClassBenchmark>();

//BenchmarkRunner.Run<ParserBenchmarks>();

//BoxingRunner.Run();

//ListRunner.Run();

//GenericListRunner.Run();

//CallpathRunner.Run();

CallpathFactoryRunner.Run();