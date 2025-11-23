namespace PerfDemo.Generics.Boxing;
public class SharpLabScript
{
    public void Run()
    {

        MyStruct something = new MyStruct();
        ObjectService.DoSomething(something);
        InterfaceService.DoSomething(something);
        GenericService.DoSomething(something);



        /*
         
               using System;

 MyObject myClass = new MyObject();
 ObjectService.DoSomething(myClass);
 InterfaceService.DoSomething(myClass);
 GenericService.DoSomething(myClass);

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

public interface IMyInterface
{
    string BespokeToString();
}

public struct MyObject : IMyInterface //class
{
    public string BespokeToString() => "Some value";
}
         
         */

    }


}


