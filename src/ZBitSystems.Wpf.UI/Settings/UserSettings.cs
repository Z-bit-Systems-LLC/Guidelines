namespace ZBitSystems.Wpf.UI.Settings;

using ZBitSystems.Wpf.UI.Services;

/// <summary>
/// Base settings class that implements <see cref="IWindowStateStorage"/> directly,
/// eliminating the need for an adapter. Apps extend this for app-specific settings.
/// </summary>
public class UserSettings : IWindowStateStorage
{
    /// <inheritdoc />
    public double WindowWidth { get; set; } = 800;

    /// <inheritdoc />
    public double WindowHeight { get; set; } = 600;

    /// <inheritdoc />
    public double? WindowLeft { get; set; }

    /// <inheritdoc />
    public double? WindowTop { get; set; }

    /// <inheritdoc />
    public bool IsMaximized { get; set; }
}
