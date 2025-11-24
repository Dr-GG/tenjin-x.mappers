using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Tests.TestModels;

namespace TenjinX.Mappers.Tests.Implementations;

public class BToCAndCToBMapper : ITenjinXMapper<TestModelB, TestModelC>, ITenjinXMapper<TestModelC, TestModelB>
{
    public ITenjinXMapper<TestModelB, TestModelC> Map
    (
        TestModelB source,
        TestModelC destination,
        object? context = null
    )
    {
        destination.Text = $"{source.Text}_C";
        destination.Number = source.Number + 2;
        destination.Flag = source.Flag;

        return this;
    }

    public ITenjinXMapper<TestModelC, TestModelB> Map
    (
        TestModelC source,
        TestModelB destination,
        object? context = null
    )
    {
        destination.Text = $"{source.Text}_B";
        destination.Number = source.Number - 2;
        destination.Flag = source.Flag;

        return this;
    }
}
