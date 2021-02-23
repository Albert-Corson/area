using System.Net;
using Area.AcceptanceTests.Models.Responses;

namespace Area.AcceptanceTests.Models
{
    public class ResponseHolder<TData> where TData : StatusModel
    {
        public HttpStatusCode Status { get; set; }
        public bool Successful => (int) Status >= 200 && (int) Status < 300;
        public TData Content { get; set; } = default!;
    }
}