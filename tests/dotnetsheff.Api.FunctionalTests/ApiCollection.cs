using Xunit;

namespace dotnetsheff.Api.FunctionalTests
{
    [CollectionDefinition(XUnitCollectionNames.ApiCollection)]
    public class ApiCollection : ICollectionFixture<AzureFunctionsFixture>
    {
        // Errr... xunit is weird.
    }
}
