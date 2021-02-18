using Xunit;

namespace Area.AcceptanceTests.Collections
{
    [CollectionDefinition(nameof(ApiTestCollection), DisableParallelization = true)]
    public class ApiTestCollection : ICollectionFixture<AreaApiClient>
    { }
}