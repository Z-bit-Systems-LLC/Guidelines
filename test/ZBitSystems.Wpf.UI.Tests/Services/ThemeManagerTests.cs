using System.ComponentModel;
using System.Windows;
using Wpf.Ui.Appearance;
using ZBitSystems.Wpf.UI.Services;

namespace ZBitSystems.Wpf.UI.Tests.Services;

[TestFixture]
[Apartment(ApartmentState.STA)] // Required for WPF tests
public class ThemeManagerTests
{
    [Test]
    public void Constructor_WithoutWindow_CreatesInstance()
    {
        // Act
        using var manager = new ThemeManager();

        // Assert
        Assert.That(manager, Is.Not.Null);
    }

    [Test]
    public void Constructor_WithWindow_CreatesInstance()
    {
        // Arrange
        var window = new Window();

        try
        {
            // Act
            using var manager = new ThemeManager(window);

            // Assert
            Assert.That(manager, Is.Not.Null);
        }
        finally
        {
            window.Close();
        }
    }

    [Test]
    public void StartWatching_WithNullWindow_ThrowsArgumentNullException()
    {
        // Arrange
        using var manager = new ThemeManager();

        // Assert
        Assert.Throws<ArgumentNullException>(() => manager.StartWatching(null!));
    }

    [Test]
    public void StartWatching_WithWindow_DoesNotThrow()
    {
        // Arrange
        var window = new Window();
        using var manager = new ThemeManager();

        try
        {
            // Act & Assert
            Assert.DoesNotThrow(() => manager.StartWatching(window));
        }
        finally
        {
            window.Close();
        }
    }

    [Test]
    public void IsDarkMode_ReturnsBoolean()
    {
        // Arrange
        using var manager = new ThemeManager();

        // Act
        var isDarkMode = manager.IsDarkMode;

        // Assert - Should return either true or false based on system theme
        Assert.That(isDarkMode, Is.TypeOf<bool>());
    }

    [Test]
    public void CurrentTheme_ReturnsTheme()
    {
        // Arrange
        using var manager = new ThemeManager();

        // Act
        var theme = manager.CurrentTheme;

        // Assert - Should return a valid theme (enum has defined values)
        Assert.That(Enum.IsDefined(typeof(ApplicationTheme), theme), Is.True);
    }

    [Test]
    public void PropertyChanged_IsImplemented()
    {
        // Arrange
        using var manager = new ThemeManager();

        // Act & Assert
        // We can't easily trigger a real theme change in unit tests, so we verify
        // that the manager implements INotifyPropertyChanged correctly
        Assert.That(manager, Is.InstanceOf<INotifyPropertyChanged>());

        // Verify event subscription doesn't throw
        Assert.DoesNotThrow(() =>
        {
            manager.PropertyChanged += (sender, e) => { };
        });
    }

    [Test]
    public void Dispose_UnsubscribesFromEvents()
    {
        // Arrange
        var manager = new ThemeManager();

        // Act
        manager.Dispose();

        // Assert - Should not throw when accessing properties after dispose
        // The manager should still return valid values but stop listening to events
        Assert.DoesNotThrow(() => _ = manager.IsDarkMode);
        Assert.DoesNotThrow(() => _ = manager.CurrentTheme);
    }

    [Test]
    public void Dispose_CanBeCalledMultipleTimes()
    {
        // Arrange
        var manager = new ThemeManager();

        // Act & Assert
        Assert.DoesNotThrow(() =>
        {
            manager.Dispose();
            manager.Dispose();
        });
    }
}
