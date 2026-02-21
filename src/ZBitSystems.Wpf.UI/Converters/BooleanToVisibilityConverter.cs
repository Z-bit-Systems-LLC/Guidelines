using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ZBitSystems.Wpf.UI.Converters;

/// <summary>
/// Converts a boolean value to a <see cref="Visibility"/> value, with optional inversion via the converter parameter.
/// </summary>
public class BooleanToVisibilityConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool booleanValue)
        {
            // Handle the "Invert" parameter
            var isInverted = parameter != null && parameter.ToString()!.Equals("Invert", StringComparison.OrdinalIgnoreCase);

            // Return the inverted or non-inverted result
            return isInverted ? booleanValue ? Visibility.Collapsed : Visibility.Visible :
                booleanValue ? Visibility.Visible : Visibility.Collapsed;
        }

        return Visibility.Collapsed; // Default
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Visibility visibilityValue)
        {
            var isInverted = parameter != null && parameter.ToString()!.Equals("Invert", StringComparison.OrdinalIgnoreCase);

            // Convert visibility back to boolean
            return isInverted ? visibilityValue == Visibility.Collapsed : visibilityValue == Visibility.Visible;
        }

        return false; // Default
    }
}
