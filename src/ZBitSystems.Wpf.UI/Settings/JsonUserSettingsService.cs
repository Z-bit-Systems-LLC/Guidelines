using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZBitSystems.Wpf.UI.Settings;

/// <summary>
/// JSON file-based settings service. Stores settings to
/// <c>%LOCALAPPDATA%/{appName}/settings.json</c> for unpackaged apps,
/// or the package-scoped local folder for MSIX-packaged apps.
/// </summary>
/// <typeparam name="T">The settings type, which must derive from <see cref="UserSettings"/>.</typeparam>
public class JsonUserSettingsService<T> : IUserSettingsService<T> where T : UserSettings, new()
{
    private const string SettingsFileName = "settings.json";

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly string _settingsFilePath;

    /// <inheritdoc />
    public T Settings { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonUserSettingsService{T}"/> class.
    /// Settings are loaded synchronously during construction; failures fall back to defaults.
    /// </summary>
    /// <param name="appName">
    /// Application name used as the subfolder under <c>%LOCALAPPDATA%</c> for unpackaged apps.
    /// </param>
    public JsonUserSettingsService(string appName)
    {
        var folderPath = PackageHelper.GetSettingsFolderPath(appName);
        Directory.CreateDirectory(folderPath);
        _settingsFilePath = Path.Combine(folderPath, SettingsFileName);
        Settings = Load();
    }

    /// <inheritdoc />
    public async Task SaveAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(Settings, JsonOptions);
            await File.WriteAllTextAsync(_settingsFilePath, json);
        }
        catch
        {
            // Silently fail — settings will revert to defaults next launch
        }
    }

    private T Load()
    {
        try
        {
            if (File.Exists(_settingsFilePath))
            {
                var json = File.ReadAllText(_settingsFilePath);
                return JsonSerializer.Deserialize<T>(json) ?? new T();
            }
        }
        catch
        {
            // Fall back to defaults
        }

        return new T();
    }
}

/// <summary>
/// Helper for determining packaged-app vs. unpackaged-app storage paths.
/// Separated from <see cref="JsonUserSettingsService{T}"/> because DllImport
/// cannot appear inside a generic type.
/// </summary>
internal static class PackageHelper
{
    private const int AppmodelErrorNoPackage = 15700;

    internal static string GetSettingsFolderPath(string appName)
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        if (IsPackagedApp())
        {
            // MSIX-packaged apps get an automatic per-app local folder
            return localAppData;
        }

        return Path.Combine(localAppData, appName);
    }

    private static bool IsPackagedApp()
    {
        var length = 0u;
        var result = GetCurrentPackageFullName(ref length, null);
        return result != AppmodelErrorNoPackage;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
    private static extern int GetCurrentPackageFullName(ref uint packageFullNameLength, char[]? packageFullName);
}
