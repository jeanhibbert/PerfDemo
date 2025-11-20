namespace CastingObjects;

public class StaticObjects
{
    public static object Nick = new Person
    {
        Id = Guid.NewGuid(),
        FullName = "Nick Chapsas"
    };

    public static List<object> People = Enumerable
        .Range(0, 10000)
        .Select(x => (object)new Person
        {
            Id = Guid.NewGuid(),
            FullName = Guid.NewGuid().ToString()
        }).ToList();
}
