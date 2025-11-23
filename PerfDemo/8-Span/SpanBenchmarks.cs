using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfDemo._8_Span;

[ShortRunJob]
[MemoryDiagnoser(false)]
[HideColumns("StdDev", "RatioSD", "Alloc Ratio")]
[ReturnValueValidator]
public class SubstringSpanBenchmarks
{
    private const string _text = "Content-Length: 132";

    [Benchmark]
    public int Substring() => int.Parse(_text.Substring(startIndex: 16));

    [Benchmark]
    public int AsSpanSlice() => int.Parse(_text.AsSpan().Slice(start: 16));

    [Benchmark(Baseline = true)]
    public int AsSpan() => int.Parse(_text.AsSpan(startIndex: 16));
}

[ShortRunJob]
[MemoryDiagnoser(false)]
[HideColumns("StdDev", "RatioSD", "Alloc Ratio")]
[ReturnValueValidator]
public class CreateStringSpanBenchmarks
{
    private string _text1 = "Hello";
    private string _text2 = "World";

    [Benchmark]
    public string String() => $"{_text1} {_text2}".ToUpper();

    [Benchmark]
    public string Span()
    {
        Span<char> destination = stackalloc char[_text1.Length + 1 + _text2.Length];
        _text1.AsSpan().ToUpper(destination, null);
        destination[_text1.Length] = ' ';
        _text2.AsSpan().ToUpper(destination.Slice(_text1.Length + 1, _text2.Length), null);
        return destination.ToString();
    }

    [Benchmark(Baseline = true)]
    public string StringCreate()
    {
        return string.Create(_text1.Length + 1 + _text2.Length, (_text1, _text2),
                             static (Span<char> destination, (string Text1, string Text2) state) => {

                                 state.Text1.AsSpan().ToUpper(destination, null);
                                 destination[state.Text1.Length] = ' ';
                                 state.Text2.AsSpan().ToUpper(destination.Slice(state.Text1.Length + 1, state.Text2.Length), null);
                             });
    }
}

[ShortRunJob]
[MemoryDiagnoser(false), HideColumns("StdDev", "RatioSD", "Alloc Ratio"), ReturnValueValidator]
public class SpanSplitBenchmarks
{

    private const string _text = "123,456,789";

    [Benchmark]
    public int StringSplit()
    {
        string[] a = _text.Split(',', StringSplitOptions.RemoveEmptyEntries);
        int t = 0;
        foreach (string word in a) t += int.Parse(word);
        return t;
    }


    [Benchmark(Baseline = true)]
    public int SpanSplit()
    {
        ReadOnlySpan<char> span = _text.AsSpan();
        int t = 0;
        foreach (Range range in span.Split(','))
        {
            if (range.Start.Value < range.End.Value) t += int.Parse(span[range]);
        }
        return t;
    }
}