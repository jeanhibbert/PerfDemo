using BenchmarkDotNet.Attributes;

namespace CastingObjects;

[SimpleJob]
[MemoryDiagnoser(false)]
public class CastingBenchmarks
{
    [Benchmark]
    public Person HardCast()
    {
        Person nickHardCast = (Person)StaticObjects.Nick;
        return nickHardCast;
    }

    [Benchmark]
    public Person SafeCast()
    {
        Person? nick = StaticObjects.Nick as Person;
        return nick!;
    }

    [Benchmark]
    public Person MatchCast()
    {
        if (StaticObjects.Nick is Person person)
        {
            return person;
        }

        return null!;
    }

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
 net9.0
| Method          | Mean      | Error    | StdDev   | Allocated |
|---------------- |----------:|---------:|---------:|----------:|
| OfType          |  43.17 us | 0.491 us | 0.460 us |  78.27 KB |
| Cast_As         | 133.21 us | 1.619 us | 1.514 us | 256.44 KB |
| Cast_Is         | 143.23 us | 2.063 us | 1.930 us | 256.44 KB |
| HardCast_As     |  22.92 us | 0.443 us | 0.474 us |  78.33 KB |
| HardCast_Is     |  22.72 us | 0.422 us | 0.395 us |  78.33 KB |
| HardCast_TypeOf |  22.09 us | 0.213 us | 0.199 us |  78.33 KB |
 
 */