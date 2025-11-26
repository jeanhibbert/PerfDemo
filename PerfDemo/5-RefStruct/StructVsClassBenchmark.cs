using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PerfDemo._5_RefStruct;
using System.Drawing;

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
            var s = new MyReadOnlyRefStruct();
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
            var s = new MyReadOnlyRefStruct();
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
    private int ProcessReadOnlyRefStructByIn(in MyReadOnlyRefStruct s) => s.GetValue();
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
            var s = new MyReadOnlyRefStruct();
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
            var s1 = new MyReadOnlyRefStruct();
            var s2 = new MyReadOnlyRefStruct();
            var s3 = new MyReadOnlyRefStruct();
            var s4 = new MyReadOnlyRefStruct();
            var s5 = new MyReadOnlyRefStruct();

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

/*
 | Method                              | Mean         | Error      | StdDev      | Rank | Gen0   | Gen1   | Allocated |
|------------------------------------ |-------------:|-----------:|------------:|-----:|-------:|-------:|----------:|
| StackAllocatedReadOnlyRefStructs    |     2.329 ns |  0.0131 ns |   0.0116 ns |    1 |      - |      - |         - |
| StackAllocatedRefStructs            |     4.833 ns |  0.1236 ns |   0.1269 ns |    2 |      - |      - |         - |
| StructArray                         |    61.955 ns |  1.2380 ns |   1.3761 ns |    3 | 0.0337 |      - |     424 B |
| HeapAllocatedClasses                |   104.425 ns |  1.3495 ns |   1.1963 ns |    4 | 0.0956 |      - |    1200 B |
| PassReadOnlyRefStructByIn           |   197.902 ns |  2.9703 ns |   2.7784 ns |    5 |      - |      - |         - |
| CompoundReadOnlyRefStructOperations |   199.683 ns |  0.7434 ns |   0.6953 ns |    5 |      - |      - |         - |
| MutateStructByValue                 |   232.370 ns |  1.7409 ns |   1.6284 ns |    6 |      - |      - |         - |
| PassStructByValue                   |   233.199 ns |  1.4667 ns |   1.3002 ns |    6 |      - |      - |         - |
| PassRefStructByIn                   |   233.946 ns |  2.6196 ns |   2.4503 ns |    6 |      - |      - |         - |
| MutateRefStruct                     |   234.868 ns |  0.7534 ns |   0.7047 ns |    6 |      - |      - |         - |
| PassStructByIn                      |   237.077 ns |  3.4204 ns |   3.1995 ns |    6 |      - |      - |         - |
| ClassArray                          |   264.218 ns |  3.8403 ns |   3.2069 ns |    7 | 0.2565 | 0.0029 |    3224 B |
| CreateAndAccessRefStruct            |   354.916 ns |  6.0130 ns |   5.6246 ns |    8 |      - |      - |         - |
| CreateAndAccessStruct               |   356.996 ns |  3.0997 ns |   2.8995 ns |    8 |      - |      - |         - |
| CompoundRefStructOperations         |   372.366 ns |  0.9665 ns |   0.8568 ns |    9 |      - |      - |         - |
| CompoundStructOperations            |   378.580 ns |  4.7302 ns |   4.1932 ns |    9 |      - |      - |         - |
| CreateAndAccessReadOnlyRefStruct    |   504.039 ns |  1.9151 ns |   1.7914 ns |   10 |      - |      - |         - |
| StructViaInterface                  | 1,464.093 ns | 28.6059 ns |  32.9426 ns |   11 | 1.9112 |      - |   24000 B |
| ClassViaInterface                   | 1,481.597 ns | 29.4646 ns |  41.3053 ns |   11 | 1.9112 |      - |   24000 B |
| PassClassByReference                | 1,484.040 ns | 28.1118 ns |  28.8688 ns |   11 | 1.9112 |      - |   24000 B |
| PassClassByInReference              | 1,522.931 ns | 24.0471 ns |  22.4936 ns |   11 | 1.9112 |      - |   24000 B |
| MutateClass                         | 1,532.124 ns | 22.9062 ns |  21.4264 ns |   11 | 1.9112 |      - |   24000 B |
| CompoundClassOperations             | 1,672.890 ns | 25.3197 ns |  23.6840 ns |   12 | 1.9112 |      - |   24000 B |
| CreateAndAccessClass                | 2,979.216 ns | 58.4528 ns | 118.0775 ns |   13 | 1.9112 |      - |   24000 B |
 */