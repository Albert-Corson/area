using Area.AcceptanceTests.Utilities;
using Xunit;

namespace Area.AcceptanceTests.Tests
{
    public class AboutDoJsonTest
    {
        private readonly AreaApi _areaApi = new AreaApi();

        [Fact]
        public async void ModelFormat()
        {
            var response = await _areaApi.AboutDotJson();

            AssertExtension.SuccessfulApiResponse(response);
        }
    }
}