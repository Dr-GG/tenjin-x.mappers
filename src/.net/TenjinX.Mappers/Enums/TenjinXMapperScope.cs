namespace TenjinX.Mappers.Enums;

/// <summary>
/// Defines the lifetime scope of a TenjinX mapper.
/// </summary>
public enum TenjinXMapperScope
{
    /// <summary>
    /// An unknown mapper scope.
    /// </summary>
    Unknown,

    /// <summary>
    /// A mapper with a scoped lifetime.
    /// </summary>
    Scoped,

    /// <summary>
    /// A mapper with a singleton lifetime.
    /// </summary>
    Singleton,

    /// <summary>
    /// A mapper with a transient lifetime.
    /// </summary>
    Transient
}
