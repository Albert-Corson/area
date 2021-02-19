using System.Net;
using Area.AcceptanceTests.Models;
using Area.AcceptanceTests.Models.Responses;
using Xunit;

namespace Area.AcceptanceTests.Utilities
{
    public static class AssertExtension
    {
        public static void FailedApiResponse(ResponseHolder<StatusModel> result, HttpStatusCode expectedStatus)
        {
            Assert.Equal(expectedStatus, result.Status);
            Assert.False(result.Content.Successful);
            Assert.NotNull(result.Content.Error);
        }

        public static void FailedApiResponse<TData>(ResponseHolder<ResponseModel<TData>> result, HttpStatusCode expectedStatus)
            where TData : class
        {
            FailedApiResponse(new ResponseHolder<StatusModel> {
                Content = result.Content,
                Status = result.Status
            }, expectedStatus);
            Assert.Null(result.Content.Data);
        }

        public static void SuccessfulApiResponse(ResponseHolder<StatusModel> result, HttpStatusCode expectedStatus = HttpStatusCode.OK)
        {
            Assert.Equal(expectedStatus, result.Status);
            Assert.True(result.Content.Successful);
            Assert.Null(result.Content.Error);
        }

        public static void SuccessfulApiResponse<TData>(ResponseHolder<ResponseModel<TData>> result, HttpStatusCode expectedStatus = HttpStatusCode.OK)
            where TData : class
        {
            SuccessfulApiResponse(new ResponseHolder<StatusModel> {
                Content = result.Content,
                Status = result.Status
            }, expectedStatus);
            Assert.NotNull(result.Content.Data);
        }
    }
}