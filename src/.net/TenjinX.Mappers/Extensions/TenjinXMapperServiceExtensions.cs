using TenjinX.Extensions;
using TenjinX.Mappers.Exceptions;
using TenjinX.Mappers.Interfaces.Services;
using TenjinX.Mappers.Models;

namespace TenjinX.Mappers.Extensions;

/// <summary>
/// The collection of extension methods for the <see cref="ITenjinXMapperService"/>.
/// </summary>
public static class TenjinXMapperServiceExtensions
{
    /// <summary>
    /// Maps the source to a new instance of the destination type with the default service provider.
    /// </summary>
    public static TDestination MapNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        object source,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapNew<TDestination>(mapper.ServiceProvider, source, context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    public static TDestination MapNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        object source,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapNew(serviceProvider, source, (_) => new TDestination(), context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided async factory.
    /// </summary>
    public static TDestination MapNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        object source,
        Func<TenjinXMapperNewContext<object>, Task<TDestination>> destinationFactory,
        object? context = null
    )
    {
        return mapper.MapNew(mapper.ServiceProvider, source, destinationFactory, context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided async factory with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    public static TDestination MapNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        object source,
        Func<TenjinXMapperNewContext<object>, Task<TDestination>> destinationFactory,
        object? context = null
    )
    {
        return mapper.MapNew(serviceProvider, source, (newContext) => destinationFactory(newContext).Result, context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided factory and the default <see cref="IServiceProvider"/>.
    /// </summary>
    public static TDestination MapNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        object source,
        Func<TenjinXMapperNewContext<object>, TDestination> destinationFactory,
        object? context = null
    )
    {
        return mapper.MapNew(mapper.ServiceProvider, source, destinationFactory, context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided factory with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    public static TDestination MapNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        object source,
        Func<TenjinXMapperNewContext<object>, TDestination> destinationFactory,
        object? context = null
    )
    {
        var newContext = new TenjinXMapperNewContext<object>
        {
            Index = 0,
            Source = source,
            Context = context
        };
        var destination = destinationFactory(newContext) 
            ?? throw new TenjinXMapperException("The destination factory returned null when creating a new destination instance.");

        mapper.Map(serviceProvider, source, destination, context);

        return destination;
    }

    /// <summary>
    /// Maps the source to the destination only if both are not null with the default <see cref="IServiceProvider"/>.
    /// </summary>
    public static ITenjinXMapperService MapNullable<TDestination>
    (
        this ITenjinXMapperService mapper,
        object? source,
        TDestination? destination,
        object? context = null
    )
    {
        return mapper.MapNullable(mapper.ServiceProvider, source, destination, context);
    }

    /// <summary>
    /// Maps the source to the destination only if both are not null with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    public static ITenjinXMapperService MapNullable<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        object? source,
        TDestination? destination,
        object? context = null
    )
    {
        if (source is null || destination is null)
        {
            return mapper;
        }

        mapper.Map(serviceProvider, source, destination, context);

        return mapper;
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type, returning null if the source is null with the default <see cref="IServiceProvider"/>.
    /// </summary>
    public static TDestination? MapNullableNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        object? source,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapNullableNew<TDestination>(mapper.ServiceProvider, source, context);
    }

    /// <summary> 
    /// Maps the source to a new instance of the destination type, returning null if the source is null with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    public static TDestination? MapNullableNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        object? source,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapNullableNew(serviceProvider, source, (_) => new TDestination(), context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided async factory, returning null if the source is null with the default <see cref="IServiceProvider"/>.
    /// </summary>
    public static TDestination? MapNullableNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        object? source,
        Func<TenjinXMapperNewContext<object>, Task<TDestination>> destinationFactory,
        object? context = null
    )
    {
        return mapper.MapNullableNew(mapper.ServiceProvider, source, destinationFactory, context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided async factory, returning null if the source is null with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    public static TDestination? MapNullableNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        object? source,
        Func<TenjinXMapperNewContext<object>, Task<TDestination>> destinationFactory,
        object? context = null
    )
    {
        return mapper.MapNullableNew(serviceProvider, source, (newContext) => destinationFactory(newContext).Result, context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided factory, returning null if the source is null with the default <see cref="IServiceProvider"/>.
    /// </summary>
    public static TDestination? MapNullableNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        object? source,
        Func<TenjinXMapperNewContext<object>, TDestination> destinationFactory,
        object? context = null
    )
    {
        return mapper.MapNullableNew(mapper.ServiceProvider, source, destinationFactory, context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided factory, returning null if the source is null with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    public static TDestination? MapNullableNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        object? source,
        Func<TenjinXMapperNewContext<object>, TDestination> destinationFactory,
        object? context = null
    )
    {
        if (source is null)
        {
            return default;
        }

        var newContext = new TenjinXMapperNewContext<object>
        {
            Index = 0,
            Source = source,
            Context = context
        };
        var destination = destinationFactory(newContext);

        if (destination is null)
        {
            return default;
        }

        mapper.Map(serviceProvider, source, destination, context);

        return destination;
    }

    /// <summary>
    /// Maps a collection of source items to a collection of destination items with the default <see cref="IServiceProvider"/>.
    /// </summary>
    /// <remarks>
    /// The <paramref name="destination"/> will be populated with new instances of <typeparamref name="TDestination"/>.
    /// </remarks>
    public static ITenjinXMapperService MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IEnumerable<object>? source,
        ICollection<TDestination>? destination,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(mapper.ServiceProvider, source, destination, context);
    }

    /// <summary>
    /// Maps a collection of source items to a collection of destination items with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    /// <remarks>
    /// The <paramref name="destination"/> will be populated with new instances of <typeparamref name="TDestination"/>.
    /// </remarks>
    public static ITenjinXMapperService MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        IEnumerable<object>? source,
        ICollection<TDestination>? destination,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(serviceProvider, source, destination, (_) => new TDestination(), context);
    }

    /// <summary>
    /// Maps a collection of source items to a collection of destination items using the provided async factory with the default <see cref="IServiceProvider"/>.
    /// </summary>
    /// <remarks>
    /// The <paramref name="destination"/> will be populated with new instances of <typeparamref name="TDestination"/>.
    /// </remarks>
    public static ITenjinXMapperService MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IEnumerable<object>? source,
        ICollection<TDestination>? destination,
        Func<TenjinXMapperNewContext<object>, Task<TDestination>> destinationFactory,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(mapper.ServiceProvider, source, destination, destinationFactory, context);
    }

    /// <summary>
    /// Maps a collection of source items to a collection of destination items using the provided async factory with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    /// <remarks>
    /// The <paramref name="destination"/> will be populated with new instances of <typeparamref name="TDestination"/>.
    /// </remarks>
    public static ITenjinXMapperService MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        IEnumerable<object>? source,
        ICollection<TDestination>? destination,
        Func<TenjinXMapperNewContext<object>, Task<TDestination>> destinationFactory,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(serviceProvider, source, destination, (newContext) => destinationFactory(newContext).Result, context);
    }

    /// <summary>
    /// Maps a collection of source items to a collection of destination items using the provided factory with the default <see cref="IServiceProvider"/>.
    /// </summary>
    /// <remarks>
    /// The <paramref name="destination"/> will be populated with new instances of <typeparamref name="TDestination"/>.
    /// </remarks>
    public static ITenjinXMapperService MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IEnumerable<object>? source,
        ICollection<TDestination>? destination,
        Func<TenjinXMapperNewContext<object>, TDestination> destinationFactory,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(mapper.ServiceProvider, source, destination, destinationFactory, context);
    }

