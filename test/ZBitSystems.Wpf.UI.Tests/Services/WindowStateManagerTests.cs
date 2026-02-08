using System.Windows;
using Moq;
using ZBitSystems.Wpf.UI.Services;

namespace ZBitSystems.Wpf.UI.Tests.Services;

[TestFixture]
[Apartment(ApartmentState.STA)] // Required for WPF Window tests
public class WindowStateManagerTests
{
    private Mock<IWindowStateStorage> _mockStorage = null!;
    private Window _window = null!;
    private WindowStateManager _manager = null!;

    [SetUp]
    public void SetUp()
    {
        _mockStorage = new Mock<IWindowStateStorage>();
        _window = new Window();
        _manager = new WindowStateManager(_window, _mockStorage.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _window?.Close();
    }

    [Test]
    public void Constructor_WithNullWindow_ThrowsArgumentNullException()
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => new WindowStateManager(null!, _mockStorage.Object));
    }

    [Test]
    public void Constructor_WithNullStorage_ThrowsArgumentNullException()
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => new WindowStateManager(_window, null!));
    }

    [Test]
    public void RestoreWindowState_WithNoSavedPosition_CentersWindow()
    {
        // Arrange
        _mockStorage.Setup(s => s.WindowWidth).Returns(800);
        _mockStorage.Setup(s => s.WindowHeight).Returns(600);
        _mockStorage.Setup(s => s.WindowLeft).Returns((double?)null);
        _mockStorage.Setup(s => s.WindowTop).Returns((double?)null);
        _mockStorage.Setup(s => s.IsMaximized).Returns(false);

        // Act
        _manager.RestoreWindowState();

        // Assert
        Assert.That(_window.Width, Is.EqualTo(800));
        Assert.That(_window.Height, Is.EqualTo(600));
        Assert.That(_window.WindowState, Is.EqualTo(WindowState.Normal));
        // Position should be set (centered), but exact values depend on screen size
        Assert.That(_window.Left, Is.GreaterThanOrEqualTo(0));
        Assert.That(_window.Top, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public void RestoreWindowState_WithSavedPosition_RestoresPosition()
    {
        // Arrange
        var workArea = SystemParameters.WorkArea;
        var targetLeft = workArea.Left + 100;
        var targetTop = workArea.Top + 100;

        _mockStorage.Setup(s => s.WindowWidth).Returns(800);
        _mockStorage.Setup(s => s.WindowHeight).Returns(600);
        _mockStorage.Setup(s => s.WindowLeft).Returns(targetLeft);
        _mockStorage.Setup(s => s.WindowTop).Returns(targetTop);
        _mockStorage.Setup(s => s.IsMaximized).Returns(false);

        // Act
        _manager.RestoreWindowState();

        // Assert
        Assert.That(_window.Width, Is.EqualTo(800));
        Assert.That(_window.Height, Is.EqualTo(600));
        Assert.That(_window.Left, Is.EqualTo(targetLeft));
        Assert.That(_window.Top, Is.EqualTo(targetTop));
    }

    [Test]
    public void RestoreWindowState_WithMaximized_RestoresMaximizedState()
    {
        // Arrange
        _mockStorage.Setup(s => s.WindowWidth).Returns(800);
        _mockStorage.Setup(s => s.WindowHeight).Returns(600);
        _mockStorage.Setup(s => s.WindowLeft).Returns((double?)null);
        _mockStorage.Setup(s => s.WindowTop).Returns((double?)null);
        _mockStorage.Setup(s => s.IsMaximized).Returns(true);

        // Act
        _manager.RestoreWindowState();

        // Assert
        Assert.That(_window.WindowState, Is.EqualTo(WindowState.Maximized));
    }

    [Test]
    public void RestoreWindowState_WithTooLargeSize_ClampsToWorkArea()
    {
        // Arrange
        var workArea = SystemParameters.WorkArea;
        _mockStorage.Setup(s => s.WindowWidth).Returns(workArea.Width + 1000);
        _mockStorage.Setup(s => s.WindowHeight).Returns(workArea.Height + 1000);
        _mockStorage.Setup(s => s.WindowLeft).Returns((double?)null);
        _mockStorage.Setup(s => s.WindowTop).Returns((double?)null);
        _mockStorage.Setup(s => s.IsMaximized).Returns(false);

        // Act
        _manager.RestoreWindowState();

        // Assert - Should be clamped to work area size
        Assert.That(_window.Width, Is.LessThanOrEqualTo(workArea.Width));
        Assert.That(_window.Height, Is.LessThanOrEqualTo(workArea.Height));
    }

    [Test]
    public void RestoreWindowState_WithTooSmallSize_ClampsToMinimum()
    {
        // Arrange
        _mockStorage.Setup(s => s.WindowWidth).Returns(100); // Less than minimum 400
        _mockStorage.Setup(s => s.WindowHeight).Returns(100); // Less than minimum 300
        _mockStorage.Setup(s => s.WindowLeft).Returns((double?)null);
        _mockStorage.Setup(s => s.WindowTop).Returns((double?)null);
        _mockStorage.Setup(s => s.IsMaximized).Returns(false);

        // Act
        _manager.RestoreWindowState();

        // Assert - Should be clamped to minimum size
        Assert.That(_window.Width, Is.EqualTo(400));
        Assert.That(_window.Height, Is.EqualTo(300));
    }

    [Test]
    public void SaveWindowState_WithNormalWindow_SavesCurrentState()
    {
        // Arrange
        _window.Width = 850;
        _window.Height = 650;
        _window.Left = 200;
        _window.Top = 150;
        _window.WindowState = WindowState.Normal;

        // Act
        _manager.SaveWindowState();

        // Assert
        _mockStorage.VerifySet(s => s.WindowWidth = 850);
        _mockStorage.VerifySet(s => s.WindowHeight = 650);
        _mockStorage.VerifySet(s => s.WindowLeft = 200);
        _mockStorage.VerifySet(s => s.WindowTop = 150);
        _mockStorage.VerifySet(s => s.IsMaximized = false);
    }

    [Test]
    public void SaveWindowState_WithMaximizedWindow_SavesMaximizedStateOnly()
    {
        // Arrange
        _window.WindowState = WindowState.Maximized;

        // Act
        _manager.SaveWindowState();

        // Assert
        _mockStorage.VerifySet(s => s.IsMaximized = true);
        // Should NOT save size/position when maximized
        _mockStorage.VerifySet(s => s.WindowWidth = It.IsAny<double>(), Times.Never);
        _mockStorage.VerifySet(s => s.WindowHeight = It.IsAny<double>(), Times.Never);
        _mockStorage.VerifySet(s => s.WindowLeft = It.IsAny<double>(), Times.Never);
        _mockStorage.VerifySet(s => s.WindowTop = It.IsAny<double>(), Times.Never);
    }

    [Test]
    public void SaveWindowState_WithMinimizedWindow_DoesNotSaveSize()
    {
        // Arrange
        _window.WindowState = WindowState.Minimized;

        // Act
        _manager.SaveWindowState();

        // Assert
        _mockStorage.VerifySet(s => s.IsMaximized = false);
        // Should NOT save size/position when minimized
        _mockStorage.VerifySet(s => s.WindowWidth = It.IsAny<double>(), Times.Never);
        _mockStorage.VerifySet(s => s.WindowHeight = It.IsAny<double>(), Times.Never);
    }
}
