using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text.Json;

namespace PerfDemo.Generics.GenericList;
public class GenericListRunner
{
    public static void Run()
    {
        BenchmarkRunner.Run<BenchLists>();
    }
}

[MemoryDiagnoser]
public class BenchLists
{
    [Params(100, 1000, 10000)] public int Count;

    [Benchmark]
    public string ArrayListWrite()
    {
        var list = new System.Collections.ArrayList();
        for (int i = 0; i < Count; i++)
        {
            list.Add(new Dummy());
        }
        return JsonSerializer.Serialize(list);
    }

    [Benchmark]
    public string ListWrite()
    {
        var list = new List<Dummy>();
        for (int i = 0; i < Count; i++)
        {
            list.Add(new Dummy());
        }
        return JsonSerializer.Serialize(list);
    }
}

public class  Dummy;

/* Results:
 
| Method         | Count | Mean       | Error     | StdDev    | Gen0    | Gen1    | Gen2    | Allocated |
|--------------- |------ |-----------:|----------:|----------:|--------:|--------:|--------:|----------:|
| ArrayListWrite | 100   |   4.950 us | 0.0372 us | 0.0290 us |  0.4349 |       - |       - |    5.4 KB |
| ListWrite      | 100   |   4.047 us | 0.0480 us | 0.0401 us |  0.4349 |       - |       - |    5.4 KB |
| ArrayListWrite | 1000  |  46.756 us | 0.5629 us | 0.4700 us |  3.7231 |  0.3662 |       - |  45.84 KB |
| ListWrite      | 1000  |  38.923 us | 0.5001 us | 0.4678 us |  3.7231 |  0.3662 |       - |  45.84 KB |
| ArrayListWrite | 10000 | 499.926 us | 4.5618 us | 3.8093 us | 41.0156 | 41.0156 | 41.0156 | 549.63 KB |
| ListWrite      | 10000 | 451.441 us | 5.1033 us | 4.7737 us | 41.5039 | 41.5039 | 41.5039 | 549.63 KB |

 
 */