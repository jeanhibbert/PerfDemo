using BenchmarkDotNet.Running;
using FastListIteration;

public class ListRunner
{
    public void Run()
    {
        BenchmarkRunner.Run<Benchmarks>();
    }
}
