using Moq;
using ZBitSystems.Wpf.UI.Localization;

namespace ZBitSystems.Wpf.UI.Tests.Localization;

[TestFixture]
public class LocalizationServiceTests
{
    private Mock<ILocalizationProvider>? _mockProvider;

    [SetUp]
    public void SetUp()
    {
        _mockProvider = new Mock<ILocalizationProvider>();
    }

    [TearDown]
    public void TearDown()
    {
        // Reset the service after each test
        if (_mockProvider != null)
        {
            LocalizationService.Provider = _mockProvider.Object;
        }
    }

    [Test]
    public void Provider_WhenSet_ReturnsSetProvider()
    {
        // Arrange & Act
        LocalizationService.Provider = _mockProvider!.Object;

        // Assert
        Assert.That(LocalizationService.Provider, Is.SameAs(_mockProvider.Object));
    }

    [Test]
    public void Provider_WhenSetToNull_ThrowsArgumentNullException()
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => LocalizationService.Provider = null!);
    }

    [Test]
    public void Provider_WhenNotSet_ThrowsInvalidOperationException()
    {
        // Arrange
        // Force the provider to null using reflection to test this scenario
        var field = typeof(LocalizationService).GetField("_provider",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        field?.SetValue(null, null);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => _ = LocalizationService.Provider);
        Assert.That(ex!.Message, Does.Contain("must be set before using"));
    }

    [Test]
    public void IsProviderSet_WhenProviderIsSet_ReturnsTrue()
    {
        // Arrange & Act
        LocalizationService.Provider = _mockProvider!.Object;

        // Assert
        Assert.That(LocalizationService.IsProviderSet, Is.True);
    }

    [Test]
    public void IsProviderSet_WhenProviderIsNotSet_ReturnsFalse()
    {
        // Arrange - Force provider to null
        var field = typeof(LocalizationService).GetField("_provider",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        field?.SetValue(null, null);

        // Assert
        Assert.That(LocalizationService.IsProviderSet, Is.False);
    }
}
