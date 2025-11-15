using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace PerfDemo.Generics.Boxing;

internal class BoxingRunner
{
    public static void Run()
    {
        BenchmarkRunner.Run<BenchStructs>();
    }
}

[MemoryDiagnoser]
public class BenchObjects
{
    private MyClass _myClass = new MyClass();

    [GlobalSetup]
    public void Setup()
    {
        _myClass = new MyClass { Value = "Some value" };
    }

    [Benchmark]
    public string Object() => ObjectService.DoSomething(_myClass);

    [Benchmark]
    public string Interface() => InterfaceService.DoSomething(_myClass);


    [Benchmark]
    public string Generic() => GenericService.DoSomething(_myClass);
}

/*
 * | Method    | Mean      | Error     | StdDev    | Median    | Allocated |
|---------- |----------:|----------:|----------:|----------:|----------:|
| Object    | 0.0382 ns | 0.0250 ns | 0.0234 ns | 0.0340 ns |         - |
| Interface | 0.0008 ns | 0.0035 ns | 0.0029 ns | 0.0000 ns |         - |
| Generic   | 0.0434 ns | 0.0186 ns | 0.0174 ns | 0.0397 ns |         - |
 */


[MemoryDiagnoser]
public class BenchStructs
{
    private MyStruct _myStruct = new MyStruct();

    [GlobalSetup]
    public void Setup()
    {
        _myStruct = new MyStruct { Value = "Some value" };
    }

    [Benchmark]
    public string Object() => ObjectService.DoSomething(_myStruct);

    [Benchmark]
    public string Interface() => InterfaceService.DoSomething(_myStruct);


    [Benchmark]
    public string Generic() => GenericService.DoSomething(_myStruct);
}

/*
 | Method    | Mean      | Error     | StdDev    | Median    | Gen0   | Allocated |
|---------- |----------:|----------:|----------:|----------:|-------:|----------:|
| Object    | 2.4998 ns | 0.1000 ns | 0.1777 ns | 2.5277 ns | 0.0019 |      24 B |
| Interface | 2.2054 ns | 0.0540 ns | 0.0505 ns | 2.2074 ns | 0.0019 |      24 B |
| Generic   | 0.0036 ns | 0.0082 ns | 0.0073 ns | 0.0000 ns |      - |         - |
 */