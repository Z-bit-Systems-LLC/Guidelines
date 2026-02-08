using System.ComponentModel;

namespace ZBitSystems.Wpf.UI.Localization;

/// <summary>
/// Provides a binding that automatically updates when the culture changes.
/// Used internally by LocalizeExtension to enable dynamic language switching.
/// </summary>
public class LocalizedStringBinding : INotifyPropertyChanged
{
    private readonly string _key;

    /// <summary>
    /// Initializes a new instance of the LocalizedStringBinding.
    /// </summary>
    /// <param name="key">The resource key.</param>
    public LocalizedStringBinding(string key)
    {
        _key = key;

        // Subscribe to culture changes from the provider
        LocalizationService.Provider.PropertyChanged += OnProviderPropertyChanged;
    }

    /// <summary>
    /// Gets the current localized value for the key.
    /// </summary>
    public string Value => LocalizationService.Provider.GetString(_key);

    private void OnProviderPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // When culture changes in provider, notify that our Value property has changed
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Finalizer to unsubscribe from provider events.
    /// </summary>
    ~LocalizedStringBinding()
    {
        if (LocalizationService.IsProviderSet)
        {
            LocalizationService.Provider.PropertyChanged -= OnProviderPropertyChanged;
        }
    }
}
