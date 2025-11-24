using TenjinX.Exceptions;

namespace TenjinX.Mappers.Exceptions;

/// <summary>
/// The exception that is thrown when a TenjinX mapper error occurs.
/// </summary>
/// <remarks>
public class TenjinXMapperException(string message) : TenjinException(message);
