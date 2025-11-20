using BenchmarkDotNet.Running;
using FastListIteration;

public class ListRunner
{
    public static void Run()
    {
        BenchmarkRunner.Run<ListIterationBenchmarks>();
    }
}
