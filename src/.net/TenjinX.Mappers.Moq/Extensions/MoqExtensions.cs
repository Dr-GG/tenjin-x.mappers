using Moq;
using TenjinX.Mappers.Interfaces.Mappers;

namespace TenjinX.Mappers.Moq.Extensions;

public static class MoqExtensions
{
    public static Mock<ITenjinXMapper<TSource, TDestination>> SetupMapMew<TSource, TDestination>
    (
        this Mock<ITenjinXMapper<TSource, TDestination>> mockMapper,
        TSource source,
        TDestination destination
    )
        where TSource : class
        where TDestination : class
    {
        mockMapper
            .Setup(m => m.Map
            (
                It.Is<TSource>(s => s == source), 
                It.IsAny<TDestination>(), 
                It.IsAny<object?>())
            )
            .Returns(destination);

        return mockMapper;
    }
}
