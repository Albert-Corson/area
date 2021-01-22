using System;
using System.Net;
using System.Runtime.Serialization;

namespace Area.API.Exceptions.Http
{
    public class ConflictHttpException : HttpException
    {
        private const HttpStatusCode ExceptionStatusCode = HttpStatusCode.Conflict;

        private new const string Message = "Conflict with other resource(s)";

        public ConflictHttpException(string message) : base(ExceptionStatusCode, message)
        { }

        public ConflictHttpException(string message, Exception inner) : base(ExceptionStatusCode, message, inner)
        { }

        public ConflictHttpException() : base(ExceptionStatusCode, Message)
        { }

        protected ConflictHttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
