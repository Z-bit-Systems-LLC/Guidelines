using Moq;
using ZBitSystems.Wpf.UI.Localization;

namespace ZBitSystems.Wpf.UI.Tests.Localization;

[TestFixture]
public class LocalizeExtensionTests
{
    private Mock<ILocalizationProvider> _mockProvider = null!;

    [SetUp]
    public void SetUp()
    {
        _mockProvider = new Mock<ILocalizationProvider>();
        _mockProvider.Setup(p => p.GetString("Test_Greeting")).Returns("Hello");
        _mockProvider.Setup(p => p.GetString("Test_Key")).Returns("Test Value");
        _mockProvider.Setup(p => p.GetString(It.Is<string>(key => !new[] { "Test_Greeting", "Test_Key" }.Contains(key))))
            .Returns((string key) => $"[{key}]");

        LocalizationService.Provider = _mockProvider.Object;
    }

    [Test]
    public void Constructor_WithoutKey_CreatesExtension()
    {
        // Act
        var extension = new LocalizeExtension();

        // Assert
        Assert.That(extension, Is.Not.Null);
        Assert.That(extension.Key, Is.Empty);
    }

    [Test]
    public void Constructor_WithKey_SetsKey()
    {
        // Act
        var extension = new LocalizeExtension("Test_Key");

        // Assert
        Assert.That(extension.Key, Is.EqualTo("Test_Key"));
    }

    [Test]
    public void Key_CanBeSetViaProperty()
    {
        // Arrange
        var extension = new LocalizeExtension();

        // Act
        extension.Key = "Test_Greeting";

        // Assert
        Assert.That(extension.Key, Is.EqualTo("Test_Greeting"));
    }

    [Test]
    public void ProvideValue_WithEmptyKey_ReturnsMissingKeyPlaceholder()
    {
        // Arrange
        var extension = new LocalizeExtension("");

        // Act
        var result = extension.ProvideValue(null!);

        // Assert
        Assert.That(result, Is.EqualTo("[MISSING_KEY]"));
    }

    [Test]
    public void ProvideValue_WithNullKey_ReturnsMissingKeyPlaceholder()
    {
        // Arrange
        var extension = new LocalizeExtension
        {
            Key = null!
        };

        // Act
        var result = extension.ProvideValue(null!);

        // Assert
        Assert.That(result, Is.EqualTo("[MISSING_KEY]"));
    }

    [Test]
    public void ProvideValue_WithValidKey_CreatesBinding()
    {
        // Arrange
        var extension = new LocalizeExtension("Test_Greeting");

        // Act
        var result = extension.ProvideValue(null!);

        // Assert - ProvideValue returns a Binding object for dynamic updates
        Assert.That(result, Is.InstanceOf<System.Windows.Data.Binding>());
    }

    [Test]
    public void ProvideValue_WithUnknownKey_CreatesBinding()
    {
        // Arrange
        var extension = new LocalizeExtension("Unknown_Key");

        // Act
        var result = extension.ProvideValue(null!);

        // Assert - ProvideValue returns a Binding object even for unknown keys
        // The binding will resolve to [Unknown_Key] when evaluated
        Assert.That(result, Is.InstanceOf<System.Windows.Data.Binding>());
    }

}
