namespace CatImageApi.Application.Result
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }
        public int StatusCode { get; }

        private Result(T value, int statusCode)
        {
            IsSuccess = true;
            Value = value;
            StatusCode = statusCode;
        }

        private Result(string error, int statusCode)
        {
            IsSuccess = false;
            Error = error;
            StatusCode = statusCode;
        }

        public static Result<T> Success(T value, int statusCode) => new(value, statusCode);
        public static Result<T> Failure(string error, int statusCode) => new(error, statusCode);
    }
}
