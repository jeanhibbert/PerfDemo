using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class BMConcatToUpperRunner
{
    public static void Run()
    {
        var summary = BenchmarkDotNet.Running.BenchmarkRunner.Run<BMConcatToUpper>();
    }
}

[ShortRunJob]
[MemoryDiagnoser(false), HideColumns("StdDev", "RatioSD", "Alloc Ratio"), ReturnValueValidator]
public class BMConcatToUpper {

    private string _text1 = "Hello";
    private string _text2 = "World";

    [Benchmark]
    public string String() => $"{_text1} {_text2}".ToUpper();

    [Benchmark]
    public string Span() {
        Span<char> destination = stackalloc char[_text1.Length + 1 + _text2.Length];
        _text1.AsSpan().ToUpper(destination, null);
        destination[_text1.Length] = ' ';
        _text2.AsSpan().ToUpper(destination.Slice(_text1.Length + 1, _text2.Length), null);
        return destination.ToString();
    }

    [Benchmark(Baseline = true)]
    public string StringCreate() {
        return string.Create(_text1.Length + 1 + _text2.Length, (_text1, _text2),
                             static (Span<char> destination, (string Text1, string Text2) state) => {

                                 state.Text1.AsSpan().ToUpper(destination, null);
                                 destination[state.Text1.Length] = ' ';
                                 state.Text2.AsSpan().ToUpper(destination.Slice(state.Text1.Length + 1, state.Text2.Length), null);
                             });
    }
}