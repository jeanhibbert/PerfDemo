using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace PerfDemo;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class SpanVsArrayBenchmarks
{
    private int[] _sourceArray = null!;
    private const int Size = 1000;

    [GlobalSetup]
    public void Setup()
    {
        _sourceArray = Enumerable.Range(0, Size).ToArray();
    }

    // Array copying benchmarks
    [Benchmark]
    public int[] CopyWithArrayCopy()
    {
        var destination = new int[Size];
        Array.Copy(_sourceArray, destination, Size);
        return destination;
    }

    [Benchmark]
    public int[] CopyWithSpan()
    {
        var destination = new int[Size];
        _sourceArray.AsSpan().CopyTo(destination);
        return destination;
    }

    [Benchmark]
    public int[] CopyWithForLoop()
    {
        var destination = new int[Size];
        for (int i = 0; i < Size; i++)
        {
            destination[i] = _sourceArray[i];
        }
        return destination;
    }

    // Slicing benchmarks
    [Benchmark]
    public int[] SliceWithArraySegment()
    {
        var segment = new ArraySegment<int>(_sourceArray, 100, 200);
        return segment.ToArray();
    }

    [Benchmark]
    public int[] SliceWithSpan()
    {
        return _sourceArray.AsSpan().Slice(100, 200).ToArray();
    }

    [Benchmark]
    public int[] SliceWithLinq()
    {
        return _sourceArray.Skip(100).Take(200).ToArray();
    }

    // Reverse benchmarks
    [Benchmark]
    public void ReverseWithArray()
    {
        var copy = new int[Size];
        Array.Copy(_sourceArray, copy, Size);
        Array.Reverse(copy);
    }

    [Benchmark]
    public void ReverseWithSpan()
    {
        Span<int> span = stackalloc int[Size];
        _sourceArray.AsSpan().CopyTo(span);
        span.Reverse();
    }

    // Sum benchmarks
    [Benchmark]
    public int SumWithArray()
    {
        int sum = 0;
        for (int i = 0; i < _sourceArray.Length; i++)
        {
            sum += _sourceArray[i];
        }
        return sum;
    }

    [Benchmark]
    public int SumWithSpan()
    {
        int sum = 0;
        ReadOnlySpan<int> span = _sourceArray;
        for (int i = 0; i < span.Length; i++)
        {
            sum += span[i];
        }
        return sum;
    }

    // String parsing with Span
    [Benchmark]
    public int ParseWithSubstring()
    {
        string text = "12345";
        string sub = text.Substring(1, 3);
        return int.Parse(sub);
    }

    [Benchmark]
    public int ParseWithSpan()
    {
        string text = "12345";
        ReadOnlySpan<char> span = text.AsSpan(1, 3);
        return int.Parse(span);
    }

    // Fill benchmarks
    [Benchmark]
    public int[] FillWithForLoop()
    {
        var array = new int[Size];
        for (int i = 0; i < Size; i++)
        {
            array[i] = 42;
        }
        return array;
    }

    [Benchmark]
    public int[] FillWithSpan()
    {
        var array = new int[Size];
        array.AsSpan().Fill(42);
        return array;
    }

    // Search benchmarks
    [Benchmark]
    public int IndexOfWithArray()
    {
        for (int i = 0; i < _sourceArray.Length; i++)
        {
            if (_sourceArray[i] == 500)
            {
                return i;
            }
        }
        return -1;
    }

    [Benchmark]
    public int IndexOfWithSpan()
    {
        return _sourceArray.AsSpan().IndexOf(500);
    }

    [Benchmark]
    public int IndexOfWithArrayIndexOf()
    {
        return Array.IndexOf(_sourceArray, 500);
    }

    // Stack allocation benchmark (small size)
    [Benchmark]
    public int SmallArrayOnHeap()
    {
        var array = new int[10];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = i * 2;
        }
        return array.Sum();
    }

    [Benchmark]
    public int SmallSpanOnStack()
    {
        Span<int> span = stackalloc int[10];
        for (int i = 0; i < span.Length; i++)
        {
            span[i] = i * 2;
        }

        int sum = 0;
        for (int i = 0; i < span.Length; i++)
        {
            sum += span[i];
        }
        return sum;
    }

    // Memory<T> benchmarks
    [Benchmark]
    public int ProcessWithMemory()
    {
        Memory<int> memory = _sourceArray.AsMemory();
        return ProcessMemory(memory.Slice(0, 100));
    }

    [Benchmark]
    public int ProcessWithSpan()
    {
        Span<int> span = _sourceArray.AsSpan();
        return ProcessSpan(span.Slice(0, 100));
    }

    private int ProcessMemory(Memory<int> memory)
    {
        int sum = 0;
        var span = memory.Span;
        for (int i = 0; i < span.Length; i++)
        {
            sum += span[i];
        }
        return sum;
    }

    private int ProcessSpan(Span<int> span)
    {
        int sum = 0;
        for (int i = 0; i < span.Length; i++)
        {
            sum += span[i];
        }
        return sum;
    }
}
