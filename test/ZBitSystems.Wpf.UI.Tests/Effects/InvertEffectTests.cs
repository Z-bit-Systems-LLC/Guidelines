using System.Windows;
using System.Windows.Media.Effects;
using ZBitSystems.Wpf.UI.Effects;

namespace ZBitSystems.Wpf.UI.Tests.Effects;

[TestFixture]
[Apartment(ApartmentState.STA)]
public class InvertEffectTests
{
    [Test]
    public void Constructor_CreatesInstance()
    {
        // Act
        var effect = new InvertEffect();

        // Assert
        Assert.That(effect, Is.Not.Null);
    }

    [Test]
    public void Constructor_CreatesShaderEffect()
    {
        // Act
        var effect = new InvertEffect();

        // Assert
        Assert.That(effect, Is.InstanceOf<ShaderEffect>());
    }

    [Test]
    public void InputProperty_IsDependencyProperty()
    {
        // Assert
        Assert.That(InvertEffect.InputProperty, Is.Not.Null);
        Assert.That(InvertEffect.InputProperty, Is.InstanceOf<DependencyProperty>());
    }

    [Test]
    public void MultipleInstances_DoNotThrow()
    {
        // Act & Assert - creating multiple instances should reuse the static shader
        Assert.DoesNotThrow(() =>
        {
            _ = new InvertEffect();
            _ = new InvertEffect();
        });
    }
}
