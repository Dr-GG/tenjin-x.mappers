using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TenjinX.Extensions;
using TenjinX.Mappers.Enums;
using TenjinX.Mappers.Exceptions;
using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Interfaces.Services;
using TenjinX.Mappers.Models;
using TenjinX.Mappers.Services.Mappers;

namespace TenjinX.Mappers.Extensions;

/// <summary>
/// The collection of extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// The collection of <see cref="Assembly"/> instances to preload for Tenjin-X Mapper implementations.
    /// </summary>
    internal static readonly ICollection<Assembly> TenjinXMapperPreloadAssemblies = [];

    /// <summary>
    /// Adds a collection of <see cref="Assembly"/> instances to preload for Tenjin-X Mapper implementations.
    /// </summary>
    public static IServiceCollection AddTenjinXMapperPreloadAssemblies
    (
        this IServiceCollection services, 
        params Assembly[] assemblies
    )
    {
        assemblies
            .Where(a => TenjinXMapperPreloadAssemblies.DoesNotContain(a))
            .ForEach(a => TenjinXMapperPreloadAssemblies.Add(a));

        return services;
    }

    /// <summary>
    /// Adds all <see cref="ITenjinXMapper{TSource, TDestination}"/> and <see cref="ITenjinXDualMapper{TLeft, TRight}"/> implementations from the specified assembly to the service collection.
    /// </summary>
    public static IServiceCollection AddTenjinXMappers
    (
        this IServiceCollection services,
        Assembly assembly,
        TenjinXMapperScope scope = TenjinXMapperScope.Singleton,
        bool preloadAssembly = true
    )
    {
        AddMapperTypesFromAssebly
        (
            services,
            scope,
            assembly
        );

        if (preloadAssembly)
        {
            services.AddTenjinXMapperPreloadAssemblies(assembly);
        }

        return services;
    }

    /// <summary>
    /// Adds the <see cref="ITenjinXMapperService"/> implementation to the service collection.
    /// </summary>
    public static IServiceCollection AddTenjinXMapperService
    (
        this IServiceCollection services,
        TenjinXMapperScope scope = TenjinXMapperScope.Singleton
    )
    {
        AddMapperType
        (
            services,
            scope,
            typeof(ITenjinXMapperService),
            typeof(TenjinXMapperService)
        );

        return services;
    }

    /// <summary>
    /// Adds all <see cref="ITenjinXMapper{TSource, TDestination}"/> implementations from the specified assembly and the <see cref="ITenjinXMapperService"/> implementation to the service collection.
    /// </summary>
    public static IServiceCollection AddTenjinXMappersAndService
    (
        this IServiceCollection services,
        Assembly assembly,
        TenjinXMapperScope scope = TenjinXMapperScope.Singleton,
        bool preloadAssembly = true
    )
    {
        return services
            .AddTenjinXMappers(assembly, scope, preloadAssembly)
            .AddTenjinXMapperService(scope);
    }

    internal static IEnumerable<TenjinXMapperTypeData> GetTypeDataFromAssembly(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetITenjinXMapperInterfaces().Any())
            .Select(t => new TenjinXMapperTypeData(t));
    }

    private static void AddTypeData
    (
        IServiceCollection services, 
        TenjinXMapperScope scope, 
        IEnumerable<TenjinXMapperTypeData> data
    )
    {
        foreach (var typeInfo in data)
        {
            foreach (var typeInfoInterfaceType in typeInfo.InterfaceTypes)
            {
                AddMapperType
                (
                    services,
                    scope,
                    typeInfoInterfaceType,
                    typeInfo.ImplementationType
                );
            }
        }
    }

    private static void AddMapperTypesFromAssebly
    (
        IServiceCollection services,
        TenjinXMapperScope scope,
        Assembly assembly
    )
    {
        var data = GetTypeDataFromAssembly(assembly);

        AddTypeData(services, scope, data);
    }

    private static void AddMapperType
    (
        IServiceCollection services,
        TenjinXMapperScope scope,
        Type interfaceType,
        Type implementationType
    )
    {
        switch (scope)
        {
            case TenjinXMapperScope.Scoped: services.AddScoped(interfaceType, implementationType); break;
            case TenjinXMapperScope.Singleton: services.AddSingleton(interfaceType, implementationType); break;
            case TenjinXMapperScope.Transient: services.AddTransient(interfaceType, implementationType); break;

            default: throw new TenjinXMapperException($"Unsupported Tenjin-X Mapper scope: {scope}");
        }
    }
}
