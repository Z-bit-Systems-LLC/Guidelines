namespace ZBitSystems.Wpf.UI.Services;

/// <summary>
/// Interface for storing and retrieving window state information.
/// Applications should implement this interface to integrate with their settings storage mechanism.
/// </summary>
public interface IWindowStateStorage
{
    /// <summary>
    /// Gets or sets the window width in device-independent pixels.
    /// </summary>
    double WindowWidth { get; set; }

    /// <summary>
    /// Gets or sets the window height in device-independent pixels.
    /// </summary>
    double WindowHeight { get; set; }

    /// <summary>
    /// Gets or sets the window left position in device-independent pixels.
    /// Null indicates the window should be centered.
    /// </summary>
    double? WindowLeft { get; set; }

    /// <summary>
    /// Gets or sets the window top position in device-independent pixels.
    /// Null indicates the window should be centered.
    /// </summary>
    double? WindowTop { get; set; }

    /// <summary>
    /// Gets or sets whether the window is maximized.
    /// </summary>
    bool IsMaximized { get; set; }
}
