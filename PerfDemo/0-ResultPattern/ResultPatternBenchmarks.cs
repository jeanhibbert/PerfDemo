using BenchmarkDotNet.Attributes;

namespace PerfDemo._0_ResultPattern;

[MemoryDiagnoser]
public class ResultPatternBenchmarks
{
    [Benchmark]
    public void ValidateAge_WithResultPattern()
    {
        for (int i = 0; i < 10000; i++)
        {
            var result = Validator.ValidateAge(i % 2 == 0 ? i : 0);
            if (!result.IsSuccess)
            {
                // Handle error (no-op for benchmark)
            }
        }
    }

    [Benchmark]
    public void ValidateAge_WithException()
    {
        for (int i = 0; i < 10000; i++)
        {
            try
            {
                var isValid = Validator.ValidateAgeWithException(i % 2 == 0 ? i : 0);
            }
            catch (ArgumentException)
            {
                // Handle exception (no-op for benchmark)
            }
        }
    }
}


/*
 
| Method                        | Mean        | Error      | StdDev     | Gen0    | Allocated  |
|------------------------------ |------------:|-----------:|-----------:|--------:|-----------:|
| ValidateAge_WithResultPattern |    34.29 us |   0.636 us |   0.595 us | 44.6167 |  546.88 KB |
| ValidateAge_WithException     | 6,970.33 us | 111.519 us | 104.315 us | 85.9375 | 1093.97 KB |
 
 */