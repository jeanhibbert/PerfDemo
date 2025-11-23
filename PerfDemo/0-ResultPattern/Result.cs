namespace PerfDemo._0_ResultPattern;

public record Result<TValue>
{
    private readonly TValue? _value;
    private readonly bool _isSuccess;
    private readonly Error _error;

    private Result(TValue value)
    {
        _value = value;
        _isSuccess = true;
        _error = Error.None;    
    }

    private Result(Error error)
    {
        _error = error;
        _isSuccess = false;
        _value = default;
    }

    public static Result<TValue> Success(TValue value) => new Result<TValue>(value);
    public static Result<TValue> Failure(Error error) => new Result<TValue>(error);
    
    public bool IsSuccess => _isSuccess;
    
    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(Error error) => Failure(error);
}

public record Error(string Code)
{
    public static Error None => new Error(string.Empty);
    public static Error ValidationFailure => new Error("VALIDATION_FAILURE");
}


