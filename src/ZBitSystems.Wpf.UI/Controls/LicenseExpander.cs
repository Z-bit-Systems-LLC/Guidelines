using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ZBitSystems.Wpf.UI.Controls;

/// <summary>
/// A reusable control that displays license text inside an expandable card.
/// License content can be set directly via <see cref="LicenseText"/> or loaded
/// automatically from a resource URI via <see cref="ResourceUri"/>.
/// </summary>
/// <example>
/// <code>
/// &lt;controls:LicenseExpander Header="Apache License 2.0"
///     ResourceUri="pack://application:,,,/Assets/Apache.txt" /&gt;
/// </code>
/// </example>
public class LicenseExpander : Control
{
    static LicenseExpander()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(LicenseExpander),
            new FrameworkPropertyMetadata(typeof(LicenseExpander)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LicenseExpander"/> class.
    /// </summary>
    public LicenseExpander()
    {
        Loaded += OnLoaded;
    }

    /// <summary>
    /// Identifies the <see cref="Header"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(LicenseExpander),
            new PropertyMetadata(string.Empty));

    /// <summary>
    /// Gets or sets the header text displayed on the expander.
    /// </summary>
    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="LicenseText"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty LicenseTextProperty =
        DependencyProperty.Register(
            nameof(LicenseText),
            typeof(string),
            typeof(LicenseExpander),
            new PropertyMetadata(string.Empty));

    /// <summary>
    /// Gets or sets the license text content to display.
    /// </summary>
    public string LicenseText
    {
        get => (string)GetValue(LicenseTextProperty);
        set => SetValue(LicenseTextProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="ResourceUri"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ResourceUriProperty =
        DependencyProperty.Register(
            nameof(ResourceUri),
            typeof(Uri),
            typeof(LicenseExpander),
            new PropertyMetadata(null, OnResourceUriChanged));

    /// <summary>
    /// Gets or sets a pack:// URI to a text resource. When set, the control
    /// automatically loads the resource content into <see cref="LicenseText"/>.
    /// </summary>
    public Uri? ResourceUri
    {
        get => (Uri?)GetValue(ResourceUriProperty);
        set => SetValue(ResourceUriProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="MaxContentHeight"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty MaxContentHeightProperty =
        DependencyProperty.Register(
            nameof(MaxContentHeight),
            typeof(double),
            typeof(LicenseExpander),
            new PropertyMetadata(double.PositiveInfinity));

    /// <summary>
    /// Gets or sets the maximum height of the scrollable license content area.
    /// Defaults to <see cref="double.PositiveInfinity"/> (no constraint).
    /// </summary>
    public double MaxContentHeight
    {
        get => (double)GetValue(MaxContentHeightProperty);
        set => SetValue(MaxContentHeightProperty, value);
    }

    private static void OnResourceUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LicenseExpander { IsLoaded: true } expander && e.NewValue is Uri uri)
        {
            expander.LoadResource(uri);
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (ResourceUri != null && string.IsNullOrEmpty(LicenseText))
        {
            LoadResource(ResourceUri);
        }
    }

    private void LoadResource(Uri uri)
    {
        var info = Application.GetResourceStream(uri);
        if (info != null)
        {
            using StreamReader reader = new(info.Stream);
            LicenseText = reader.ReadToEnd();
        }
    }
}
