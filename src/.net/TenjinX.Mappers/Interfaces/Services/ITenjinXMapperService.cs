namespace TenjinX.Mappers.Interfaces.Services;

/// <summary>
/// The global mapper service interface for mapping between objects of any types.
/// </summary>
public interface ITenjinXMapperService
{
    /// <summary>
    /// Gets the default <see cref="IServiceProvider"/> used by the mapper service.
    /// </summary>
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Maps the existing source object to the existing destination object using the default <see cref="IServiceProvider"/>.
    /// </summary>
    ITenjinXMapperService Map(object source, object destination, object? context = null);

    /// <summary>
    /// Maps the existing source object to the existing destination object using a provided <see cref="IServiceProvider"/>.
    /// </summary>
    ITenjinXMapperService Map(IServiceProvider serviceProvider, object source, object destination, object? context = null);
}
