namespace PerfDemo.Generics.CallpathElimination;

public static class ObjectService
{
    public static int GetType(object something)
    {
        if (something.GetType() == typeof(int))
            return 1;

        return 0;
    }

    public static int Is(object something)
    {
        if (something is int)
            return 1;

        return 0;
    }
}

public static class GenericService
{
    public static int GetType<T>(T something)
    {
        if (something.GetType() == typeof(int))
            return 1;

        return 0;
    }

    public static int Is<T>(T something)
    {
        if (something is int)
            return 1;

        return 0;
    }

    public static int TypeOf<T>(T something)
    {
        if (typeof(T) == typeof(int))
            return 1;
        
        return 0;
    }
}