using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Tests.TestModels;

namespace TenjinX.Mappers.Tests.Implementations;

public class BToAMapper : ITenjinXMapper<TestModelB, TestModelA>
{
    public ITenjinXMapper<TestModelB, TestModelA> Map
    (
        TestModelB source,
        TestModelA destination,
        object? context = null
    )
    {
        destination.Text = $"{source.Text}_A";
        destination.Number = source.Number - 1;
        destination.Flag = !source.Flag;

        return this;
    }
}
