namespace InAndOutExamples;

public class OutExample
{
    public (bool CouldParse, int ParsedValue) TryParse(string valueToParse)
    {
        var success = int.TryParse(valueToParse, out var parsedValue);
        return (success, parsedValue);
    }

    public void ExampleOut(out int intValue)
    {
        intValue = 10;
    }

    public void ExampleOut(int intValue)
    {
        intValue = 10;
    }
}
