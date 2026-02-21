using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ZBitSystems.Wpf.UI.Helpers;

/// <summary>
/// Provides an attached property for adding a copy-to-clipboard command to TextBox controls.
/// </summary>
public static class CopyTextBoxHelper
{
    /// <summary>
    /// Identifies the CopyCommand attached dependency property.
    /// </summary>
    public static readonly DependencyProperty CopyCommandProperty =
        DependencyProperty.RegisterAttached(
            "CopyCommand",
            typeof(ICommand),
            typeof(CopyTextBoxHelper),
            new PropertyMetadata(null));

    /// <summary>
    /// Gets the copy command for the specified element.
    /// </summary>
    public static ICommand GetCopyCommand(DependencyObject obj)
    {
        return (ICommand)obj.GetValue(CopyCommandProperty);
    }

    /// <summary>
    /// Sets the copy command for the specified element.
    /// </summary>
    public static void SetCopyCommand(DependencyObject obj, ICommand value)
    {
        obj.SetValue(CopyCommandProperty, value);
    }

    /// <summary>
    /// Gets a default command that copies the TextBox text to the clipboard.
    /// </summary>
    public static readonly ICommand DefaultCopyCommand = new RelayCommand(
        parameter =>
        {
            if (parameter is TextBox textBox && !string.IsNullOrEmpty(textBox.Text))
            {
                try
                {
                    Clipboard.SetText(textBox.Text);
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                    // Clipboard is locked by another application, ignore silently
                    // This is a common Windows issue and shouldn't crash the app
                }
            }
        });
}

/// <summary>
/// A simple relay command implementation for WPF commanding.
/// </summary>
public class RelayCommand(Action<object> execute, Func<object, bool>? canExecute = null)
    : ICommand
{
    private readonly Action<object> _execute = execute ?? throw new ArgumentNullException(nameof(execute));

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <inheritdoc />
    public bool CanExecute(object? parameter)
    {
        return parameter != null && (canExecute?.Invoke(parameter) ?? true);
    }

    /// <inheritdoc />
    public void Execute(object? parameter)
    {
        if (parameter != null) _execute(parameter);
    }
}
