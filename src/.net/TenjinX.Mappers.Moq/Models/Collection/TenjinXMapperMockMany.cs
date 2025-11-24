using TenjinX.Mappers.Moq.Models.Context;

namespace TenjinX.Mappers.Moq.Models.Collection;

/// <summary>
/// The data structure for mocking many mappings in TenjinX mapper Moq.
/// </summary>
public class TenjinXMapperMockMany<TSource, TDestination> 
    where TSource : class
    where TDestination : class
{
    /// <summary>
    /// Gets or sets the source object.
    /// </summary>
    public TSource? Source { get; init; }

    /// <summary>
    /// Gets the destination object that was provided for mapping.
    /// </summary>
    public required TDestination ProvidedDestination { get; init; }

    /// <summary>
    /// Gets the destination object that will be mapped.
    /// </summary>
    public TDestination? IncomingDestination { get; init; }

    /// <summary>
    /// Gets the context object.
    /// </summary>
    public object? Context { get; init; }

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> to be used during mapping.
    /// </summary>
    public IServiceProvider? ServiceProvider { get; init; }

    /// <summary>
    /// Gets the callback to be invoked during mapping.
    /// </summary>
    public Action<TenjinXMapperMoqContext<TSource, TDestination>>? Callback { get; init; }
}
