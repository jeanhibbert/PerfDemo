namespace PerfDemo.Generics.Boxing;

public class ObjectService
{
    public static string DoSomething(object something) => (something as IMyInterface).BespokeToString();
}

public class InterfaceService
{
    public static string DoSomething(IMyInterface something) => something.BespokeToString();
}

public class GenericService
{
    public static string DoSomething<T>(T something) where T : IMyInterface => something.BespokeToString();
}
