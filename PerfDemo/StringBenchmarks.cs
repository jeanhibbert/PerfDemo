using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Text;

namespace PerfDemo;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class StringBenchmarks
{
    private const int Iterations = 100;
    private readonly string[] _words = { "Hello", "World", "Performance", "Testing", "Benchmark" };

    [Benchmark]
    public string ConcatenationWithPlus()
    {
        string result = "";
        for (int i = 0; i < Iterations; i++)
        {
            result = result + _words[i % _words.Length];
        }
        return result;
    }

    [Benchmark]
    public string ConcatenationWithPlusEquals()
    {
        string result = "";
        for (int i = 0; i < Iterations; i++)
        {
            result += _words[i % _words.Length];
        }
        return result;
    }

    [Benchmark]
    public string StringBuilderDefault()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Iterations; i++)
        {
            sb.Append(_words[i % _words.Length]);
        }
        return sb.ToString();
    }

    [Benchmark]
    public string StringBuilderWithCapacity()
    {
        var sb = new StringBuilder(Iterations * 10);
        for (int i = 0; i < Iterations; i++)
        {
            sb.Append(_words[i % _words.Length]);
        }
        return sb.ToString();
    }

    [Benchmark]
    public string StringInterpolation()
    {
        string result = "";
        for (int i = 0; i < Iterations; i++)
        {
            result = $"{result}{_words[i % _words.Length]}";
        }
        return result;
    }

    [Benchmark]
    public string StringConcat()
    {
        string result = "";
        for (int i = 0; i < Iterations; i++)
        {
            result = string.Concat(result, _words[i % _words.Length]);
        }
        return result;
    }

    [Benchmark]
    public string StringJoin()
    {
        var items = new string[Iterations];
        for (int i = 0; i < Iterations; i++)
        {
            items[i] = _words[i % _words.Length];
        }
        return string.Join("", items);
    }

    [Benchmark]
    public string StringCreate()
    {
        var items = new string[Iterations];
        for (int i = 0; i < Iterations; i++)
        {
            items[i] = _words[i % _words.Length];
        }

        int totalLength = 0;
        foreach (var item in items)
        {
            totalLength += item.Length;
        }

        return string.Create(totalLength, items, (span, state) =>
        {
            int position = 0;
            foreach (var item in state)
            {
                item.AsSpan().CopyTo(span.Slice(position));
                position += item.Length;
            }
        });
    }
}
