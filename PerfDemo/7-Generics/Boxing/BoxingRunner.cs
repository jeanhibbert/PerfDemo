using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace PerfDemo.Generics.Boxing;

internal class BoxingRunner
{
    public static void Run()
    {
        
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
net9.0
| Method    | Mean      | Error     | StdDev    | Median    | Allocated |
|---------- |----------:|----------:|----------:|----------:|----------:|
| Object    | 0.0228 ns | 0.0228 ns | 0.0213 ns | 0.0156 ns |         - |
| Interface | 0.1880 ns | 0.0198 ns | 0.0185 ns | 0.1861 ns |         - |
| Generic   | 0.0419 ns | 0.0270 ns | 0.0252 ns | 0.0430 ns |         - | 

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
net9.0
| Method    | Mean      | Error     | StdDev    | Median    | Allocated |
|---------- |----------:|----------:|----------:|----------:|----------:|
| Object    | 0.0269 ns | 0.0318 ns | 0.0298 ns | 0.0180 ns |         - |
| Interface | 0.0006 ns | 0.0025 ns | 0.0023 ns | 0.0000 ns |         - |
| Generic   | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |         - |
 */