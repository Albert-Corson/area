using System;
using System.Net;
using System.Runtime.Serialization;

namespace Area.API.Exceptions.Http
{
    public class ForbiddenHttpException : HttpException
    {
        private const HttpStatusCode ExceptionStatusCode = HttpStatusCode.Forbidden;

        private new const string Message = "Forbidden";

        public ForbiddenHttpException(string message) : base(ExceptionStatusCode, message)
        { }

        public ForbiddenHttpException(string message, Exception inner) : base(ExceptionStatusCode, message, inner)
        { }

        public ForbiddenHttpException() : base(ExceptionStatusCode, Message)
        { }

        protected ForbiddenHttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}