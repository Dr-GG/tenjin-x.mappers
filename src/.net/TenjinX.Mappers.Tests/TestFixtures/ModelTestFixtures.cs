using TenjinX.Mappers.Tests.TestModels;

namespace TenjinX.Mappers.Tests.TestFixtures;

public static class ModelTestFixtures
{
    public static readonly TestModelA StartA = new()
    {
        Flag = true,
        Text = "InitialA",
        Number = 42
    };

    public static readonly TestModelB StartB = new()
    {
        Flag = false,
        Text = "InitialB",
        Number = 43
    };

    public static readonly TestModelC StartC = new()
    {
        Flag = true,
        Text = "InitialC",
        Number = 44
    };

    public static readonly TestModelD StartD = new()
    {
        Flag = false,
        Text = "InitialD",
        Number = 45
    };

    public static readonly TestModelB FromAToB = new()
    {
        Flag = false,
        Text = "InitialA_B",
        Number = 43
    };

    public static readonly TestModelA FromBToA = new()
    {
        Flag = true,
        Text = "InitialB_A",
        Number = 42
    };

    public static readonly TestModelC FromBToC = new()
    {
        Flag = false,
        Text = "InitialB_C",
        Number = 45
    };

    public static readonly TestModelB FromCToB = new()
    {
        Flag = true,
        Text = "InitialC_B",
        Number = 42
    };

    public static readonly TestModelD FromCToD = new()
    {
        Flag = false,
        Text = "InitialC_D",
        Number = 47
    };

    public static TestModelA StartAFromIndex(int index)
    {
        return new TestModelA
        {
            Flag = index % 2 == 0,
            Text = $"InitialA_{index}",
            Number = 42 + index
        };
    }

    public static TestModelB FromAToBFromIndex(int index)
    {
        return new TestModelB
        {
            Flag = index % 2 != 0,
            Text = $"InitialA_{index}_B",
            Number = 43 + index
        };
    }
}
