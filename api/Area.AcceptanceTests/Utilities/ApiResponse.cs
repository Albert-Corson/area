using System.Net;

namespace Area.AcceptanceTests.Utilities
{
    public class ApiResponse<TData>
    {
        public HttpStatusCode Status { get; set; }
        public bool Successful => (int) Status >= 200 && (int) Status < 300;
        public TData Data { get; set; } = default!;
    }
}