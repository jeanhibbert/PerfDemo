using BenchmarkDotNet.Attributes;

namespace InAndOutExamples;

[MemoryDiagnoser(false)]
public class InAndOutBenchmarks
{
    private readonly ImmutableStruct _immutableStruct = new(6.9, 4.20);
    private readonly MutableStruct _mutableStruct = new(6.9, 4.20);
    private readonly MutableStructReadonly _mutableStructReadonly = new(6.9, 4.20);

    [Benchmark]
    public double MutableAddByType()
    {
        return AddByType(_mutableStruct);
    }

    [Benchmark]
    public double MutableAddByRefType()
    {
        return AddByReftype(in _mutableStruct);
    }

    [Benchmark]
    public double MutableReadOnlyAddByType()
    {
        return AddByType(_mutableStructReadonly);
    }

    [Benchmark]
    public double MutableReadonlyAddByRefType()
    {
        return AddByReftype(in _mutableStructReadonly);
    }

    [Benchmark]
    public double ImmutableAddByType()
    {
        return AddByType(_immutableStruct);
    }

    [Benchmark]
    public double ImmutableAddByRefType()
    {
        return AddByReftype(in _immutableStruct);
    }

    private double AddByType(ImmutableStruct s)
    {
        return s.X + s.Y;
    }

    private double AddByReftype(in ImmutableStruct s) //winner!
    {
        return s.X + s.Y;
    }

    public double AddByType(MutableStruct s)
    {
        return s.X + s.Y;
    }

    public double AddByReftype(in MutableStruct s)
    {
        return s.X + s.Y;
    }

    public double AddByType(MutableStructReadonly s)
    {
        return s.X + s.Y;
    }

    public double AddByReftype(in MutableStructReadonly s)
    {
        return s.X + s.Y;
    }
}


/*
 
| Method                      | Mean      | Error     | StdDev    | Median    | Allocated |
|---------------------------- |----------:|----------:|----------:|----------:|----------:|
| MutableAddByType            | 0.2444 ns | 0.0282 ns | 0.0264 ns | 0.2428 ns |         - |
| MutableAddByRefType         | 0.4241 ns | 0.0339 ns | 0.0317 ns | 0.4333 ns |         - |
| MutableReadOnlyAddByType    | 0.1651 ns | 0.0138 ns | 0.0123 ns | 0.1630 ns |         - |
| MutableReadonlyAddByRefType | 0.2284 ns | 0.0320 ns | 0.0342 ns | 0.2201 ns |         - |
| ImmutableAddByType          | 0.2084 ns | 0.0177 ns | 0.0165 ns | 0.2140 ns |         - |
| ImmutableAddByRefType       | 0.0155 ns | 0.0174 ns | 0.0163 ns | 0.0103 ns |         - |
 
 */


public readonly struct ImmutableStruct
{
    public double X { get; }

    public double Y { get; }

    public double Z { get; }

    private readonly double a;
    private readonly double b;
    private readonly double c;
    private readonly double d;
    private readonly double e;
    private readonly double f;
    private readonly double g;
    private readonly double h;

    public ImmutableStruct(double x, double y = 0, double z = 0)
    {
        X = x;
        Y = y;
        Z = z;
        a = 1;
        b = 2;
        c = 3;
        d = 4;
        e = 5;
        f = 6;
        g = 7;
        h = 8;
    }
}

public struct MutableStruct
{
    public double X { get => x; set => x = value; }

    public double Y { get => y; set => y = value; }

    public double Z { get => z; set => z = value; }

    private double a;
    private double b;
    private double c;
    private double d;
    private double e;
    private double f;
    private double g;
    private double h;
    private double z;
    private double y;
    private double x;

    public MutableStruct(double x, double y = 0, double z = 0)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.a = 1;
        this.b = 2;
        this.c = 3;
        this.d = 4;
        this.e = 5;
        this.f = 6;
        this.g = 7;
        this.h = 8;
    }
}
public struct MutableStructReadonly
{
    public double X { readonly get => x; set => x = value; }
    public double Y { readonly get => y; set => y = value; }
    public double Z { readonly get => z; set => z = value; }

    private double a;
    private double b;
    private double c;
    private double d;
    private double e;
    private double f;
    private double g;
    private double h;
    private double z;
    private double y;
    private double x;

    public MutableStructReadonly(double x, double y = 0, double z = 0)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.a = 1;
        this.b = 2;
        this.c = 3;
        this.d = 4;
        this.e = 5;
        this.f = 6;
        this.g = 7;
        this.h = 8;
    }
}
