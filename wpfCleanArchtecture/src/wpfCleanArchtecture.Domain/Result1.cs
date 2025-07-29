public class Result
{
    public bool IsSuccess { get; private set; }
    public string Error { get; private set; } = string.Empty;

    private Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, string.Empty);
    public static Result Failure(string error) => new(false, error);
}