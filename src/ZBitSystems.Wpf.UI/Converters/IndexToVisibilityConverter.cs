using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ZBitSystems.Wpf.UI.Converters;

/// <summary>
/// Converts an integer index to a <see cref="Visibility"/> value based on pipe-separated target indices in the converter parameter.
/// </summary>
public class IndexToVisibilityConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int index && parameter is string paramString)
        {
            // Support pipe-separated indices like "1|2"
            var targetIndices = paramString.Split('|');
            foreach (var targetStr in targetIndices)
            {
                if (int.TryParse(targetStr.Trim(), out int targetIndex) && index == targetIndex)
                {
                    return Visibility.Visible;
                }
            }
        }

        return Visibility.Collapsed;
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
