using BenchmarkDotNet.Attributes;

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

/*
 
| Method      | Mean     | Error     | Ratio | Allocated |
|------------ |---------:|----------:|------:|----------:|
| Substring   | 6.550 ns | 2.6370 ns |  1.56 |      32 B |
| AsSpanSlice | 4.226 ns | 1.3996 ns |  1.01 |         - |
| AsSpan      | 4.202 ns | 0.7137 ns |  1.00 |         - |

 */


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

/*
 
| Method       | Mean     | Error     | Ratio | Allocated |
|------------- |---------:|----------:|------:|----------:|
| String       | 20.72 ns |  4.658 ns |  1.35 |      96 B |
| Span         | 20.55 ns | 10.857 ns |  1.34 |      48 B |
| StringCreate | 15.37 ns |  2.045 ns |  1.00 |      48 B |
 
 */



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

/*
 
| Method      | Mean     | Error     | Ratio | Allocated |
|------------ |---------:|----------:|------:|----------:|
| StringSplit | 46.19 ns | 16.916 ns |  1.94 |     144 B |
| SpanSplit   | 23.79 ns |  5.470 ns |  1.00 |         - |
 
 */