namespace PerfDemo.Generics.CallpathFactory;

public interface IService;

internal class A : IService
{
}

internal class B : IService
{
}

public enum ServiceType
{
    A,
    B
}

public interface IServiceType { };
public struct ServiceTypeA : IServiceType { }
public struct ServiceTypeB : IServiceType { }