    /// <summary>
    /// Maps a collection of source items to a collection of destination items using the provided factory with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    /// <remarks>
    /// The <paramref name="destination"/> will be populated with new instances of <typeparamref name="TDestination"/>.
    /// </remarks>
    public static ITenjinXMapperService MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        IEnumerable<object>? source,
        ICollection<TDestination>? destination,
        Func<TenjinXMapperNewContext<object>, TDestination> destinationFactory,
        object? context = null
    ) where TDestination : new()
    {
        if (source.IsNullOrEmpty() || destination == null)
        {
            return mapper;
        }

        var enumeratedList = source.EnumerateToList();

        for (var i = 0; i < enumeratedList.Count; ++i)
        {
            var item = enumeratedList[i];
            var newContext = new TenjinXMapperNewContext<object>
            {
                Index = i,
                Source = item,
                Context = context
            };
            var destinationItem = destinationFactory(newContext) 
                ?? throw new TenjinXMapperException("The destination factory returned null when creating a new destination instance.");

            mapper.Map(serviceProvider, item, destinationItem, context);
            destination.Add(destinationItem);
        }

        return mapper;
    }

    /// <summary>
    /// Maps a collection of source items to a new collection of destination items with the default <see cref="IServiceProvider"/>.
    /// </summary>
    public static IEnumerable<TDestination> MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IEnumerable<object>? source,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew<TDestination>(mapper.ServiceProvider, source, context);
    }

    /// <summary>
    /// Maps a collection of source items to a new collection of destination items with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    public static IEnumerable<TDestination> MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        IEnumerable<object>? source,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(serviceProvider, source, (_) => new TDestination(), context);
    }

    /// <summary>
    /// Maps a collection of source items to a new collection of destination items using the provided async factory with the default <see cref="IServiceProvider"/>.
    /// </summary>
    public static IEnumerable<TDestination> MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IEnumerable<object>? source,
        Func<TenjinXMapperNewContext<object>, Task<TDestination>> destinationFactory,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(mapper.ServiceProvider, source, destinationFactory, context);
    }

    /// <summary>
    /// Maps a collection of source items to a new collection of destination items using the provided async factory with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    public static IEnumerable<TDestination> MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        IEnumerable<object>? source,
        Func<TenjinXMapperNewContext<object>, Task<TDestination>> destinationFactory,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(serviceProvider, source, (newContext) => destinationFactory(newContext).Result, context);
    }

    /// <summary>
    /// Maps a collection of source items to a new collection of destination items using the provided factory with the default <see cref="IServiceProvider"/>.
    /// </summary>
    public static IEnumerable<TDestination> MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IEnumerable<object>? source,
        Func<TenjinXMapperNewContext<object>, TDestination> destinationFactory,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(mapper.ServiceProvider, source, destinationFactory, context);
    }

    /// <summary>
    /// Maps a collection of source items to a new collection of destination items using the provided factory with a specified <see cref="IServiceProvider"/>.
    /// </summary>
    public static IEnumerable<TDestination> MapManyNew<TDestination>
    (
        this ITenjinXMapperService mapper,
        IServiceProvider serviceProvider,
        IEnumerable<object>? source,
        Func<TenjinXMapperNewContext<object>, TDestination> destinationFactory,
        object? context = null
    ) where TDestination : new()
    {
        if (source.IsNullOrEmpty())
        {
            return [];
        }

        var result = new List<TDestination>();

        mapper.MapManyNew(serviceProvider, source, result, destinationFactory, context);

        return result;
    }
}
