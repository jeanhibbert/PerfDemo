namespace PerfDemo.Examples;

public class ParseWithSpanExample
{

    public static void Run()
    {

        int[] a1 = { 1, 2, 3, 4, 5, 6, 7, 8 };

        int[] a2 = a1[2..6]; // another heap allocation requiring garbage collection :(

        a2[3] = 42;

        foreach (var item in a1) Console.WriteLine(item);

        Console.WriteLine("----");

        foreach (var item in a2) Console.WriteLine(item);

        //foreach (var item in intSpan) Console.WriteLine(item);


        //ReadOnlySpan<string> strSpan = [ "one", "two", "three", "four", "five", "six", "seven", "eight" ];

        //Span<MyClass> classSpan = [new MyClass(1), new MyClass(2), new MyClass(3)];
        //foreach (var item in classSpan) Console.WriteLine(item.Value);

        //Span<MyRefStruct> structSpan = [ new MyRefStruct(1), new MyRefStruct(2), new MyRefStruct(3), new MyRefStruct(4), new MyRefStruct(5), new MyRefStruct(6), new MyRefStruct(7), new MyRefStruct(8) ];
        //foreach (var item in structSpan) Console.WriteLine(item.Value);

        //ReadOnlySpan<char> charSpan = "Hello, World!".AsSpan().Slice(7, 5); // slice or range>

        //foreach (var item in charSpan) Console.WriteLine(item);

        //Span<int> a1 = stackalloc int[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        //foreach (var item in a1) Console.WriteLine(item);

        Console.ReadLine();
    }


    public class MyClass
    {
        public int Value;
        public MyClass(int value)
        {
            Value = value;
        }
    }

    public struct MyRefStruct
    {
        public int Value;
        public MyRefStruct(int value)
        {
            Value = value;
        }
    }

}
