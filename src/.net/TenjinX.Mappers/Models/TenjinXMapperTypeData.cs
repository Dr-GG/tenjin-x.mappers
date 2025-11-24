using System;
using System.Collections.Generic;
using System.Text;
using TenjinX.Mappers.Interfaces.Mappers;

namespace TenjinX.Mappers.Models;

/// <summary>
/// A data structure representing the implementation of one or more <see cref="ITenjinXMapper{TSource, TDestination}"/> interfaces.
/// </summary>
internal record TenjinXMapperTypeData
{
    /// <summary>
    /// Gets the primary implementation <see cref="Type"/>.
    /// </summary>
    public Type ImplementationType { get; }

    /// <summary>
    /// Gets the <see cref="ITenjinXMapper{TSource, TDestination}"/> interfaces.
    /// </summary>
    public IEnumerable<Type> InterfaceTypes { get; }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public TenjinXMapperTypeData(Type implementationType)
    {
        var interfaceType = typeof(ITenjinXMapper<,>);

        ImplementationType = implementationType;
        InterfaceTypes = implementationType
            .GetInterfaces()
            .Where(i => i.IsGenericType && (i.GetGenericTypeDefinition() == interfaceType))
            .ToList();
    }
}
