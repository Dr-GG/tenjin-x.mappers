namespace TenjinX.Mappers.Models;

/// <summary>
/// The data context provided when mapping a new instance of a type.
/// </summary>
/// <typeparam name="TSource"></typeparam>
public record TenjinXMapperNewContext<TSource>
{
    /// <summary>
    /// Gets the index of the current item being mapped in a collection, if applicable.
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// Gets the source object to be mapped.
    /// </summary>
    public required TSource Source { get; init; }

    /// <summary>
    /// Gets the context object associated with the mapping operation.
    /// </summary>
    public required object? Context { get; init; }
}
