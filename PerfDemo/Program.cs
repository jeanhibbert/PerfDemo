using BenchmarkDotNet.Running;
using CastingObjects;
using FastListIteration;
using FrozenCollections;
using PerfDemo;
using PerfDemo._0_ResultPattern;
using PerfDemo._8_Span;
using PerfDemo._9_CsvParserDemo;
using PerfDemo.Examples;
using PerfDemo.Generics.Boxing;
using PerfDemo.Generics.CallpathElimination;
using PerfDemo.Generics.CallpathFactory;
using PerfDemo.Generics.GenericList;

using PerfDemo.ZLinqBenchmark;

//TODO: Result Pattern benchmarks
//BenchmarkRunner.Run<ResultPatternBenchmarks>(); // 0

//BenchmarkRunner.Run<PerfDemo._1_DictionaryAddUpdate.DictionaryBenchmarks>(); // 1

//BenchmarkRunner.Run<ListIterationBenchmarks>(); // 2

//BenchmarkRunner.Run<ZLinqBenchmarks>(); // 3

//BenchmarkRunner.Run<FrozenDictionaryBenchmarks>(); // 4
//BenchmarkRunner.Run<SetBenchmarks>();

//BenchmarkRunner.Run<StructVsClassBenchmark>(); // 5

//BenchmarkRunner.Run<CastingBenchmarks>(); // 6

//BenchmarkRunner.Run<BenchObjects>(); // 7
//BenchmarkRunner.Run<BenchStructs>();

//BenchmarkRunner.Run<BenchElimination>();
//BenchmarkRunner.Run<BenchEliminationFactory>();


BenchmarkRunner.Run<SpanSplitBenchmarks>(); // 8
BenchmarkRunner.Run<SubstringSpanBenchmarks>();
BenchmarkRunner.Run<CreateStringSpanBenchmarks>();
   

//CsvParserBenchmarks.RunLightBenchmark(); // 9


//////////////////////////////////////////////
//BenchmarkRunner.Run<ParserBenchmarks>();



