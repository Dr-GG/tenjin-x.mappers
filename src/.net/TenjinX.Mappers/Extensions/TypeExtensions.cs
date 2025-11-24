using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TenjinX.Mappers.Interfaces.Mappers;
using TenjinX.Mappers.Models;

namespace TenjinX.Mappers.Extensions;

/// <summary>
/// A collection of extension methods for the <see cref="Type"/> class.
/// </summary>
internal static class TypeExtensions
{
    private static readonly Type ITenjinXMapperType = typeof(ITenjinXMapper<,>);

    /// <summary>
    /// Gets all the <see cref="TenjinXMapperTypeData"/> from an <see cref="Assembly"/>.
    /// </summary>
    internal static IEnumerable<TenjinXMapperTypeData> GetTenjinXMapperTypeData(this Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(t => !t.IsInterface && !t.IsAbstract)
            .Where(t => t.GetITenjinXMapperInterfaces().Any())
            .Select(t => new TenjinXMapperTypeData(t));
    }

    /// <summary>
    /// Gets all the <see cref="ITenjinXMapper{TSource, TDestination}"/> of a <see cref="Type"/>.
    /// </summary>
    internal static IEnumerable<Type> GetITenjinXMapperInterfaces(this Type root)
    {
        return root
            .GetInterfaces()
            .Where(IsITenjinXMapperInterface);
    }

    /// <summary>
    /// Determines if a <see cref="Type"/> is of type <see cref="ITenjinXMapper{TSource, TDestination}"/>.
    /// </summary>
    internal static bool IsITenjinXMapperInterface(this Type type)
    {
        return 
            type.IsInterface && 
            type.IsGenericType && 
            type.GetGenericTypeDefinition() == ITenjinXMapperType;
    }
}
