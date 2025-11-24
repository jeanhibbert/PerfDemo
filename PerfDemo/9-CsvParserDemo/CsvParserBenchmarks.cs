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

        stopwatch.Stop();
        Console.WriteLine($"Duration:  {stopwatch.Elapsed.TotalSeconds} sec");
        Console.WriteLine($"Allocated: {(GC.GetTotalAllocatedBytes() - memorySize) / 1024 / 1024} mb");
        Console.ReadKey();
    }

    public static void RunParserForever()
    {
        while (true)
        {
            CsvParser.ParseCsv1();
            // if esc key is pressed, break
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                break;
            }
        }
    }

    [Benchmark]
    public void CsvParser1() => CsvParser.ParseCsv1();

    [Benchmark]
    public void CsvParser2() => CsvParser.ParseCsv2();

    [Benchmark]
    public void CsvParser3() => CsvParser.ParseCsv3();

    [Benchmark]
    public void CsvParser4() => CsvParser.ParseCsv4();
}

/* 
 | Method     | Mean      | Error     | StdDev    | Gen0      | Gen1      | Gen2      | Allocated |
|----------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|
| CsvParser1 | 15.447 ms | 0.1579 ms | 0.1477 ms | 1984.3750 | 1984.3750 | 1031.2500 |  18.16 MB |
| CsvParser2 | 14.938 ms | 0.2643 ms | 0.2473 ms | 1812.5000 | 1796.8750 |  984.3750 |  14.33 MB |
| CsvParser3 |  3.755 ms | 0.0734 ms | 0.1077 ms | 1324.2188 | 1292.9688 |  996.0938 |   9.19 MB |
| CsvParser4 |  5.155 ms | 0.1730 ms | 0.5046 ms | 1320.3125 | 1312.5000 |  992.1875 |    8.3 MB |
 */