using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Tests.TestModels;

namespace TenjinX.Mappers.Tests.Implementations;

public abstract class AbstractMapper : ITenjinXMapper<TestModelA, TestModelB>
{
    public abstract ITenjinXMapper<TestModelA, TestModelB> Map(TestModelA source, TestModelB destination, object? context = null);
}
