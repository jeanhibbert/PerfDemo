using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Drawing;

namespace PerfDemo;

public interface ISomeInterface
{
    int GetValue();
}

public struct MyStruct : ISomeInterface
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

public readonly ref struct MyReadOnlyStruct //: IMyStruct
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

[SimpleJob]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class StructVsClassBenchmark
{
    const int iterations = 1_000;
    
    [GlobalSetup]
    public void Setup()
    {
    }

    [Benchmark]
    public void ProcessStructWithIn()
    {
        for (int i = 0; i < iterations; i++)
        {
            var myStruct = new MyStruct();
            ProcessStructWithIn(myStruct);
        }
    }

    public void ProcessStructWithIn(in MyStruct myStruct)
    {
        myStruct.GetValue();
    }
    
    [Benchmark]
    public void ProcessRefStructWithIn()
    {
        for (int i = 0; i < iterations; i++)
        {
            var myStruct = new MyRefStruct();
            ProcessRefStructWithIn(myStruct);
        }
    }

    public void ProcessRefStructWithIn(in MyRefStruct myStruct)
    {
        myStruct.GetValue();
    }

    [Benchmark]
    public void ProcessMyReadonlyStructWithIn()
    {
        for (int i = 0; i < iterations; i++)
        {
            var myStruct = new MyReadOnlyStruct();
            ProcessMyReadonlyStructWithIn(myStruct);
        }
    }

    public void ProcessMyReadonlyStructWithIn(in MyReadOnlyStruct myStruct)
    {
        myStruct.GetValue();
    }

    [Benchmark]
    public void ProcessMyClassWithIn()
    {
        for (int i = 0; i < iterations; i++)
        {
            var myStruct = new MyClass();
            ProcessMyClassWithIn(myStruct);
        }
    }

    public void ProcessMyClassWithIn(in MyClass myStruct)
    {
        myStruct.GetValue();
    }
}
