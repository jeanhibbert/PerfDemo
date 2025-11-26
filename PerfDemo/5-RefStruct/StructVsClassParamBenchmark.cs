using BenchmarkDotNet.Attributes;

namespace PerfDemo._5_RefStruct;

public interface ISomeInterface
{
    int GetValue();
}

public struct MyMutatableStruct : ISomeInterface
{
    public int X;
    public int GetValue() => X;
    public void Mutate() => X++;
}

public ref struct MyRefStruct //: IMyStruct
{
    public int X;
    public int GetValue() => X;
    public void Mutate() => X++;
}

public readonly ref struct MyReadOnlyRefStruct //: IMyStruct
{
    public readonly int X => 1;
    public readonly int GetValue() => X;
    //public void Mutate() => X++;
}

public class MyClass : ISomeInterface
{
    public int X;
    public int GetValue() => X;
    public void Mutate() => X++;
}

public class MyService
{
    public void ProcessRefStructIn(in MyRefStruct s)
    {
    }

    public void ProcessReadOnlyRefStructIn(in MyReadOnlyRefStruct s)
    {
    }

    public void ProcessClassIn(in MyClass c)
    {
    }
}


[MemoryDiagnoser]
public class StructVsClassParamBenchmark
{
    private MyService _service;

    [GlobalSetup]
    public void Setup()
    {
        _service = new MyService();
    }

    [Benchmark]
    public void ProcessRefStructIn()
    {
        var s = new MyRefStruct();
        _service.ProcessRefStructIn(in s);
    }

    [Benchmark]
    public void ProcessReadOnlyRefStructIn() { 
        var s = new MyReadOnlyRefStruct();
        _service.ProcessReadOnlyRefStructIn(in s);
    }

    [Benchmark]
    public void ProcessClassIn() {
        var c = new MyClass();
        _service.ProcessClassIn(in c);
    }
}

/*
 net9.0
 | Method                     | Mean      | Error     | StdDev    | Median    | Allocated |
|--------------------------- |----------:|----------:|----------:|----------:|----------:|
| ProcessRefStructIn         | 0.0082 ns | 0.0087 ns | 0.0082 ns | 0.0074 ns |         - |
| ProcessReadOnlyRefStructIn | 0.0080 ns | 0.0026 ns | 0.0023 ns | 0.0079 ns |         - |
| ProcessClassIn             | 0.0002 ns | 0.0006 ns | 0.0006 ns | 0.0000 ns |         - |
 */