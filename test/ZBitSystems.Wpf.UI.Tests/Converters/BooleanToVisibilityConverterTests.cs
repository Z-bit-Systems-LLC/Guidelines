using System.Globalization;
using System.Windows;
using ZBitSystems.Wpf.UI.Converters;

namespace ZBitSystems.Wpf.UI.Tests.Converters;

[TestFixture]
public class BooleanToVisibilityConverterTests
{
    private BooleanToVisibilityConverter _converter = null!;

    [SetUp]
    public void SetUp()
    {
        _converter = new BooleanToVisibilityConverter();
    }

    [Test]
    public void Convert_TrueWithoutInvert_ReturnsVisible()
    {
        // Act
        var result = _converter.Convert(true, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Visible));
    }

    [Test]
    public void Convert_FalseWithoutInvert_ReturnsCollapsed()
    {
        // Act
        var result = _converter.Convert(false, typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Collapsed));
    }

    [Test]
    public void Convert_TrueWithInvert_ReturnsCollapsed()
    {
        // Act
        var result = _converter.Convert(true, typeof(Visibility), "Invert", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Collapsed));
    }

    [Test]
    public void Convert_FalseWithInvert_ReturnsVisible()
    {
        // Act
        var result = _converter.Convert(false, typeof(Visibility), "Invert", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Visible));
    }

    [Test]
    public void Convert_NonBooleanValue_ReturnsCollapsed()
    {
        // Act
        var result = _converter.Convert("not a bool", typeof(Visibility), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Collapsed));
    }

    [Test]
    public void ConvertBack_Visible_ReturnsTrue()
    {
        // Act
        var result = _converter.ConvertBack(Visibility.Visible, typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(true));
    }

    [Test]
    public void ConvertBack_Collapsed_ReturnsFalse()
    {
        // Act
        var result = _converter.ConvertBack(Visibility.Collapsed, typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(false));
    }

    [Test]
    public void ConvertBack_VisibleWithInvert_ReturnsFalse()
    {
        // Act
        var result = _converter.ConvertBack(Visibility.Visible, typeof(bool), "Invert", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(false));
    }

    [Test]
    public void ConvertBack_CollapsedWithInvert_ReturnsTrue()
    {
        // Act
        var result = _converter.ConvertBack(Visibility.Collapsed, typeof(bool), "Invert", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(true));
    }
}
