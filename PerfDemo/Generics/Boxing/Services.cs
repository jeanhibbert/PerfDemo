namespace PerfDemo.Generics.Boxing;

internal class ObjectService
{
    internal static string DoSomething(object something) => (something as IMyInterface).BespokeToString();
}

internal class InterfaceService
{
    internal static string DoSomething(IMyInterface something) => something.BespokeToString();
}

internal class GenericService
{
    internal static string DoSomething<T>(T something) where T : IMyInterface => something.BespokeToString();
}
