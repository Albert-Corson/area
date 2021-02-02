using System;
using System.Net;
using System.Runtime.Serialization;

namespace Area.API.Exceptions.Http
{
    public class BadRequestHttpException : HttpException
    {
        private const HttpStatusCode ExceptionStatusCode = HttpStatusCode.BadRequest;

        private new const string Message = "Bad request";

        public BadRequestHttpException(string message) : base(ExceptionStatusCode, message)
        { }

        public BadRequestHttpException(string message, Exception inner) : base(ExceptionStatusCode, message, inner)
        { }

        public BadRequestHttpException() : base(ExceptionStatusCode, Message)
        { }

        protected BadRequestHttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}