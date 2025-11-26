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
}

/*
 net9.0
 | Method    | Mean      | Error     | StdDev    | Allocated |
|---------- |----------:|----------:|----------:|----------:|
| HardCast  | 0.7282 ns | 0.0318 ns | 0.0297 ns |         - |
| SafeCast  | 0.8860 ns | 0.0400 ns | 0.0374 ns |         - |
| MatchCast | 0.9565 ns | 0.0707 ns | 0.0695 ns |         - |
 */