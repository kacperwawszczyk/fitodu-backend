using Xunit;

namespace Scheduledo.Test
{
    [CollectionDefinition(nameof(IntegrationTestCollection))]
    public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
    {
    }
}
