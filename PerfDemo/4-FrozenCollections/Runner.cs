using BenchmarkDotNet.Running;
using FrozenCollections;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace PerfDemo.FrozenCollections;
public class Runner
{
    public static void Run()
    {
        

        var dic = new Dictionary<string, int>
{
    { "1", 1 },
    { "2", 2 },
    { "3", 3 },
};

        var immutableDic = dic.ToImmutableDictionary();

        var frozenDic = dic.ToFrozenDictionary();

        ref var value = ref Unsafe.AsRef(in frozenDic.GetValueRefOrNullRef("2"));

        value = 10;

        Console.WriteLine();

        /*
        var list = new List<int> { 1, 2, 2, 3 };

        var hashSet = list.ToHashSet();

        var immutableHashSet = list.ToImmutableHashSet();

        var frozenSet = list.ToFrozenSet();

        Console.WriteLine();
        */
    }
}
