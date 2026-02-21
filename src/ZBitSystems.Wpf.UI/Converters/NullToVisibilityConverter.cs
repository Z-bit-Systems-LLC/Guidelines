using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ZBitSystems.Wpf.UI.Converters;

/// <summary>
/// Converts a nullable value to a <see cref="Visibility"/> value. Non-null is <see cref="Visibility.Visible"/>; null is <see cref="Visibility.Collapsed"/>.
/// </summary>
public class NullToVisibilityConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isVisible = value != null;
        if (parameter != null && bool.Parse(parameter.ToString() ?? "False")) isVisible = !isVisible;
        return isVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
