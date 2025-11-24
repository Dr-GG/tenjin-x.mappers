using TenjinX.Mappers.Moq.Models.Context;
using TenjinX.Mappers.Moq.Services.Cache;

namespace TenjinX.Mappers.Moq.Utilities;

/// <summary>
/// A collection of utilities for TenjinX mapper Moq.
/// </summary>
internal static class TenjinXMapperMoqUtilities
{
    /// <summary>
    /// The default callbacks for the mocked mappers.
    /// </summary>
    internal static void DefaultCallbacks<TSource, TDestination>(TenjinXMapperMoqContext<TSource, TDestination> context)
        where TSource : class
        where TDestination : class
    {
        var properties = TenjinXMapperMoqCallbackCache.Instance.GetTypeProperties(typeof(TDestination));

        foreach (var property in properties)
        {
            var providedPropertyValue = property.GetValue(context.ProvidedDestination);

            property.SetValue(context.IncomingDestination, providedPropertyValue);
        }
    }

    /// <summary>
    /// Gets the TenjinX mapper Moq callback.
    /// </summary>
    internal static Action<TSource, TDestination, object?> GetTenjinXMapperMoqCallback<TSource, TDestination>
    (
        TDestination providedDestination,
        Action<TenjinXMapperMoqContext<TSource, TDestination>>? callback
    )
        where TSource : class
        where TDestination : class
    {
        return (source, destination, context) =>
        {
            var moqContext = new TenjinXMapperMoqContext<TSource, TDestination>
            {
                SourceObject = source,
                ProvidedDestinationObject = providedDestination,
                IncomingDestinationObject = destination
            };

            if (callback == null)
            {
                DefaultCallbacks(moqContext);
            }
            else
            {
                callback(moqContext);
            }
        };
    }

    internal static Action<IServiceProvider, object, object, object?> GetTenjinXMapperServiceMoqCallback<TSource, TDestination>
    (
        TDestination providedDestination,
        Action<TenjinXMapperMoqContext<TSource, TDestination>>? callback
    )
        where TSource : class
        where TDestination : class
    {
        return (serviceProvider, source, destination, ctx) =>
        {
            var moqContext = new TenjinXMapperMoqContext<TSource, TDestination>
            {
                SourceObject = (TSource)source,
                ServiceProvider = serviceProvider,
                ProvidedDestinationObject = providedDestination,
                IncomingDestinationObject = (TDestination)destination
            };

            if (callback == null)
            {
                DefaultCallbacks(moqContext);
            }
            else
            {
                callback(moqContext);
            }
        };
    }
}
