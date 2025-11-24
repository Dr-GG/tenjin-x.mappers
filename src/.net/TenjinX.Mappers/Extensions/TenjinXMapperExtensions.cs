using TenjinX.Extensions;
using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Models;

namespace TenjinX.Mappers.Extensions;

/// <summary>
/// A collection of extension methods for the <see cref="ITenjinXMapper{TSource, TDestination}"/> interface.
/// </summary>
public static class TenjinXMapperExtensions
{
    /// <summary>
    /// Maps the source to a new instance of the destination type.
    /// </summary>
    public static TDestination MapNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper, 
        TSource source,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapNew(source, (_) => new TDestination(), context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided async factory.
    /// </summary>
    public static TDestination MapNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper, 
        TSource source,
        Func<TenjinXMapperNewContext<TSource>, Task<TDestination>> destinationFactory,
        object? context = null
    )
    {
        return mapper.MapNew(source, (newContext) => destinationFactory(newContext).Result, context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided factory.
    /// </summary>
    public static TDestination MapNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper,a
        TSource source,
        Func<TenjinXMapperNewContext<TSource>, TDestination> destinationFactory,
        object? context = null
    )
    {
        var newContext = new TenjinXMapperNewContext<TSource>
        {
            Index = 0,
            Source = source,
            Context = context
        };
        var destination = destinationFactory(newContext);

        mapper.Map(source, destination, context);

        return destination;
    }

    /// <summary>
    /// Maps the source to the destination only if both are not null.
    /// </summary>
    public static ITenjinXMapper<TSource, TDestination> MapNullable<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper, 
        TSource? source, 
        TDestination? destination,
        object? context = null
    )
    {
        if (source is null || destination is null)
        {
            return mapper;
        }

        mapper.Map(source, destination, context);

        return mapper;
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type, returning null if the source is null.
    /// </summary>
    public static TDestination? MapNullableNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper, 
        TSource? source,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapNullableNew(source, (_) => new TDestination(), context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided async factory, returning null if the source is null.
    /// </summary>
    public static TDestination? MapNullableNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper, 
        TSource? source,
        Func<TenjinXMapperNewContext<TSource>, Task<TDestination>> destinationFactory,
        object? context = null
    )
    {
        return mapper.MapNullableNew(source, (newContext) => destinationFactory(newContext).Result, context);
    }

    /// <summary>
    /// Maps the source to a new instance of the destination type using the provided factory, returning null if the source is null.
    /// </summary>
    public static TDestination? MapNullableNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper,
        TSource? source,
        Func<TenjinXMapperNewContext<TSource>, TDestination> destinationFactory,
        object? context = null
    )
    {
        if (source is null)
        {
            return default;
        }

        var newContext = new TenjinXMapperNewContext<TSource>
        {
            Index = 0,
            Source = source,
            Context = context
        };
        var destination = destinationFactory(newContext);

        mapper.Map(source, destination, context);

        return destination;
    }

    /// <summary>
    /// Maps a collection of source items to a collection of destination items.
    /// </summary>
    /// <remarks>
    /// The <paramref name="destination"/> will be populated with new instances of <typeparamref name="TDestination"/>.
    /// </remarks>
    public static ITenjinXMapper<TSource, TDestination> MapManyNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper, 
        IEnumerable<TSource>? source, 
        ICollection<TDestination>? destination,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(source, destination, (_) => new TDestination(), context);
    }

    /// <summary>
    /// Maps a collection of source items to a collection of destination items using the provided async factory.
    /// </summary>
    /// <remarks>
    /// The <paramref name="destination"/> will be populated with new instances of <typeparamref name="TDestination"/>.
    /// </remarks>
    public static ITenjinXMapper<TSource, TDestination> MapManyNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper,
        IEnumerable<TSource>? source,
        ICollection<TDestination>? destination,
        Func<TenjinXMapperNewContext<TSource>, Task<TDestination>> destinationFactory,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(source, destination, (newContext) => destinationFactory(newContext).Result, context);
    }

    /// <summary>
    /// Maps a collection of source items to a collection of destination items using the provided factory.
    /// </summary>
    /// <remarks>
    /// The <paramref name="destination"/> will be populated with new instances of <typeparamref name="TDestination"/>.
    /// </remarks>
    public static ITenjinXMapper<TSource, TDestination> MapManyNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper,
        IEnumerable<TSource>? source,
        ICollection<TDestination>? destination,
        Func<TenjinXMapperNewContext<TSource>, TDestination> destinationFactory,
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
            var newContext = new TenjinXMapperNewContext<TSource>
            {
                Index = i,
                Source = item,
                Context = context
            };
            var destinationItem = destinationFactory(newContext);

            mapper.Map(item, destinationItem, context);

            destination.Add(destinationItem);
        }

        return mapper;
    }

    /// <summary>
    /// Maps a collection of source items to a new collection of destination items.
    /// </summary>
    public static IEnumerable<TDestination> MapManyNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper, 
        IEnumerable<TSource>? source,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(source, (_) => new TDestination(), context);
    }

    /// <summary>
    /// Maps a collection of source items to a new collection of destination items using the provided async factory.
    /// </summary>
    public static IEnumerable<TDestination> MapManyNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper,
        IEnumerable<TSource>? source,
        Func<TenjinXMapperNewContext<TSource>, Task<TDestination>> destinationFactory,
        object? context = null
    ) where TDestination : new()
    {
        return mapper.MapManyNew(source, (newContext) => destinationFactory(newContext).Result, context);
    }

    /// <summary>
    /// Maps a collection of source items to a new collection of destination items using the provided factory.
    /// </summary>
    public static IEnumerable<TDestination> MapManyNew<TSource, TDestination>
    (
        this ITenjinXMapper<TSource, TDestination> mapper,
        IEnumerable<TSource>? source,
        Func<TenjinXMapperNewContext<TSource>, TDestination> destinationFactory,
        object? context = null
    ) where TDestination : new()
    {
        if (source.IsNullOrEmpty())
        {
            return [];
        }

        var result = new List<TDestination>();

        mapper.MapManyNew(source, result, destinationFactory, context);

        return result;
    }
}
