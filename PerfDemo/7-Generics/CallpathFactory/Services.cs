namespace PerfDemo.Generics.CallpathFactory;

public interface IService;

public class A : IService
{
}

public class B : IService
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

