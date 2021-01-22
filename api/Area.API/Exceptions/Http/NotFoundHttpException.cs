using System;
using System.Net;
using System.Runtime.Serialization;

namespace Area.API.Exceptions.Http
{
    public class NotFoundHttpException : HttpException
    {
        private const HttpStatusCode ExceptionStatusCode = HttpStatusCode.NotFound;

        private new const string Message = "Resource not found";

        public NotFoundHttpException(string message) : base(ExceptionStatusCode, message)
        { }

        public NotFoundHttpException(string message, Exception inner) : base(ExceptionStatusCode, message, inner)
        { }

        public NotFoundHttpException() : base(ExceptionStatusCode, Message)
        { }

        protected NotFoundHttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
