using BenchmarkDotNet.Attributes;
using CastingObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfDemo._6_Casting;

[MemoryDiagnoser]
public class LinqCastingBenchmarks
{
    [Benchmark]
    public List<Person> OfType()
    {
        return StaticObjects.People
            .OfType<Person>()
            .ToList();
    }

    [Benchmark]
    public List<Person> Cast_As()
    {
        return StaticObjects.People
            .Where(x => x as Person is not null)
            .Cast<Person>()
            .ToList();
    }

    [Benchmark]
    public List<Person> Cast_Is()
    {
        return StaticObjects.People
            .Where(x => x is Person)
            .Cast<Person>()
            .ToList();
    }

    [Benchmark]
    public List<Person> HardCast_As()
    {
        return StaticObjects.People
            .Where(x => x as Person is not null)
            .Select(x => (Person)x)
            .ToList();
    }

    [Benchmark]
    public List<Person> HardCast_Is()
    {
        return StaticObjects.People
            .Where(x => x is Person)
            .Select(x => (Person)x)
            .ToList();
    }

    [Benchmark]
    public List<Person> HardCast_TypeOf()
    {
        return StaticObjects.People
            .Where(x => x.GetType() == typeof(Person))
            .Select(x => (Person)x)
            .ToList();
    }
}


/*
 net 9.0
| Method          | Mean      | Error    | StdDev   | Gen0    | Gen1    | Gen2    | Allocated |
|---------------- |----------:|---------:|---------:|--------:|--------:|--------:|----------:|
| OfType          |  43.12 us | 0.573 us | 0.536 us |  6.3477 |       - |       - |  78.27 KB |
| Cast_As         | 130.18 us | 2.493 us | 2.561 us | 41.5039 | 41.5039 | 41.5039 | 256.44 KB |
| Cast_Is         | 130.44 us | 1.670 us | 1.480 us | 41.5039 | 41.5039 | 41.5039 | 256.44 KB |
| HardCast_As     |  22.20 us | 0.289 us | 0.270 us |  6.3477 |       - |       - |  78.33 KB |
| HardCast_Is     |  22.19 us | 0.256 us | 0.227 us |  6.3477 |       - |       - |  78.33 KB |
| HardCast_TypeOf |  19.92 us | 0.393 us | 0.421 us |  6.3477 |       - |       - |  78.33 KB |

 
 */