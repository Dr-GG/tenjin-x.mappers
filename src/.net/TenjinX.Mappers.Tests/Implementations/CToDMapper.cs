using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Tests.TestModels;

namespace TenjinX.Mappers.Tests.Implementations;

public class CToDMapper : ITenjinXMapper<TestModelC, TestModelD>
{
    public ITenjinXMapper<TestModelC, TestModelD> Map
    (
        TestModelC source,
        TestModelD destination,
        object? context = null
    )
    {
        destination.Text = $"{source.Text}_D";
        destination.Number = source.Number + 3;
        destination.Flag = !source.Flag;

        return this;
    }
}
