namespace PerfDemo.Generics.Boxing;

public interface IMyInterface
{
    string BespokeToString();
}

public class MyClass : IMyInterface
{
    public string Value { get; internal set; }
    public string BespokeToString() => Value;
}

public struct MyStruct : IMyInterface
{
    public string Value { get; internal set; }
    public string BespokeToString() => Value;
}