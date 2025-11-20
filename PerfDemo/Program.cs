using BenchmarkDotNet.Running;
using CastingObjects;
using FastListIteration;
using FrozenCollections;
using PerfDemo;
using PerfDemo.Examples;
using PerfDemo.Generics.Boxing;
using PerfDemo.Generics.CallpathElimination;
using PerfDemo.Generics.CallpathFactory;
using PerfDemo.Generics.GenericList;

using PerfDemo.ZLinqBenchmark;

//TODO: Result Pattern benchmarks

//BenchmarkRunner.Run<PerfDemo._1_DictionaryAddUpdate.DictionaryBenchmarks>();

//BenchmarkRunner.Run<ListIterationBenchmarks>();

BenchmarkRunner.Run<ZLinqBenchmarks>();

//BenchmarkRunner.Run<FrozenDictionaryBenchmarks>();
//BenchmarkRunner.Run<SetBenchmarks>();

//BenchmarkRunner.Run<BenchObjects>();
//BenchmarkRunner.Run<BenchStructs>();

//BenchmarkRunner.Run<CastingBenchmarks>();

// Uncomment the line below to run benchmarks
// BenchmarkMenu.Run();

// Run struct vs class benchmark
//BenchmarkRunner.Run<StructVsClassBenchmark>();

//BenchmarkRunner.Run<ParserBenchmarks>();

//BoxingRunner.Run();

//ListRunner.Run();

//GenericListRunner.Run();

//CallpathRunner.Run();

//CallpathFactoryRunner.Run();