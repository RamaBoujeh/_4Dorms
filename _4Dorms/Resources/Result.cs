public class Result
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }

    public Result() { }

    public Result(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }
}
