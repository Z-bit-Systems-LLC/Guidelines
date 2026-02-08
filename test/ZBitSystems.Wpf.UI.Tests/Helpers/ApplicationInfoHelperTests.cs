using System.Reflection;
using ZBitSystems.Wpf.UI.Helpers;

namespace ZBitSystems.Wpf.UI.Tests.Helpers;

[TestFixture]
public class ApplicationInfoHelperTests
{
    [Test]
    public void GetVersion_WithExplicitAssembly_ReturnsFormattedVersion()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version!;

        // Act
        var result = ApplicationInfoHelper.GetVersion(assembly);

        // Assert
        Assert.That(result, Is.EqualTo($"Version {version.Major}.{version.Minor}.{version.Build}"));
    }

    [Test]
    public void GetVersion_WithExplicitAssembly_StartsWithVersion()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();

        // Act
        var result = ApplicationInfoHelper.GetVersion(assembly);

        // Assert
        Assert.That(result, Does.StartWith("Version "));
    }

    [Test]
    public void GetCopyright_WithExplicitAssembly_ReturnsString()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();

        // Act
        var result = ApplicationInfoHelper.GetCopyright(assembly);

        // Assert - test assembly may or may not have copyright attribute
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetProductName_WithExplicitAssembly_ReturnsString()
    {
        // Arrange
        var assembly = Assembly.GetExecutingAssembly();

        // Act
        var result = ApplicationInfoHelper.GetProductName(assembly);

        // Assert - test assembly may or may not have product attribute
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetVersion_NullAssembly_DoesNotThrow()
    {
        // Act & Assert - will use entry assembly (may be null in test runner)
        Assert.DoesNotThrow(() => ApplicationInfoHelper.GetVersion(null));
    }

    [Test]
    public void GetCopyright_NullAssembly_DoesNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => ApplicationInfoHelper.GetCopyright(null));
    }

    [Test]
    public void GetProductName_NullAssembly_DoesNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => ApplicationInfoHelper.GetProductName(null));
    }
}
