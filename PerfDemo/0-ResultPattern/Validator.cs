namespace PerfDemo._0_ResultPattern;

public class Validator
{
    public static Result<bool> ValidateAge(int age)
    {
        if (age == 0)
        {
            return Error.ValidationFailure;
        }
        return true;
    }

    public static bool ValidateAgeWithException(int age)
    {
        if (age == 0)
        {
            throw new ArgumentException("Age cannot be zero", nameof(age));
        }
        return true;
    }
}
