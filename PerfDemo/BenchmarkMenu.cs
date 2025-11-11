using BenchmarkDotNet.Running;
using PerfDemo.LinqPerf;

namespace PerfDemo;

public static class BenchmarkMenu
{
    public static void Run()
    {
        Console.WriteLine("Performance Benchmarks Demo");
        Console.WriteLine("===========================");
        Console.WriteLine();
        Console.WriteLine("Select a benchmark to run:");
        Console.WriteLine("1. Dictionary Benchmarks");
        Console.WriteLine("2. String Benchmarks");
        Console.WriteLine("3. LINQ vs Loop Benchmarks");
        Console.WriteLine("4. Span vs Array Benchmarks");
        Console.WriteLine("5. Frozen Collection Benchmarks");
        Console.WriteLine("6. Collection Types Benchmarks");
        Console.WriteLine("7. Struct vs Class Benchmarks");
        Console.WriteLine("8. Zlinq vs LINQ Benchmarks");
        Console.WriteLine("9. Run All Benchmarks");
        Console.WriteLine("0. Exit");
        Console.WriteLine();
        Console.Write("Enter your choice (or press Enter to run all): ");

        var input = Console.ReadLine();

        switch (input)
        {
            case "1":
                BenchmarkRunner.Run<DictionaryBenchmarks>();
                break;
            case "2":
                BenchmarkRunner.Run<StringBenchmarks>();
                break;
            case "3":
                BenchmarkRunner.Run<LinqVsLoopBenchmarks>();
                break;
            case "4":
                BenchmarkRunner.Run<SpanVsArrayBenchmarks>();
                break;
            case "5":
                BenchmarkRunner.Run<FrozenCollectionBenchmarks>();
                break;
            case "6":
                BenchmarkRunner.Run<CollectionTypesBenchmarks>();
                break;
            case "7":
                BenchmarkRunner.Run<StructVsClassBenchmark>();
                break;
            case "8":
                BenchmarkRunner.Run<ZlinqVsLinqBenchmarks>();
                break;
            case "9":
            case "":
                Console.WriteLine("Running all benchmarks...");
                RunAllBenchmarks();
                break;
            case "0":
                Console.WriteLine("Exiting...");
                break;
            default:
                Console.WriteLine("Invalid choice. Running all benchmarks...");
                RunAllBenchmarks();
                break;
        }
    }

    private static void RunAllBenchmarks()
    {
        BenchmarkRunner.Run<DictionaryBenchmarks>();
        BenchmarkRunner.Run<StringBenchmarks>();
        BenchmarkRunner.Run<LinqVsLoopBenchmarks>();
        BenchmarkRunner.Run<SpanVsArrayBenchmarks>();
        BenchmarkRunner.Run<FrozenCollectionBenchmarks>();
        BenchmarkRunner.Run<CollectionTypesBenchmarks>();
        BenchmarkRunner.Run<StructVsClassBenchmark>();
        BenchmarkRunner.Run<ZlinqVsLinqBenchmarks>();
    }
}
