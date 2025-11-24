using Moq;
using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Interfaces.Services;
using TenjinX.Mappers.Moq.Models.Collection;
using TenjinX.Mappers.Moq.Models.Context;
using TenjinX.Mappers.Moq.Utilities;

namespace TenjinX.Mappers.Moq.Extensions;

/// <summary>
/// The collection of extension methods for mocking <see cref="ITenjinXMapperService"/> with Moq.
/// </summary>
public static class TenjinXMapperServiceMoqExtensions
{
    /// <summary>
    /// Mocks the <see cref="ITenjinXMapperService.Map(IServiceProvider, object, object, object?)"/> method.
    /// </summary>
    public static Mock<ITenjinXMapperService> SetupMap<TSource, TDestination>
    (
        this Mock<ITenjinXMapperService> mockMapperService,
        TDestination returnDestination,
        TSource? source = null,
        TDestination? destination = null,
        IServiceProvider? serviceProvider = null,
        object? context = null,
        Action<TenjinXMapperMoqContext<TSource, TDestination>>? callback = null
    )
        where TSource : class
        where TDestination : class
    {
        mockMapperService
            .Setup
            (
                ms => ms.Map
                (
                    It.Is<IServiceProvider>(sp => serviceProvider == null || sp.Equals(serviceProvider)),
                    It.Is<object>(s => source == null || s.Equals(source)),
                    It.Is<object>(d => destination == null || d.Equals(destination)),
                    It.Is<object?>(c => context == null || (c != null && c.Equals(context)))
                )
            )
            .Callback(TenjinXMapperMoqUtilities.GetTenjinXMapperServiceMoqCallback<TSource, TDestination>(returnDestination, callback))
            .Returns(mockMapperService.Object);

        return mockMapperService;
    }

    /// <summary>
    /// Mocks the <see cref="ITenjinXMapperService"/> method.
    /// </summary>
    public static Mock<ITenjinXMapperService> SetupMapNew<TSource, TDestination>
    (
        this Mock<ITenjinXMapperService> mockMapperService,
        TSource? source = null,
        TDestination? destination = null,
        IServiceProvider? serviceProvider = null,
        object? context = null,
        Action<TenjinXMapperMoqContext<TSource, TDestination>>? callback = null
    )
        where TSource : class
        where TDestination : class, new()
    {
        return mockMapperService.SetupMap(new TDestination(), source, destination, serviceProvider, context, callback);
    }

    /// <summary>
    /// Mocks the <see cref="ITenjinXMapperService"/> method when using the MapMany methods.
    /// </summary>
    public static Mock<ITenjinXMapperService> SetupMapManyNew<TSource, TDestination>
    (
        this Mock<ITenjinXMapperService> mockMapperService,
        params TenjinXMapperMockMany<TSource, TDestination>[] mappings
    )
        where TSource : class
        where TDestination : class
    {
        foreach (var mapping in mappings)
        {
            mockMapperService.SetupMap
            (
                mapping.ProvidedDestination,
                mapping.Source,
                mapping.IncomingDestination,
                mapping.ServiceProvider,
                mapping.Context,
                mapping.Callback
            );
        }

        return mockMapperService;
    }

    /// <summary>
    /// Verifies the <see cref="ITenjinXMapperService.Map(IServiceProvider, object, object, object?)"/> method was called the expected number of times.
    /// </summary>
    public static Mock<ITenjinXMapperService> VerifyMap<TSource, TDestination>
    (
        this Mock<ITenjinXMapperService> mockMapperService,
        Times times,
        TSource? source = null,
        TDestination? destination = null,
        IServiceProvider? serviceProvider = null,
        object? context = null
    )
        where TSource : class
        where TDestination : class
    {
        mockMapperService
            .Verify
            (
                m => m.Map
                (
                    It.Is<IServiceProvider>(sp => serviceProvider == null || sp.Equals(serviceProvider)),
                    It.Is<object>(s => source == null || s.Equals(source)),
                    It.Is<object>(d => destination == null || d.Equals(destination)),
                    It.Is<object?>(c => context == null || (c != null && c.Equals(context)))
                ),
                times
            );

        return mockMapperService;
    }

    /// <summary>
    /// Verifies the <see cref="ITenjinXMapperService.Map(IServiceProvider, object, object, object?)"/> method was called the expected number of times when using the MapMany methods.
    /// </summary>
    public static Mock<ITenjinXMapperService> VerifyMapMany<TSource, TDestination>
    (
        this Mock<ITenjinXMapperService> mockMapperService,
        Times times,
        params TenjinXMapperMockMany<TSource, TDestination>[] mappings
    )
        where TSource : class
        where TDestination : class
    {
        foreach (var mapping in mappings)
        {
            mockMapperService.VerifyMap
            (
                times,
                mapping.Source,
                mapping.IncomingDestination,
                mapping.ServiceProvider,
                mapping.Context
            );
        }

        return mockMapperService;
    }
}
