using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class BMSubstringRunner
{
    public static void Run()
    {
        var summary = BenchmarkDotNet.Running.BenchmarkRunner.Run<BMSubstring>();
    }
}


[ShortRunJob]
[MemoryDiagnoser(false)]
[HideColumns("StdDev", "RatioSD", "Alloc Ratio")]
[ReturnValueValidator]
public class BMSubstring {
    
    private const string _text = "Content-Length: 132";

    [Benchmark] 
    public int Substring() => int.Parse(_text.Substring(startIndex: 16));

    [Benchmark]
    public int AsSpanSlice() => int.Parse(_text.AsSpan().Slice(start: 16));

    [Benchmark(Baseline = true)]
    public int AsSpan() => int.Parse(_text.AsSpan(startIndex: 16));
}