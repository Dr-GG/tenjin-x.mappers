using TenjinX.Mappers.Moq.Tests.Models;

namespace TenjinX.Mappers.Moq.Tests.Fixtures;

public static class TestModelTestFixtures
{
    public static TestSourceModel EmptyTestSourceModel => new();

    public static TestSourceModel DefaultTestSourceModel => new()
    {
        PropertyOne = "Source Property One",
        PropertyTwo = "Source Property Two"
    };

    public static TestSourceModel RandomTestSourceModel => new()
    {
        PropertyOne = Guid.NewGuid().ToString(),
        PropertyTwo = Guid.NewGuid().ToString()
    };

    public static TestDestinationModel EmptyTestDestinationModel => new();

    public static TestDestinationModel DefaultTestDestinationModel => new()
    {
        Property01 = "Source Property One",
        Property02 = "Source Property Two"
    };

    public static TestDestinationModel RandomTestDestinationModel => new()
    {
        Property01 = Guid.NewGuid().ToString(),
        Property02 = Guid.NewGuid().ToString()
    };
}
