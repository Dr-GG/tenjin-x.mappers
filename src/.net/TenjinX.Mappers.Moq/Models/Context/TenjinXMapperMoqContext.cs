namespace TenjinX.Mappers.Moq.Models.Context;

/// <summary>
/// The base TenjinX mapper Moq context.
/// </summary>
public record TenjinXMapperMoqContext
{
    /// <summary>
    /// Gets the source object for the mapping operation.
    /// </summary>
    public object? SourceObject { get; init; }

    /// <summary>
    /// Gets the destination object provided by the unit test for the mapping operation.
    /// </summary>
    public object? ProvidedDestinationObject { get; init; }

    /// <summary>
    /// Gets the destination object that is incoming to be mapped to.
    /// </summary>
    public object? IncomingDestinationObject { get; init; }

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance.
    /// </summary>
    public IServiceProvider? ServiceProvider { get; init; }
}

/// <summary>
/// The TenjinX mapper Moq context with strongly typed source and destination objects.
/// </summary>
public record TenjinXMapperMoqContext<TSource, TDestination> : TenjinXMapperMoqContext
    where TSource : class
    where TDestination : class
{
    /// <summary>
    /// Gets the source object for the mapping operation.
    /// </summary>
    public TSource? Source
    {
        get => (TSource?)base.SourceObject;
    }

    /// <summary>
    /// Gets the destination object provided by the unit test for the mapping operation.
    /// </summary>
    public TDestination? ProvidedDestination
    {
        get => (TDestination?)base.ProvidedDestinationObject;
    }

    /// <summary>
    /// Gets the destination object that is incoming to be mapped to.
    /// </summary>
    public TDestination? IncomingDestination
    {
        get => (TDestination?)base.IncomingDestinationObject;
    }
}
