using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Tests.TestModels;

namespace TenjinX.Mappers.Tests.Implementations;

public class AToBMapper : ITenjinXMapper<TestModelA, TestModelB>
{
    public ITenjinXMapper<TestModelA, TestModelB> Map
    (
        TestModelA source,
        TestModelB destination,
        object? context = null
    )
    {
        destination.Text = $"{source.Text}_B";
        destination.Number = source.Number + 1;
        destination.Flag = !source.Flag;

        return this;
    }

    // This is an overload to test that the mapper selection logic
    public ITenjinXMapper<TestModelA, TestModelB> Map
    (
        TestModelA source
    )
    {
        throw new NotSupportedException();
    }

    // This is an overload to test that the mapper selection logic
    public ITenjinXMapper<TestModelA, TestModelB> Map
    (
        TestModelA source,
        TestModelB destination
    )
    {
        throw new NotSupportedException();
    }

    // This is an overload to test that the mapper selection logic
    public static void Map()
    {
        // No implementation needed for this test
    }

    public ITenjinXMapper<TestModelA, TestModelB> Map
    (
        TestModelA source,
        TestModelB destination,
        object context,
        object extraParam
    )
    {
        throw new NotSupportedException();
    }
}
