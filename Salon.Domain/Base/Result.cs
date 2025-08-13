using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Salon.Domain.Base
{
    public class Result : ObjectResult
    {
        public bool Error { get; set; } = false;
        public string Message { get; set; }

        public Result() : base(null) { }

        public Result(object value) : base(value) { StatusCode = (int)HttpStatusCode.OK; }
        public Result(object value, HttpStatusCode statusCode) : base(value) { StatusCode = (int)statusCode; }
        public Result(HttpStatusCode statusCode) : base(null) { StatusCode = (int)statusCode; }

        public Result(bool error, string message, HttpStatusCode statusCode) : base(null)
        {
            Error = error;
            Message = message;
            StatusCode = ((int)statusCode);
        }

        public Result ValidationError(string message)
        {
            Error = true;
            Message = message;
            StatusCode = (int)HttpStatusCode.BadRequest;
            return this;
        }
    }
}
