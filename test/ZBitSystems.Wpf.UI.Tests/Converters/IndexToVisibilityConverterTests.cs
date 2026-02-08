using System.Globalization;
using System.Windows;
using ZBitSystems.Wpf.UI.Converters;

namespace ZBitSystems.Wpf.UI.Tests.Converters;

[TestFixture]
public class IndexToVisibilityConverterTests
{
    private IndexToVisibilityConverter _converter = null!;

    [SetUp]
    public void SetUp()
    {
        _converter = new IndexToVisibilityConverter();
    }

    [Test]
    public void Convert_MatchingIndex_ReturnsVisible()
    {
        // Act
        var result = _converter.Convert(2, typeof(Visibility), "2", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Visible));
    }

    [Test]
    public void Convert_NonMatchingIndex_ReturnsCollapsed()
    {
        // Act
        var result = _converter.Convert(3, typeof(Visibility), "2", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Collapsed));
    }

    [Test]
    public void Convert_MatchingIndexInPipeList_ReturnsVisible()
    {
        // Act
        var result = _converter.Convert(1, typeof(Visibility), "0|1|2", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Visible));
    }

    [Test]
    public void Convert_NonMatchingIndexInPipeList_ReturnsCollapsed()
    {
        // Act
        var result = _converter.Convert(5, typeof(Visibility), "0|1|2", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Collapsed));
    }

    [Test]
    public void Convert_PipeListWithSpaces_MatchesTrimmedIndex()
    {
        // Act
        var result = _converter.Convert(1, typeof(Visibility), "0 | 1 | 2", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Visible));
    }

    [Test]
    public void Convert_NonIntegerValue_ReturnsCollapsed()
    {
        // Act
        var result = _converter.Convert("not an int", typeof(Visibility), "1", CultureInfo.InvariantCulture);

        // Assert
        Assert.That(result, Is.EqualTo(Visibility.Collapsed));
    }

    [Test]
    public void ConvertBack_ThrowsNotImplementedException()
    {
        // Assert
        Assert.Throws<NotImplementedException>(() =>
            _converter.ConvertBack(Visibility.Visible, typeof(int), null, CultureInfo.InvariantCulture));
    }
}
