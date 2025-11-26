using BenchmarkDotNet.Running;
using CastingObjects;
using FastListIteration;
using FrozenCollections;
using InAndOutExamples;
using PerfDemo;
using PerfDemo._0_ResultPattern;
using PerfDemo._5_RefStruct;
using PerfDemo._6_Casting;
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
//BenchmarkRunner.Run<StructVsClassParamBenchmark>();

//BenchmarkRunner.Run<CastingBenchmarks>(); // 6
//BenchmarkRunner.Run<LinqCastingBenchmarks>();

//BenchmarkRunner.Run<BenchObjects>(); // 7
//BenchmarkRunner.Run<BenchStructs>();

//BenchmarkRunner.Run<BenchElimination>();
//BenchmarkRunner.Run<BenchEliminationFactory>();


//BenchmarkRunner.Run<SpanSplitBenchmarks>(); // 8
//BenchmarkRunner.Run<SubstringSpanBenchmarks>();

//ParseWithSpanExample.Run();                         //--- DEMO

//BenchmarkRunner.Run<CreateStringSpanBenchmarks>();

//CsvParserBenchmarks.RunCsvParser(); // 9         //-- DEMO
//CsvParserBenchmarks.RunParserForever(); // Use for demoing visual studio profiler only!!

BenchmarkRunner.Run<CsvParserBenchmarks>();        //-- DEMO

//////////////////////////////////////////////
//BenchmarkRunner.Run<ParserBenchmarks>();



///////////////////////////////////////////// - if there is time
//BenchmarkRunner.Run<InAndOutBenchmarks>();
