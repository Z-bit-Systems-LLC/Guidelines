using System.Threading.Tasks;

namespace ZBitSystems.Wpf.UI.Settings;

/// <summary>
/// Generic interface for loading and persisting user settings.
/// </summary>
/// <typeparam name="T">The settings type, which must derive from <see cref="UserSettings"/>.</typeparam>
public interface IUserSettingsService<out T> where T : UserSettings
{
    /// <summary>
    /// Gets the current settings instance.
    /// </summary>
    T Settings { get; }

    /// <summary>
    /// Persists the current settings to storage.
    /// </summary>
    Task SaveAsync();
}
