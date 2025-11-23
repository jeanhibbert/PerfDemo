using BenchmarkDotNet.Attributes;

namespace PerfDemo._9_CsvParserDemo;

[MemoryDiagnoser]
public class CsvParserBenchmarks
{
    public static void RunLightBenchmark()
    {
        Console.WriteLine("Warmup Started");

        for (int i = 0; i < 32; i++)
        {
            //CsvParser.ParseCsv1();
            //CsvParser.ParseCsv2();
            //CsvParser.ParseCsv3();
            CsvParser.ParseCsv4();

            //UseSpanAndMemoryPool();
            //UseSpanMemoryPoolAndStringPool();
        }

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Console.WriteLine("Warmup Done");

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var memorySize = GC.GetTotalAllocatedBytes();

        //CsvParser.ParseCsv1();
        //CsvParser.ParseCsv2();
        //CsvParser.ParseCsv3();
        CsvParser.ParseCsv4();


        //UseSpanAndMemoryPool();
        //UseSpanMemoryPoolAndStringPool();

        stopwatch.Stop();
        Console.WriteLine($"Duration:  {stopwatch.Elapsed.TotalSeconds} sec");
        Console.WriteLine($"Allocated: {(GC.GetTotalAllocatedBytes() - memorySize) / 1024 / 1024} mb");
        Console.ReadKey();
    }

    [Benchmark]
    public void CsvParser1() => CsvParser.ParseCsv1();
}
