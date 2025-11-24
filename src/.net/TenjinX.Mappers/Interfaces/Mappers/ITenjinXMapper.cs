namespace TenjinX.Mappers.Interfaces.Mappers;

/// <summary>
/// A uni-directional mapper interface for mapping from TSource to TDestination.
/// </summary>
public interface ITenjinXMapper<TSource, TDestination>
{
    /// <summary>
    /// Maps the existing source object to the existing destination object.
    /// </summary>
    ITenjinXMapper<TSource, TDestination> Map(TSource source, TDestination destination, object? context = null);
}
