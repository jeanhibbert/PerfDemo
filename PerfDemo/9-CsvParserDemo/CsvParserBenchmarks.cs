using BenchmarkDotNet.Attributes;

namespace PerfDemo._9_CsvParserDemo;

[MemoryDiagnoser]
public class CsvParserBenchmarks
{
    public static void RunCsvParser()
    {
        Console.WriteLine("Warmup Started");

        for (int i = 0; i < 32; i++) // Tiered compilation kicks in after 30 - recompiles another version based on profile gathered output
        {
            CsvParser.ParseCsv1();
            //CsvParser.ParseCsv2();
            //CsvParser.ParseCsv3();
            //CsvParser.ParseCsv4();
            //CsvParser.ParseCsv5();
        }

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Console.WriteLine("Warmup Done");

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var memorySize = GC.GetTotalAllocatedBytes();

        CsvParser.ParseCsv1();
        //CsvParser.ParseCsv2();
        //CsvParser.ParseCsv3();
        //CsvParser.ParseCsv4();
        //CsvParser.ParseCsv5();
        
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

    [Benchmark]
    public void CsvParser5() => CsvParser.ParseCsv5();
}

/* 
net9.0
 | Method     | Mean      | Error    | StdDev   | Gen0       | Gen1       | Gen2      | Allocated |
|----------- |----------:|---------:|---------:|-----------:|-----------:|----------:|----------:|
| CsvParser1 | 179.57 ms | 3.481 ms | 5.522 ms | 33333.3333 | 14333.3333 | 3333.3333 | 426.84 MB |
| CsvParser2 | 158.70 ms | 3.088 ms | 3.433 ms | 26750.0000 |  7750.0000 | 1750.0000 | 316.52 MB |
| CsvParser3 |  84.81 ms | 1.490 ms | 1.464 ms | 25333.3333 |          - |         - |  304.6 MB |
| CsvParser4 |  84.72 ms | 1.559 ms | 1.459 ms | 10333.3333 |   500.0000 |  500.0000 |  145.3 MB |
| CsvParser5 |  79.69 ms | 1.464 ms | 1.370 ms |  9857.1429 |   142.8571 |  142.8571 | 122.34 MB |
 */