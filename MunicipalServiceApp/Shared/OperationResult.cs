using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MunicipalServiceApp.Shared
{
    public class OperationResult
    {
        public bool Success { get; }
        public string? ErrorMessage { get; }

        protected OperationResult(bool success, string? error)
        {
            Success = success;
            ErrorMessage = error;
        }

        public static OperationResult Ok() => new(true, null);
        public static OperationResult Fail(string error) => new(false, error);
    }

    public sealed class OperationResult<T> : OperationResult
    {
        public T? Value { get; }

        private OperationResult(bool success, string? error, T? value)
            : base(success, error)
        {
            Value = value;
        }

        public static OperationResult<T> Ok(T value) => new(true, null, value);
        public static new OperationResult<T> Fail(string error) => new(false, error, default);
    }
}
