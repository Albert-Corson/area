using System;
using System.Net;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Area.API.Exceptions
{
    public class HttpException : Exception
    {
        public HttpException(HttpStatusCode code)
        {
            StatusCode = code;
        }

        public HttpException(HttpStatusCode code, string message) : base(message)
        {
            StatusCode = code;
        }

        public HttpException(HttpStatusCode code, string message, Exception inner) : base(message, inner)
        {
            StatusCode = code;
        }

        public HttpException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public HttpException(string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        protected HttpException()
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public HttpStatusCode StatusCode { get; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}