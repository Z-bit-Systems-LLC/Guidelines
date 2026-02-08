using System.Windows;
using ZBitSystems.Wpf.UI.Controls;

namespace ZBitSystems.Wpf.UI.Tests.Controls;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class LicenseExpanderTests
{
    [Test]
    public void Constructor_CreatesInstance()
    {
        // Act
        var expander = new LicenseExpander();

        // Assert
        Assert.That(expander, Is.Not.Null);
    }

    [Test]
    public void Header_DefaultValue_IsEmpty()
    {
        // Act
        var expander = new LicenseExpander();

        // Assert
        Assert.That(expander.Header, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Header_SetValue_ReturnsSetValue()
    {
        // Arrange
        var expander = new LicenseExpander();

        // Act
        expander.Header = "MIT License";

        // Assert
        Assert.That(expander.Header, Is.EqualTo("MIT License"));
    }

    [Test]
    public void LicenseText_DefaultValue_IsEmpty()
    {
        // Act
        var expander = new LicenseExpander();

        // Assert
        Assert.That(expander.LicenseText, Is.EqualTo(string.Empty));
    }

    [Test]
    public void LicenseText_SetValue_ReturnsSetValue()
    {
        // Arrange
        var expander = new LicenseExpander();

        // Act
        expander.LicenseText = "License content here";

        // Assert
        Assert.That(expander.LicenseText, Is.EqualTo("License content here"));
    }

    [Test]
    public void ResourceUri_DefaultValue_IsNull()
    {
        // Act
        var expander = new LicenseExpander();

        // Assert
        Assert.That(expander.ResourceUri, Is.Null);
    }

    [Test]
    public void MaxContentHeight_DefaultValue_IsPositiveInfinity()
    {
        // Act
        var expander = new LicenseExpander();

        // Assert
        Assert.That(expander.MaxContentHeight, Is.EqualTo(double.PositiveInfinity));
    }

    [Test]
    public void MaxContentHeight_SetValue_ReturnsSetValue()
    {
        // Arrange
        var expander = new LicenseExpander();

        // Act
        expander.MaxContentHeight = 300;

        // Assert
        Assert.That(expander.MaxContentHeight, Is.EqualTo(300));
    }

    [Test]
    public void HeaderProperty_IsDependencyProperty()
    {
        // Assert
        Assert.That(LicenseExpander.HeaderProperty, Is.Not.Null);
        Assert.That(LicenseExpander.HeaderProperty, Is.InstanceOf<DependencyProperty>());
    }

    [Test]
    public void LicenseTextProperty_IsDependencyProperty()
    {
        // Assert
        Assert.That(LicenseExpander.LicenseTextProperty, Is.Not.Null);
        Assert.That(LicenseExpander.LicenseTextProperty, Is.InstanceOf<DependencyProperty>());
    }

    [Test]
    public void ResourceUriProperty_IsDependencyProperty()
    {
        // Assert
        Assert.That(LicenseExpander.ResourceUriProperty, Is.Not.Null);
        Assert.That(LicenseExpander.ResourceUriProperty, Is.InstanceOf<DependencyProperty>());
    }

    [Test]
    public void MaxContentHeightProperty_IsDependencyProperty()
    {
        // Assert
        Assert.That(LicenseExpander.MaxContentHeightProperty, Is.Not.Null);
        Assert.That(LicenseExpander.MaxContentHeightProperty, Is.InstanceOf<DependencyProperty>());
    }
}
