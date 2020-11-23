using System;
using System.Net;
using System.Runtime.Serialization;

namespace Dashboard.API.Exceptions.Http
{
    public class InternalServerErrorHttpException : HttpException
    {
        private const HttpStatusCode ExceptionStatusCode = HttpStatusCode.InternalServerError;

        public InternalServerErrorHttpException(string message) : base(ExceptionStatusCode, message)
        { }

        public InternalServerErrorHttpException(string message, Exception inner) : base(ExceptionStatusCode, message,
            inner)
        { }

        public InternalServerErrorHttpException() : base(ExceptionStatusCode)
        { }

        protected InternalServerErrorHttpException(SerializationInfo info, StreamingContext context) : base(info,
            context)
        { }
    }
}
