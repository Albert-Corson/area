using System;
using System.Net;
using System.Runtime.Serialization;

namespace Dashboard.API.Exceptions.Http
{
    public class NotFoundHttpException : HttpException
    {
        private const HttpStatusCode ExceptionStatusCode = HttpStatusCode.NotFound;

        public NotFoundHttpException(string message) : base(ExceptionStatusCode, message)
        { }

        public NotFoundHttpException(string message, Exception inner) : base(ExceptionStatusCode, message, inner)
        { }

        public NotFoundHttpException() : base(ExceptionStatusCode)
        { }

        protected NotFoundHttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
