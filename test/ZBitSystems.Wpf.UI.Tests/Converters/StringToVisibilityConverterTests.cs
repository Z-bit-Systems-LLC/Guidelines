using System.Globalization;
using System.Windows;
using ZBitSystems.Wpf.UI.Converters;

namespace ZBitSystems.Wpf.UI.Tests.Converters;

[TestFixture]
public class StringToVisibilityConverterTests
{
    private StringToVisibilityConverter _converter = null!;

    [SetUp]
    public void SetUp()
    {
        _converter = new StringToVisibilityConverter();
    }

    [Test]
    public void Convert_MatchingString_ReturnsVisible()
    {
        // Act
        var result = _converter.Convert("test", typeof(Visibility), "test", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Visible));
    }

    [Test]
    public void Convert_NonMatchingString_ReturnsCollapsed()
    {
        // Act
        var result = _converter.Convert("test", typeof(Visibility), "other", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Collapsed));
    }

    [Test]
    public void Convert_MatchingStringInSemicolonList_ReturnsVisible()
    {
        // Act
        var result = _converter.Convert("value2", typeof(Visibility), "value1;value2;value3", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Visible));
    }

    [Test]
    public void Convert_NonMatchingStringInSemicolonList_ReturnsCollapsed()
    {
        // Act
        var result = _converter.Convert("value4", typeof(Visibility), "value1;value2;value3", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Collapsed));
    }

    [Test]
    public void Convert_SemicolonListWithSpaces_MatchesTrimmedValue()
    {
        // Act
        var result = _converter.Convert("value2", typeof(Visibility), "value1 ; value2 ; value3", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Visible));
    }

    [Test]
    public void ConvertBack_ThrowsNotImplementedException()
    {
        // Assert
        Assert.Throws<NotImplementedException>(() =>
            _converter.ConvertBack(Visibility.Visible, typeof(string), null, CultureInfo.InvariantCulture));
    }
}
