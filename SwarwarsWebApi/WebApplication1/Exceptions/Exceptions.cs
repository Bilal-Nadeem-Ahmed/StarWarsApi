namespace MyStarwarsApi.Exceptions
{
    public class Exceptions
    {
        public class ApiException : Exception
        {
            public int StatusCode { get; }
            public string ErrorCode { get; }

            public ApiException(string message, int statusCode = 500, string errorCode = "GENERIC_ERROR")
                : base(message)
            {
                StatusCode = statusCode;
                ErrorCode = errorCode;
            }
        }

        public class NotFoundException : ApiException
        {
            public NotFoundException(string message, string errorCode = "NOT_FOUND")
                : base(message, 404, errorCode) { }
        }

        public class ValidationException : ApiException
        {
            public ValidationException(string message, string errorCode = "VALIDATION_ERROR")
                : base(message, 400, errorCode) { }
        }

    }
}
