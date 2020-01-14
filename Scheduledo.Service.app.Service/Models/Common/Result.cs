using Scheduledo.Core.Enums;

namespace Scheduledo.Service.Models
{
    public class Result<T> : Result
    {
        public T Data { get; set; }

        public Result() : base()
        {
        }

        public Result(T content) : base()
        {
            Data = content;
        }

        public bool IsDataNull => Data == null;
    }

    public class Result
    {
        public Result() : this(false)
        {
        }

        public Result(bool success)
        {
            Error = ErrorType.None;
            ErrorMessage = string.Empty;
        }

        public bool Success
        {
            get
            {
                if (Error == ErrorType.None)
                    return true;

                return false;
            }
        }

        public ErrorType Error { get; set; }
        public string ErrorMessage { get; set; }
    }
}