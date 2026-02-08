using System.ComponentModel;

namespace ZBitSystems.Wpf.UI.Localization;

/// <summary>
/// Interface for providing localized strings to the localization system.
/// Applications should implement this interface to provide their resource strings.
/// </summary>
public interface ILocalizationProvider : INotifyPropertyChanged
{
    /// <summary>
    /// Gets the localized string for the specified key.
    /// </summary>
    /// <param name="key">The resource key.</param>
    /// <returns>The localized string, or the key in brackets if not found.</returns>
    string GetString(string key);

    /// <summary>
    /// Gets the current culture name (e.g., "en-US", "ja-JP").
    /// </summary>
    string CurrentCulture { get; }
}
