using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;
using TenjinX.Mappers.Exceptions;
using TenjinX.Mappers.Extensions;
using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Models;

namespace TenjinX.Mappers.Services.Registry;

/// <summary>
/// The registry for TenjinX mapper reflection information.
/// </summary>
internal class TenjinXMapperReflectionRegistry
{
    private readonly ReaderWriterLockSlim _lock = new();
    private readonly Dictionary<string, TenjinXMapperMethod> _mapperMethods = [];

    /// <summary>
    /// Preloads previously registered TenjinX mappers from assemblies.
    /// </summary>
    internal void Preload(IServiceProvider serviceProvider)
    {
        foreach (var assembly in ServicesExtensions.TenjinXMapperPreloadAssemblies)
        {
            var typeData = assembly.GetTenjinXMapperTypeData();

            foreach (var type in typeData)
            {
                foreach (var interfaceType in type.InterfaceTypes)
                {
                    var genericArgs = interfaceType.GetGenericArguments();
                    var sourceType = genericArgs[0];
                    var destinationType = genericArgs[1];
                    var mapperKey = GetMapperyTypeKey(sourceType, destinationType);

                    AddMapper(mapperKey, interfaceType, sourceType, destinationType);
                }
            }
        }
    }

    /// <summary>
    /// Gets the mapping method for the specified source and destination types.
    /// </summary>
    internal TenjinXMapperMethod GetMappingMethod
    (
        object source,
        object destination
    )
    {
        var mapperTypeKey = GetMapperyTypeKey(source, destination);

        return GetMappingMethod(source, destination, mapperTypeKey);
    }

    private TenjinXMapperMethod GetMappingMethod
    (
        object source,
        object destination,
        string mapperTypeKey
    )
    {
        _lock.EnterUpgradeableReadLock();

        try
        {
            if (_mapperMethods.TryGetValue(mapperTypeKey, out var mapperMethod))
            {
                return mapperMethod;
            }

            return AddMappingMethods(mapperTypeKey, source, destination);
        }
        finally
        {
            _lock.ExitUpgradeableReadLock();
        }
    }

    private TenjinXMapperMethod AddMappingMethods
    (
        string mapperKey,
        object source,
        object destination
    )
    {
        _lock.EnterWriteLock();

        try
        {
            // Ensure another thread hasn't already added the mapper method
            if (_mapperMethods.TryGetValue(mapperKey, out var existingMapperMethod))
            {
                return existingMapperMethod;
            }

            var mapperType = typeof(ITenjinXMapper<,>).MakeGenericType(source.GetType(), destination.GetType());

            return AddMapper
            (
                mapperKey, 
                mapperType,
                source.GetType(), 
                destination.GetType()
            );
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    private TenjinXMapperMethod AddMapper
    (
        string mapperKey,
        Type mapperType,
        Type sourceType, 
        Type destinationType
    )
    {
        var mapMethods = mapperType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.Name == nameof(ITenjinXMapper<,>.Map))
            .Select(m => new
            {
                Method = m,
                Parameters = m.GetParameters().ToList()
            })
            .Where
            (d =>
                d.Parameters.Count == 3 &&
                d.Parameters[0].Name == "source" &&
                d.Parameters[1].Name == "destination" &&
                d.Parameters[2].Name == "context" &&
                d.Parameters[2].ParameterType == typeof(object) &&
                d.Parameters[2].DefaultValue == null &&
                (
                    (
                        d.Parameters[0].ParameterType == sourceType &&
                        d.Parameters[1].ParameterType == destinationType
                    )
                    ||
                    (
                        d.Parameters[0].ParameterType == destinationType &&
                        d.Parameters[1].ParameterType == sourceType
                    )
                )
            )
            .Select(d => new TenjinXMapperMethod
            {
                Key = GetMapperyTypeKey(d.Parameters[0].ParameterType, d.Parameters[1].ParameterType),
                MapAction = CreateMapAction(d.Method, d.Parameters),
                MapperType = mapperType
            });

        foreach (var mapperMethod in mapMethods)
        {
            _mapperMethods[mapperMethod.Key] = mapperMethod;
        }

        return _mapperMethods[mapperKey];
    }

    private static Action<object, object, object, object?> CreateMapAction
    (
        MethodInfo methodInfo, 
        IEnumerable<ParameterInfo> parameters
    )
    {
        // Get the source and destination parameter types
        var enumeratedParameters = parameters.ToList();
        var srcParamType = enumeratedParameters[0].ParameterType;
        var dstParamType = enumeratedParameters[1].ParameterType;

        // parameters: (object mapper, object source, object destination, object? context)
        var pMapper = Expression.Parameter(typeof(object), "mapper");
        var pSource = Expression.Parameter(typeof(object), "source");
        var pDestination = Expression.Parameter(typeof(object), "destination");
        var pContext = Expression.Parameter(typeof(object), "context");

        // Cast the mapper parameter to the concrete declaring type of the method
        var declaringType = methodInfo.DeclaringType!;
        var mapperCast = Expression.Convert(pMapper, declaringType);

        // Cast the source and destination parameters to their respective types
        var sourceCast = Expression.Convert(pSource, srcParamType);
        var destCast = Expression.Convert(pDestination, dstParamType);

        // Call the mapping method
        var call = Expression.Call(mapperCast, methodInfo, sourceCast, destCast, pContext);
        var lambda = Expression.Lambda<Action<object, object, object, object?>>
        (
            call, 
            pMapper, 
            pSource, 
            pDestination, 
            pContext
        );

        return lambda.Compile();
    }

    private static string GetMapperyTypeKey(object source, object destination)
    {
        return GetMapperyTypeKey(source.GetType(), destination.GetType());
    }

    private static string GetMapperyTypeKey(Type sourceType, Type destinationType)
    {
        return $"{sourceType.FullName}->{destinationType.FullName}";
    }
}
