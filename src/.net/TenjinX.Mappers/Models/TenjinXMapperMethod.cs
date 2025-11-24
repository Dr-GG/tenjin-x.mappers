using System.Reflection;

namespace TenjinX.Mappers.Models;

/// <summary>
/// The model representing a mapper type.
/// </summary>
public class TenjinXMapperMethod
{
    /// <summary>
    /// Gets the unique key associated with the mapper.
    /// </summary>
    public required string Key { get; init; }

    /// <summary>
    /// Gets the mapper <see cref="Type"/>.
    /// </summary>
    public required Type MapperType { get; init; }

    /// <summary>
    /// Compiled delegate wrapper for fast invocation: (object source, object destination, object? context) => mapper.Map((TSource)source, (TDestination)destination, context)
    /// </summary>
    public required Action<object, object, object, object?> MapAction { get; init; }
}
