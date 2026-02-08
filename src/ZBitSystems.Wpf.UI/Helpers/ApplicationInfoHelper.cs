using System.Reflection;

namespace ZBitSystems.Wpf.UI.Helpers;

/// <summary>
/// Provides helper methods for reading application metadata from assembly attributes.
/// </summary>
public static class ApplicationInfoHelper
{
    /// <summary>
    /// Gets a formatted version string from the assembly version.
    /// </summary>
    /// <param name="assembly">The assembly to read from, or <c>null</c> to use the entry assembly.</param>
    /// <returns>A string in the format "Version {Major}.{Minor}.{Build}", or <see cref="string.Empty"/> if unavailable.</returns>
    public static string GetVersion(Assembly? assembly = null)
    {
        var version = (assembly ?? Assembly.GetEntryAssembly())?.GetName().Version;
        return version != null ? $"Version {version.Major}.{version.Minor}.{version.Build}" : string.Empty;
    }

    /// <summary>
    /// Gets the copyright notice from the <see cref="AssemblyCopyrightAttribute"/>.
    /// </summary>
    /// <param name="assembly">The assembly to read from, or <c>null</c> to use the entry assembly.</param>
    /// <returns>The copyright string, or <see cref="string.Empty"/> if unavailable.</returns>
    public static string GetCopyright(Assembly? assembly = null)
    {
        return (assembly ?? Assembly.GetEntryAssembly())
            ?.GetCustomAttribute<AssemblyCopyrightAttribute>()
            ?.Copyright ?? string.Empty;
    }

    /// <summary>
    /// Gets the product name from the <see cref="AssemblyProductAttribute"/>.
    /// </summary>
    /// <param name="assembly">The assembly to read from, or <c>null</c> to use the entry assembly.</param>
    /// <returns>The product name, or <see cref="string.Empty"/> if unavailable.</returns>
    public static string GetProductName(Assembly? assembly = null)
    {
        return (assembly ?? Assembly.GetEntryAssembly())
            ?.GetCustomAttribute<AssemblyProductAttribute>()
            ?.Product ?? string.Empty;
    }
}
