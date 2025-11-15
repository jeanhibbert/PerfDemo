namespace PerfDemo.Generics.Boxing;
internal class SharpLabScript
{
    public void Run()
    {

        MyStruct something = new MyStruct();
        ObjectService.DoSomething(something);
        InterfaceService.DoSomething(something);
        GenericService.DoSomething(something);



        /*
         
        using System;

 MyClass myClass = new MyClass();
 ObjectService.DoSomething(myClass);
 InterfaceService.DoSomething(myClass);
 GenericService.DoSomething(myClass);

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

public interface IMyInterface
{
    string BespokeToString();
}

public class MyClass : IMyInterface
{
    public string BespokeToString() => "Some value";
}

public struct MyStruct : IMyInterface
{
    public string BespokeToString() => "Some value";
}
         
         */

    }


}


