using Songhay.DataAccess.Models;

namespace Songhay.DataAccess.Extensions;

/// <summary>
/// Extensions of <see cref="CommonDbms"/>
/// </summary>
public static class CommonDbmsExtensions
{
    /// <summary>
    /// Gets the provider not supported exception.
    /// </summary>
    /// <param name="commonDbms">the <see cref="CommonDbms"/></param>
    public static Exception GetProviderNotSupportedException(this CommonDbms? commonDbms) => new NotSupportedException($"Provider `{commonDbms?.InvariantProviderName}` is not supported.");
}
