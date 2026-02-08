using System.Globalization;
using ZBitSystems.Wpf.UI.Converters;

namespace ZBitSystems.Wpf.UI.Tests.Converters;

[TestFixture]
public class InverseBoolConverterTests
{
    private InverseBoolConverter _converter = null!;

    [SetUp]
    public void SetUp()
    {
        _converter = new InverseBoolConverter();
    }

    [Test]
    public void Convert_True_ReturnsFalse()
    {
        // Act
        var result = _converter.Convert(true, typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(false));
    }

    [Test]
    public void Convert_False_ReturnsTrue()
    {
        // Act
        var result = _converter.Convert(false, typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(true));
    }

    [Test]
    public void Convert_NonBooleanValue_ReturnsFalse()
    {
        // Act
        var result = _converter.Convert("not a bool", typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(false));
    }

    [Test]
    public void ConvertBack_True_ReturnsFalse()
    {
        // Act
        var result = _converter.ConvertBack(true, typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(false));
    }

    [Test]
    public void ConvertBack_False_ReturnsTrue()
    {
        // Act
        var result = _converter.ConvertBack(false, typeof(bool), null, CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(true));
    }
}
