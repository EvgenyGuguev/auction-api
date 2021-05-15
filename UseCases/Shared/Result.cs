using System.Collections.Generic;

namespace UseCases.Shared
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public IEnumerable<string> Error { get; set; }

        public static Result<T> Success(T value) => new() {IsSuccess = true, Value = value};
        public static Result<T> Failure(IEnumerable<string> error) => new() {IsSuccess = false, Error = error};
    }
}