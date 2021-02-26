using System.Threading.Tasks;
using Area.AcceptanceTests.Collections;
using Area.AcceptanceTests.Utilities;
using Xunit;

namespace Area.AcceptanceTests.Tests
{
    [Collection(nameof(AreaCollection))]
    public class AboutDoJsonTest
    {
        [Fact]
        public async Task ModelFormat()
        {
            var response = await new AreaApi().AboutDotJson();

            AssertExtension.SuccessfulApiResponse(response);
        }
    }
}