using System;
using System.Net;
using System.Runtime.Serialization;

namespace Dashboard.API.Exceptions.Http
{
    public class InternalServerErrorHttpException : HttpException
    {
        private const HttpStatusCode ExceptionStatusCode = HttpStatusCode.InternalServerError;

        private new const string Message = "Internal server error";

        public InternalServerErrorHttpException(string message) : base(ExceptionStatusCode, message)
        { }

        public InternalServerErrorHttpException(string message, Exception inner) : base(ExceptionStatusCode, message,
            inner)
        { }

        public InternalServerErrorHttpException() : base(ExceptionStatusCode, Message)
        { }

        protected InternalServerErrorHttpException(SerializationInfo info, StreamingContext context) : base(info,
            context)
        { }
    }
}
