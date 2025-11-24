using System.Reflection;
using TenjinX.Extensions;

namespace TenjinX.Mappers.Moq.Services.Cache;

/// <summary>
/// The cache for TenjinX mapper Moq callback property information.
/// </summary>
internal class TenjinXMapperMoqCallbackCache
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="TenjinXMapperMoqCallbackCache"/>.
    /// </summary>
    internal static TenjinXMapperMoqCallbackCache Instance { get; } = new();

    private readonly Dictionary<string, IEnumerable<PropertyInfo>> _typeProperties = new();
    private readonly ReaderWriterLockSlim _lock = new();

    public IEnumerable<PropertyInfo> GetTypeProperties(Type type)
    {
        var cacheKey = type.GetFullName();

        _lock.EnterUpgradeableReadLock();

        try
        {
            if (_typeProperties.TryGetValue(cacheKey, out var properties))
            {
                return properties;
            }

            return LoadTypeProperties(type);
        }
        finally
        {
            _lock.ExitUpgradeableReadLock();
        }
    }

    private IEnumerable<PropertyInfo> LoadTypeProperties(Type type)
    {
        _lock.EnterWriteLock();

        try
        {
            // Double-check locking
            if (_typeProperties.TryGetValue(type.GetFullName(), out var properties))
            {
                return properties;
            }

            properties = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite)
                .ToList();

            _typeProperties[type.GetFullName()] = properties;

            return properties;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}
