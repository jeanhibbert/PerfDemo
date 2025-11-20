using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Drawing;

namespace PerfDemo;

public interface ISomeInterface
{
    int GetValue();
}

public struct MyMutatableStruct : ISomeInterface
{
    public int X;
    public int GetValue() => X;
    public void Mutate() => X++;
}

public ref struct MyRefStruct //: IMyStruct
{
    public int X;
    public int GetValue() => X;
    public void Mutate() => X++;
}

public readonly ref struct MyReadOnlyStruct //: IMyStruct
{
    public readonly int X => 1;
    public readonly int GetValue() => X;
    //public void Mutate() => X++;
}

public class MyClass : ISomeInterface
{
    public int X;
    public int GetValue() => X;
    public void Mutate() => X++;
}

[SimpleJob]
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class StructVsClassBenchmark
{
    private const int Iterations = 1000;

    [GlobalSetup]
    public void Setup()
    {
    }

    // =============================================================================
    // Benchmark 1: Simple creation and access (no passing)
    // =============================================================================

    [Benchmark]
    public int CreateAndAccessStruct()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyMutatableStruct { X = i };
            sum += s.GetValue();
        }
        return sum;
    }

    [Benchmark]
    public int CreateAndAccessRefStruct()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyRefStruct { X = i };
            sum += s.GetValue();
        }
        return sum;
    }

    [Benchmark]
    public int CreateAndAccessReadOnlyRefStruct()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyReadOnlyStruct();
            sum += s.GetValue();
        }
        return sum;
    }

    [Benchmark]
    public int CreateAndAccessClass()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var c = new MyClass { X = i };
            sum += c.GetValue();
        }
        return sum;
    }

    // =============================================================================
    // Benchmark 2: Passing by value (shows struct copy cost)
    // =============================================================================

    [Benchmark]
    public int PassStructByValue()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyMutatableStruct { X = i };
            sum += ProcessStructByValue(s);
        }
        return sum;
    }

    [Benchmark]
    public int PassClassByReference()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var c = new MyClass { X = i };
            sum += ProcessClassByReference(c);
        }
        return sum;
    }

    private int ProcessStructByValue(MyMutatableStruct s) => s.GetValue();
    private int ProcessClassByReference(MyClass c) => c.GetValue();

    // =============================================================================
    // Benchmark 3: Passing by 'in' reference (optimal for structs)
    // =============================================================================

    [Benchmark]
    public int PassStructByIn()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyMutatableStruct { X = i };
            sum += ProcessStructByIn(in s);
        }
        return sum;
    }

    [Benchmark]
    public int PassRefStructByIn()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyRefStruct { X = i };
            sum += ProcessRefStructByIn(in s);
        }
        return sum;
    }

    [Benchmark]
    public int PassReadOnlyRefStructByIn()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyReadOnlyStruct();
            sum += ProcessReadOnlyRefStructByIn(in s);
        }
        return sum;
    }

    [Benchmark]
    public int PassClassByInReference()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var c = new MyClass { X = i };
            sum += ProcessClassByIn(in c);
        }
        return sum;
    }

    private int ProcessStructByIn(in MyMutatableStruct s) => s.GetValue();
    private int ProcessRefStructByIn(in MyRefStruct s) => s.GetValue();
    private int ProcessReadOnlyRefStructByIn(in MyReadOnlyStruct s) => s.GetValue();
    private int ProcessClassByIn(in MyClass c) => c.GetValue();

    // =============================================================================
    // Benchmark 4: Mutation (shows defensive copy issue)
    // =============================================================================

    [Benchmark]
    public int MutateStructByValue()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyMutatableStruct { X = i };
            s.Mutate(); // Direct mutation - no defensive copy
            sum += s.X;
        }
        return sum;
    }

    [Benchmark]
    public int MutateRefStruct()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyRefStruct { X = i };
            s.Mutate(); // Direct mutation - no defensive copy
            sum += s.X;
        }
        return sum;
    }

    [Benchmark]
    public int MutateClass()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var c = new MyClass { X = i };
            c.Mutate(); // Mutation via reference
            sum += c.X;
        }
        return sum;
    }

    // =============================================================================
    // Benchmark 5: Array/Collection storage (classes allocate, structs don't)
    // =============================================================================

    [Benchmark]
    public int StructArray()
    {
        var array = new MyMutatableStruct[100];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = new MyMutatableStruct { X = i };
        }

        int sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i].GetValue();
        }
        return sum;
    }

    [Benchmark]
    public int ClassArray()
    {
        var array = new MyClass[100];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = new MyClass { X = i }; // Heap allocation per instance!
        }

        int sum = 0;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i].GetValue();
        }
        return sum;
    }

    // =============================================================================
    // Benchmark 6: Interface boxing (struct gets boxed = heap allocation)
    // =============================================================================

    [Benchmark]
    public int StructViaInterface()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            MyMutatableStruct s = new MyMutatableStruct { X = i };
            ISomeInterface iface = s; // BOXING - heap allocation!
            sum += iface.GetValue();
        }
        return sum;
    }

    [Benchmark]
    public int ClassViaInterface()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            MyClass c = new MyClass { X = i };
            ISomeInterface iface = c; // No boxing - already a reference
            sum += iface.GetValue();
        }
        return sum;
    }

    // =============================================================================
    // Benchmark 7: Multiple operations (compound test)
    // =============================================================================

    [Benchmark]
    public int CompoundStructOperations()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyMutatableStruct { X = i };
            s.Mutate();
            sum += ProcessStructByIn(in s);
            s.Mutate();
            sum += s.GetValue();
        }
        return sum;
    }

    [Benchmark]
    public int CompoundRefStructOperations()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyRefStruct { X = i };
            s.Mutate();
            sum += ProcessRefStructByIn(in s);
            s.Mutate();
            sum += s.GetValue();
        }
        return sum;
    }

    [Benchmark]
    public int CompoundReadOnlyRefStructOperations()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var s = new MyReadOnlyStruct();
            sum += ProcessReadOnlyRefStructByIn(in s);
            sum += s.GetValue();
        }
        return sum;
    }

    [Benchmark]
    public int CompoundClassOperations()
    {
        int sum = 0;
        for (int i = 0; i < Iterations; i++)
        {
            var c = new MyClass { X = i };
            c.Mutate();
            sum += ProcessClassByIn(in c);
            c.Mutate();
            sum += c.GetValue();
        }
        return sum;
    }

    // =============================================================================
    // Benchmark 8: Stack allocation scenarios (ref struct advantage)
    // =============================================================================

    [Benchmark]
    public int StackAllocatedRefStructs()
    {
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            // Simulate stack-heavy operations
            var s1 = new MyRefStruct { X = i };
            var s2 = new MyRefStruct { X = i + 1 };
            var s3 = new MyRefStruct { X = i + 2 };
            var s4 = new MyRefStruct { X = i + 3 };
            var s5 = new MyRefStruct { X = i + 4 };

            sum += s1.GetValue() + s2.GetValue() + s3.GetValue() + 
                   s4.GetValue() + s5.GetValue();
        }
        return sum;
    }

    [Benchmark]
    public int StackAllocatedReadOnlyRefStructs()
    {
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            // Simulate stack-heavy operations with readonly
            var s1 = new MyReadOnlyStruct();
            var s2 = new MyReadOnlyStruct();
            var s3 = new MyReadOnlyStruct();
            var s4 = new MyReadOnlyStruct();
            var s5 = new MyReadOnlyStruct();

            sum += s1.GetValue() + s2.GetValue() + s3.GetValue() + 
                   s4.GetValue() + s5.GetValue();
        }
        return sum;
    }

    [Benchmark]
    public int HeapAllocatedClasses()
    {
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            // Each allocation goes to heap
            var c1 = new MyClass { X = i };
            var c2 = new MyClass { X = i + 1 };
            var c3 = new MyClass { X = i + 2 };
            var c4 = new MyClass { X = i + 3 };
            var c5 = new MyClass { X = i + 4 };

            sum += c1.GetValue() + c2.GetValue() + c3.GetValue() + 
                   c4.GetValue() + c5.GetValue();
        }
        return sum;
    }
}
