using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class BMSplitRunner {
    public static void Run() {
        var summary = BenchmarkDotNet.Running.BenchmarkRunner.Run<BMSplit>();
    }
}


[ShortRunJob]
[MemoryDiagnoser(false), HideColumns("StdDev", "RatioSD", "Alloc Ratio"), ReturnValueValidator]
public class BMSplit {

    private const string _text = "123,456,789";

    [Benchmark]
    public int StringSplit() {
        string[] a = _text.Split(',', StringSplitOptions.RemoveEmptyEntries);
        int t = 0;
        foreach (string word in a) t += int.Parse(word);
        return t;
    }


    [Benchmark(Baseline = true)]
    public int SpanSplit() {
        ReadOnlySpan<char> span = _text.AsSpan();
        int t = 0;
        foreach (Range range in span.Split(',')) {
            if (range.Start.Value < range.End.Value) t += int.Parse(span[range]);
        }
        return t;
    }
}