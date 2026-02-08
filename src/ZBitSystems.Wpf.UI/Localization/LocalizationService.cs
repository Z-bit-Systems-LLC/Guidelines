using System;

namespace ZBitSystems.Wpf.UI.Localization;

/// <summary>
/// Static service for managing the application's localization provider.
/// Applications must set the Provider property during initialization.
/// </summary>
public static class LocalizationService
{
    private static ILocalizationProvider? _provider;

    /// <summary>
    /// Gets or sets the current localization provider.
    /// This must be set by the application before using localization features.
    /// </summary>
    public static ILocalizationProvider Provider
    {
        get => _provider ?? throw new InvalidOperationException(
            "LocalizationService.Provider must be set before using localization features. " +
            "Set it during application startup (e.g., in App.xaml.cs).");
        set => _provider = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets a value indicating whether a provider has been set.
    /// </summary>
    public static bool IsProviderSet => _provider != null;
}
