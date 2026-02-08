using System.ComponentModel;
using Moq;
using ZBitSystems.Wpf.UI.Localization;

namespace ZBitSystems.Wpf.UI.Tests.Localization;

[TestFixture]
public class LocalizedStringBindingTests
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
    public void Value_ReturnsLocalizedString()
    {
        // Arrange
        var binding = new LocalizedStringBinding("Test_Greeting");

        // Act
        var value = binding.Value;

        // Assert
        Assert.That(value, Is.EqualTo("Hello"));
    }

    [Test]
    public void Value_WithUnknownKey_ReturnsKeyInBrackets()
    {
        // Arrange
        var binding = new LocalizedStringBinding("Unknown_Key");

        // Act
        var value = binding.Value;

        // Assert
        Assert.That(value, Is.EqualTo("[Unknown_Key]"));
    }

    [Test]
    public void PropertyChanged_WhenProviderNotifies_IsRaised()
    {
        // Arrange
        var binding = new LocalizedStringBinding("Test_Greeting");
        var propertyChangedRaised = false;
        string? changedPropertyName = null;

        binding.PropertyChanged += (sender, e) =>
        {
            propertyChangedRaised = true;
            changedPropertyName = e.PropertyName;
        };

        // Act
        _mockProvider.Raise(p => p.PropertyChanged += null, new PropertyChangedEventArgs(string.Empty));

        // Assert
        Assert.That(propertyChangedRaised, Is.True);
        Assert.That(changedPropertyName, Is.EqualTo(nameof(LocalizedStringBinding.Value)));
    }

    [Test]
    public void Value_AfterProviderUpdate_ReturnsUpdatedString()
    {
        // Arrange
        var binding = new LocalizedStringBinding("Test_Greeting");
        var initialValue = binding.Value;

        // Act - Change the mock to return a different value
        _mockProvider.Setup(p => p.GetString("Test_Greeting")).Returns("こんにちは");
        var updatedValue = binding.Value;

        // Assert
        Assert.That(initialValue, Is.EqualTo("Hello"));
        Assert.That(updatedValue, Is.EqualTo("こんにちは"));
    }

    [Test]
    public void Constructor_WithKey_CreatesBinding()
    {
        // Act
        var binding = new LocalizedStringBinding("Test_Key");

        // Assert
        Assert.That(binding, Is.Not.Null);
        Assert.That(binding.Value, Is.EqualTo("Test Value"));
    }
}
