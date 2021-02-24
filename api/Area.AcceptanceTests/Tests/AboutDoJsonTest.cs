using Area.AcceptanceTests.Utilities;
using Xunit;

namespace Area.AcceptanceTests.Tests
{
    public class AboutDoJsonTest
    {
        [Fact]
        public async void ModelFormat()
        {
            var response = await new AreaApi().AboutDotJson();

            AssertExtension.SuccessfulApiResponse(response);
        }
    }
}