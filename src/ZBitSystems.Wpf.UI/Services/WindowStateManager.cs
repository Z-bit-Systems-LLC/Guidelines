using System;
using System.Windows;

namespace ZBitSystems.Wpf.UI.Services;

/// <summary>
/// Manages window state persistence including position, size, and maximized state.
/// Handles multi-monitor scenarios and ensures windows remain accessible.
/// </summary>
public class WindowStateManager
{
    private readonly Window _window;
    private readonly IWindowStateStorage _storage;

    /// <summary>
    /// Initializes a new instance of the WindowStateManager.
    /// </summary>
    /// <param name="window">The window to manage.</param>
    /// <param name="storage">The storage provider for window state.</param>
    /// <exception cref="ArgumentNullException">Thrown when window or storage is null.</exception>
    public WindowStateManager(Window window, IWindowStateStorage storage)
    {
        _window = window ?? throw new ArgumentNullException(nameof(window));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    /// <summary>
    /// Restores the window state from storage.
    /// Should be called before the window is shown.
    /// </summary>
    public void RestoreWindowState()
    {
        var workArea = SystemParameters.WorkArea;

        // Restore size, clamped to fit within the primary monitor's work area
        _window.Width = Math.Clamp(_storage.WindowWidth, 400, workArea.Width);
        _window.Height = Math.Clamp(_storage.WindowHeight, 300, workArea.Height);

        // Restore position or center on screen
        if (_storage.WindowLeft.HasValue && _storage.WindowTop.HasValue)
        {
            var left = _storage.WindowLeft.Value;
            var top = _storage.WindowTop.Value;

            // Check if the saved position is within the virtual screen (all monitors)
            var virtualLeft = SystemParameters.VirtualScreenLeft;
            var virtualTop = SystemParameters.VirtualScreenTop;
            var virtualRight = virtualLeft + SystemParameters.VirtualScreenWidth;
            var virtualBottom = virtualTop + SystemParameters.VirtualScreenHeight;

            // Clamp position to ensure entire window is visible within virtual screen bounds
            left = Math.Clamp(left, virtualLeft, virtualRight - _window.Width);
            top = Math.Clamp(top, virtualTop, virtualBottom - _window.Height);

            // Additional check: ensure the window is reasonably accessible
            // (not hidden behind taskbar on primary monitor if that's where it ends up)
            if (left >= workArea.Left && left + _window.Width <= workArea.Right &&
                top >= workArea.Top && top + _window.Height <= workArea.Bottom)
            {
                // Window fits entirely within primary work area
                _window.Left = left;
                _window.Top = top;
            }
            else if (left >= virtualLeft && left + _window.Width <= virtualRight &&
                     top >= virtualTop && top + _window.Height <= virtualBottom)
            {
                // Window is on another monitor or partially outside work area but within virtual screen
                _window.Left = left;
                _window.Top = top;
            }
            else
            {
                // Window would be off-screen, center on primary monitor
                CenterOnWorkArea(workArea);
            }
        }
        else
        {
            CenterOnWorkArea(workArea);
        }

        // Restore maximized state after setting position
        if (_storage.IsMaximized)
        {
            _window.WindowState = WindowState.Maximized;
        }
    }

    /// <summary>
    /// Saves the current window state to storage.
    /// Should be called when the window is closing.
    /// </summary>
    public void SaveWindowState()
    {
        _storage.IsMaximized = _window.WindowState == WindowState.Maximized;

        // Save normal bounds (not maximized bounds)
        if (_window.WindowState == WindowState.Normal)
        {
            _storage.WindowWidth = _window.Width;
            _storage.WindowHeight = _window.Height;
            _storage.WindowLeft = _window.Left;
            _storage.WindowTop = _window.Top;
        }
    }

    private void CenterOnWorkArea(Rect workArea)
    {
        _window.Left = workArea.Left + (workArea.Width - _window.Width) / 2;
        _window.Top = workArea.Top + (workArea.Height - _window.Height) / 2;
    }
}
