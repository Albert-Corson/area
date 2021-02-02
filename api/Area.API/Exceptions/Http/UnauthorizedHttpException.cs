using System;
using System.Net;
using System.Runtime.Serialization;

namespace Area.API.Exceptions.Http
{
    public class UnauthorizedHttpException : HttpException
    {
        private const HttpStatusCode ExceptionStatusCode = HttpStatusCode.Unauthorized;

        private new const string Message = "Unauthorized";

        public UnauthorizedHttpException(string message) : base(ExceptionStatusCode, message)
        { }

        public UnauthorizedHttpException(string message, Exception inner) : base(ExceptionStatusCode, message, inner)
        { }

        public UnauthorizedHttpException() : base(ExceptionStatusCode, Message)
        { }

        protected UnauthorizedHttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}