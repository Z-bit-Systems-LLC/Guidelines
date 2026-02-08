using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace ZBitSystems.Wpf.UI.Services;

/// <summary>
/// Manages application theme by following Windows OS settings.
/// Provides notifications when the system theme changes.
/// </summary>
public class ThemeManager : INotifyPropertyChanged, IDisposable
{
    private bool _disposed;

    /// <summary>
    /// Gets whether the current theme is dark mode.
    /// </summary>
    public bool IsDarkMode => ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark;

    /// <summary>
    /// Gets the current application theme.
    /// </summary>
    public ApplicationTheme CurrentTheme => ApplicationThemeManager.GetAppTheme();

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Initializes a new instance of the ThemeManager.
    /// Starts watching for system theme changes.
    /// </summary>
    /// <param name="window">The window to watch for theme changes. Can be null if called before window is available.</param>
    public ThemeManager(Window? window = null)
    {
        if (window != null)
        {
            StartWatching(window);
        }

        ApplicationThemeManager.Changed += OnThemeChanged;
    }

    /// <summary>
    /// Starts watching the specified window for system theme changes.
    /// Call this after the window is fully loaded if not provided in constructor.
    /// </summary>
    /// <param name="window">The window to watch.</param>
    /// <exception cref="ArgumentNullException">Thrown when window is null.</exception>
    public void StartWatching(Window window)
    {
        if (window == null)
            throw new ArgumentNullException(nameof(window));

        SystemThemeWatcher.Watch(window);
    }

    private void OnThemeChanged(ApplicationTheme currentTheme, Color systemAccent)
    {
        OnPropertyChanged(nameof(IsDarkMode));
        OnPropertyChanged(nameof(CurrentTheme));
    }

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Releases resources used by the ThemeManager.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases resources used by the ThemeManager.
    /// </summary>
    /// <param name="disposing">True if called from Dispose, false if called from finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                ApplicationThemeManager.Changed -= OnThemeChanged;
            }

            _disposed = true;
        }
    }
}
