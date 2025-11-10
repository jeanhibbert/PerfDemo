using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace PerfDemo;

[SimpleJob]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class LinqVsLoopBenchmarks
{
    private List<int> _numbers = null!;
    private const int Count = 1000;

    [GlobalSetup]
    public void Setup()
    {
        _numbers = Enumerable.Range(1, Count).ToList();
    }

    // Filtering benchmarks
    [Benchmark]
    public List<int> FilterWithLinqWhere()
    {
        return _numbers.Where(x => x % 2 == 0).ToList();
    }

    [Benchmark]
    public List<int> FilterWithForLoop()
    {
        var result = new List<int>();
        for (int i = 0; i < _numbers.Count; i++)
        {
            if (_numbers[i] % 2 == 0)
            {
                result.Add(_numbers[i]);
            }
        }
        return result;
    }

    [Benchmark]
    public List<int> FilterWithForEach()
    {
        var result = new List<int>();
        foreach (var number in _numbers)
        {
            if (number % 2 == 0)
            {
                result.Add(number);
            }
        }
        return result;
    }

    // Transformation benchmarks
    [Benchmark]
    public List<int> TransformWithLinqSelect()
    {
        return _numbers.Select(x => x * 2).ToList();
    }

    [Benchmark]
    public List<int> TransformWithForLoop()
    {
        var result = new List<int>(_numbers.Count);
        for (int i = 0; i < _numbers.Count; i++)
        {
            result.Add(_numbers[i] * 2);
        }
        return result;
    }

    // Aggregation benchmarks
    [Benchmark]
    public int SumWithLinq()
    {
        return _numbers.Sum();
    }

    [Benchmark]
    public int SumWithForLoop()
    {
        int sum = 0;
        for (int i = 0; i < _numbers.Count; i++)
        {
            sum += _numbers[i];
        }
        return sum;
    }

    [Benchmark]
    public int SumWithForEach()
    {
        int sum = 0;
        foreach (var number in _numbers)
        {
            sum += number;
        }
        return sum;
    }

    // Any/All benchmarks
    [Benchmark]
    public bool AnyWithLinq()
    {
        return _numbers.Any(x => x > 500);
    }

    [Benchmark]
    public bool AnyWithForLoop()
    {
        for (int i = 0; i < _numbers.Count; i++)
        {
            if (_numbers[i] > 500)
            {
                return true;
            }
        }
        return false;
    }

    // First/FirstOrDefault benchmarks
    [Benchmark]
    public int FirstWithLinq()
    {
        return _numbers.First(x => x > 500);
    }

    [Benchmark]
    public int FirstWithForLoop()
    {
        for (int i = 0; i < _numbers.Count; i++)
        {
            if (_numbers[i] > 500)
            {
                return _numbers[i];
            }
        }
        return default;
    }

    // Complex query benchmarks
    [Benchmark]
    public List<int> ComplexLinqQuery()
    {
        return _numbers
        .Where(x => x % 2 == 0)
        .Select(x => x * 2)
         .Where(x => x > 100)
   .ToList();
    }

    [Benchmark]
    public List<int> ComplexForLoop()
    {
        var result = new List<int>();
        for (int i = 0; i < _numbers.Count; i++)
        {
            var number = _numbers[i];
            if (number % 2 == 0)
            {
                var transformed = number * 2;
                if (transformed > 100)
                {
                    result.Add(transformed);
                }
            }
        }
        return result;
    }

    // Count benchmarks
    [Benchmark]
    public int CountWithLinq()
    {
        return _numbers.Count(x => x % 2 == 0);
    }

    [Benchmark]
    public int CountWithForLoop()
    {
        int count = 0;
        for (int i = 0; i < _numbers.Count; i++)
        {
            if (_numbers[i] % 2 == 0)
            {
                count++;
            }
        }
        return count;
    }
}
