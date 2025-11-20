using BenchmarkDotNet.Attributes;

namespace PerfDemo.Generics.CallpathFactory;
internal class CallpathFactoryRunner
{
    public static void Run()
    {
        var summary = BenchmarkDotNet.Running.BenchmarkRunner.Run<BenchEliminationFactory>();
    }
}

[MemoryDiagnoser]
public class BenchEliminationFactory
{
    [Params(ServiceType.A)] public ServiceType ServiceType = ServiceType.A;

    [Benchmark]
    public int Regular() => RegularFactory.Create(ServiceType, 5);

    [Benchmark]
    public int Optimized() => OptimizedFactory.Create<ServiceTypeA>(5);
}

public class RegularFactory
{
    public static int Create
        (ServiceType serviceType, int number )
    {
        if (serviceType == ServiceType.A)
            return number * 4;
        
        if (serviceType == ServiceType.B)
            return number * 5;

        return 0;
    }
}

public class OptimizedFactory
{
    public static int Create<ServiceType>(int number)
    {
        if (typeof(ServiceType) == typeof(ServiceTypeA))
            return number * 4;
        
        if (typeof(ServiceType) == typeof(ServiceTypeB))
            return number * 5;

        return 0;
    }
}


internal static class RegularSwitchFactory
{
    public static int Create
        (ServiceType serviceType, int number )
    {
        return serviceType switch
        {
            ServiceType.A => number * 5,
            ServiceType.B => number * 5,
        };
    }
}

/*
 | Method    | ServiceType | Mean      | Error     | StdDev    | Median    | Allocated |
|---------- |------------ |----------:|----------:|----------:|----------:|----------:|
| Regular   | A           | 0.0126 ns | 0.0078 ns | 0.0073 ns | 0.0134 ns |         - |
| Optimized | A           | 0.0006 ns | 0.0009 ns | 0.0008 ns | 0.0003 ns |         - |
  */