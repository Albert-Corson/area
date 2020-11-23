using System;
using System.Net;
using System.Runtime.Serialization;

namespace Dashboard.API.Exceptions.Http
{
    public class BadRequestHttpException : HttpException
    {
        private const HttpStatusCode ExceptionStatusCode = HttpStatusCode.BadRequest;

        public BadRequestHttpException(string message) : base(ExceptionStatusCode, message)
        { }

        public BadRequestHttpException(string message, Exception inner) : base(ExceptionStatusCode, message, inner)
        { }

        public BadRequestHttpException() : base(ExceptionStatusCode)
        { }

        protected BadRequestHttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
