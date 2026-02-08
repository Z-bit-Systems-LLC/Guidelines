using ZBitSystems.Wpf.UI.Helpers;

namespace ZBitSystems.Wpf.UI.Tests.Helpers;

[TestFixture]
public class RelayCommandTests
{
    [Test]
    public void Execute_WithParameter_CallsExecuteAction()
    {
        // Arrange
        object? executedParameter = null;
        var command = new RelayCommand(param => executedParameter = param);

        // Act
        command.Execute("test");

        // Assert
        Assert.That(executedParameter, Is.EqualTo("test"));
    }

    [Test]
    public void Execute_WithNullParameter_DoesNotCallExecuteAction()
    {
        // Arrange
        var executeCalled = false;
        var command = new RelayCommand(_ => executeCalled = true);

        // Act
        command.Execute(null);

        // Assert
        Assert.That(executeCalled, Is.False);
    }

    [Test]
    public void CanExecute_WithoutCanExecuteFunc_ReturnsTrue()
    {
        // Arrange
        var command = new RelayCommand(_ => { });

        // Act
        var result = command.CanExecute("test");

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void CanExecute_WithNullParameter_ReturnsFalse()
    {
        // Arrange
        var command = new RelayCommand(_ => { });

        // Act
        var result = command.CanExecute(null);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void CanExecute_WithCanExecuteFunc_ReturnsCanExecuteResult()
    {
        // Arrange
        var command = new RelayCommand(
            _ => { },
            param => param?.ToString() == "allowed");

        // Act
        var allowedResult = command.CanExecute("allowed");
        var deniedResult = command.CanExecute("denied");

        // Assert
        Assert.That(allowedResult, Is.True);
        Assert.That(deniedResult, Is.False);
    }

    [Test]
    public void Constructor_WithNullExecuteAction_ThrowsArgumentNullException()
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => new RelayCommand(null!));
    }

    [Test]
    public void CanExecuteChanged_CanBeSubscribedAndUnsubscribed()
    {
        // Arrange
        var command = new RelayCommand(_ => { });
        EventHandler? handler = (sender, e) => { };

        // Act & Assert - Should not throw
        Assert.DoesNotThrow(() =>
        {
            command.CanExecuteChanged += handler;
            command.CanExecuteChanged -= handler;
        });
    }
}
