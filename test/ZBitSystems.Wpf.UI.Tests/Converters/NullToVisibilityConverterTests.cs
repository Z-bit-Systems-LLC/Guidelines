using System.Globalization;
using System.Windows;
using ZBitSystems.Wpf.UI.Converters;

namespace ZBitSystems.Wpf.UI.Tests.Converters;

[TestFixture]
public class NullToVisibilityConverterTests
{
    private NullToVisibilityConverter _converter = null!;

    [SetUp]
    public void SetUp()
    {
        _converter = new NullToVisibilityConverter();
    }

    [Test]
    public void Convert_NotNull_ReturnsVisible()
    {
        // Act
        var result = _converter.Convert("some value", typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Visible));
    }

    [Test]
    public void Convert_Null_ReturnsCollapsed()
    {
        // Act
        var result = _converter.Convert(null, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Collapsed));
    }

    [Test]
    public void Convert_NotNullWithInvertParameter_ReturnsCollapsed()
    {
        // Act
        var result = _converter.Convert("some value", typeof(Visibility), "True", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Collapsed));
    }

    [Test]
    public void Convert_NullWithInvertParameter_ReturnsVisible()
    {
        // Act
        var result = _converter.Convert(null, typeof(Visibility), "True", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Visible));
    }

    [Test]
    public void ConvertBack_ThrowsNotImplementedException()
    {
        // Assert
        Assert.Throws<NotImplementedException>(() =>
            _converter.ConvertBack(Visibility.Visible, typeof(object), null, CultureInfo.InvariantCulture));
    }
}
