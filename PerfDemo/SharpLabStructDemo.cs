using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructDemo;
public struct MyStruct
{
    public int X;
    public int GetValue() => X;
    public void Mutate() => X++;
}

public ref struct MyRefStruct
{
    public int X;
    public int GetValue() => X;
    public void Mutate() => X++;
}

public readonly ref struct MyReadOnlyStruct
{
    public readonly int X;
    public readonly int GetValue() => X;
    //public void Mutate() => X++;
}

internal class SharpLabStructDemo
{
    public void ProcessStructWithIn()
    {
        const int iterations = 1_000_000;

        for (int i = 0; i < iterations; i++)
        {
            var myStruct = new MyReadOnlyStruct();
            ProcessStructWithIn(myStruct);
        }
    }

    private void ProcessStructWithIn(in MyReadOnlyStruct myStruct)
    {
        myStruct.GetValue();
    }
}
