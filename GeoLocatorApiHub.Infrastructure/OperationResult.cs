namespace GeoLocatorApiHub.Infrastructure
{
    public class OperationResult<T>
    {
        public T? Data { get; private set; }
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }

        public static OperationResult<T> Success(T data) => new OperationResult<T> { Data = data, IsSuccess = true };
        public static OperationResult<T> Failure(string errorMessage) => new OperationResult<T> { ErrorMessage = errorMessage, IsSuccess = false };
    }
}
