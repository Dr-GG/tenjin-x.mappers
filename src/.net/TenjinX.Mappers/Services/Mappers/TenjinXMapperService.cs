using Microsoft.Extensions.DependencyInjection;
using TenjinX.Mappers.Exceptions;
using TenjinX.Mappers.Interfaces.Services;
using TenjinX.Mappers.Services.Registry;

namespace TenjinX.Mappers.Services.Mappers;

/// <summary>
/// The default implementation of the <see cref="ITenjinXMapperService"/> interface.
/// </summary>
public class TenjinXMapperService : ITenjinXMapperService
{
    private readonly TenjinXMapperReflectionRegistry _registry = new();
    private readonly IServiceProvider _serviceProvider;

    /// <inheritdoc />
    public IServiceProvider ServiceProvider => _serviceProvider;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public TenjinXMapperService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _registry.Preload(serviceProvider);
    }
    
    /// <inheritdoc />
    public ITenjinXMapperService Map(object source, object destination, object? context = null)
    {
        return Map(_serviceProvider, source, destination, context);
    }

    /// <inheritdoc />
    public ITenjinXMapperService Map
    (
        IServiceProvider serviceProvider,
        object source, 
        object destination, 
        object? context = null
    )
    {
        // Check for null values for projects where nullable reference types are not enabled
        if (source == null || destination == null)
        {
            throw new TenjinXMapperException($"{nameof(source)} and {nameof(destination)} objects cannot be null.");
        }

        var method = _registry.GetMappingMethod(source, destination);
        var mapper = serviceProvider.GetService(method.MapperType) 
            ?? throw new TenjinXMapperException($"Mapper of type '{method.MapperType.FullName}' could not be resolved from the service provider.");

        method.MapAction(mapper, source, destination, context);

        return this;
    }
}
