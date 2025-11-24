using Moq;
using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Moq.Models.Collection;
using TenjinX.Mappers.Moq.Models.Context;
using TenjinX.Mappers.Moq.Utilities;

namespace TenjinX.Mappers.Moq.Extensions;

/// <summary>
/// The collection of extension methods for mocking TenjinX mappers with Moq.
/// </summary>
public static class TenjinXMapperMoqExtensions
{
    /// <summary>
    /// Mocks the <see cref="ITenjinXMapper{TSource, TDestination}.Map(TSource, TDestination, object?)"/> method.
    /// </summary>
    public static Mock<ITenjinXMapper<TSource, TDestination>> SetupMap<TSource, TDestination>
    (
        this Mock<ITenjinXMapper<TSource, TDestination>> mockMapper,
        TDestination returnDestination,
        TSource? source = null,
        TDestination? destination = null,
        object? context = null,
        Action<TenjinXMapperMoqContext<TSource, TDestination>>? callback = null
    )
        where TSource : class
        where TDestination : class
    {
        mockMapper
            .Setup
            (
                m => m.Map
                (
                    It.Is<TSource>(s => source == null || s.Equals(source)),
                    It.Is<TDestination>(d => destination == null || d.Equals(destination)),
                    It.Is<object?>(c => context == null || (c != null && c.Equals(context)))
                )
            )
            .Callback(TenjinXMapperMoqUtilities.GetTenjinXMapperMoqCallback(returnDestination, callback))
            .Returns(mockMapper.Object);

        return mockMapper;
    }

    /// <summary>
    /// Mocks the <see cref="ITenjinXMapper{TSource, TDestination}.Map(TSource, TDestination, object?)"/> method.
    /// </summary>
    public static Mock<ITenjinXMapper<TSource, TDestination>> SetupMapNew<TSource, TDestination>
    (
        this Mock<ITenjinXMapper<TSource, TDestination>> mockMapper,
        TSource? source = null,
        TDestination? destination = null,
        object? context = null,
        Action<TenjinXMapperMoqContext<TSource, TDestination>>? callback = null
    )
        where TSource : class
        where TDestination : class, new()
    {
        return mockMapper.SetupMap(new TDestination(), source, destination, context, callback);
    }

    /// <summary>
    /// Mocks the <see cref="ITenjinXMapper{TSource, TDestination}.Map(TSource, TDestination, object?)"/> method when using the MapMany methods.
    /// </summary>
    public static Mock<ITenjinXMapper<TSource, TDestination>> SetupMapManyNew<TSource, TDestination>
    (
        this Mock<ITenjinXMapper<TSource, TDestination>> mockMapper,
        params TenjinXMapperMockMany<TSource, TDestination>[] mappings
    )
        where TSource : class
        where TDestination : class
    {
        foreach (var mapping in mappings)
        {
            mockMapper.SetupMap
            (
                mapping.ProvidedDestination,
                mapping.Source,
                mapping.IncomingDestination,
                mapping.Context,
                mapping.Callback
            );
        }

        return mockMapper;
    }

    /// <summary>
    /// Verifies the <see cref="ITenjinXMapper{TSource, TDestination}.Map(TSource, TDestination, object?)"/> method was called the expected number of times.
    /// </summary>
    public static Mock<ITenjinXMapper<TSource, TDestination>> VerifyMap<TSource, TDestination>
    (
        this Mock<ITenjinXMapper<TSource, TDestination>> mockMapper,
        Times times,
        TSource? source = null,
        TDestination? destination = null,
        object? context = null
    )
        where TSource : class
        where TDestination : class
    {
        mockMapper
            .Verify
            (
                m => m.Map
                (
                    It.Is<TSource>(s => source == null || s.Equals(source)),
                    It.Is<TDestination>(d => destination == null || d.Equals(destination)),
                    It.Is<object?>(c => context == null || (c != null && c.Equals(context)))
                ),
                times
            );

        return mockMapper;
    }

    /// <summary>
    /// Verifies the <see cref="ITenjinXMapper{TSource, TDestination}.Map(TSource, TDestination, object?)"/> method was called the expected number of times when using the MapMany methods.
    /// </summary>
    public static Mock<ITenjinXMapper<TSource, TDestination>> VerifyMapMany<TSource, TDestination>
    (
        this Mock<ITenjinXMapper<TSource, TDestination>> mockMapper,
        Times times,
        params TenjinXMapperMockMany<TSource, TDestination>[] mappings
    )
        where TSource : class
        where TDestination : class
    {
        foreach (var mapping in mappings)
        {
            mockMapper.VerifyMap
            (
                times,
                mapping.Source,
                mapping.IncomingDestination,
                mapping.Context
            );
        }

        return mockMapper;
    }
}
