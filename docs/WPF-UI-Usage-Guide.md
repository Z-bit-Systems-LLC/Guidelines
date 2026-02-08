# ZBitSystems.Wpf.UI Usage Guide

This guide provides detailed instructions for using the shared WPF component library in your applications.

## Table of Contents
- [Getting Started](#getting-started)
- [Design System](#design-system)
- [Converters](#converters)
- [Localization](#localization)
- [Window State Management](#window-state-management)
- [Theme Management](#theme-management)
- [Components](#components)

## Getting Started

### Installation

Add as a git submodule:
```bash
git submodule add https://github.com/Z-bit-Systems-LLC/Guidelines.git lib/Guidelines
git submodule update --init --recursive
```

Add to your solution:
```bash
dotnet sln add lib/Guidelines/src/ZBitSystems.Wpf.UI/ZBitSystems.Wpf.UI.csproj
```

Add project reference to your WPF project's .csproj:
```xml
<ProjectReference Include="..\..\lib\Guidelines\src\ZBitSystems.Wpf.UI\ZBitSystems.Wpf.UI.csproj" />
```

## Design System

### Setting Up Styles

In your `App.xaml`, merge the design system resource dictionaries:

```xml
<Application x:Class="YourApp.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!-- WPF-UI Theme (required) -->
                <ui:ThemesDictionary Theme="Dark"/>
                <ui:ControlsDictionary/>

                <!-- Z-bit Systems Design System -->
                <ResourceDictionary Source="pack://application:,,,/ZBitSystems.Wpf.UI;component/Styles/DesignTokens.xaml"/>
                <ResourceDictionary Source="pack://application:,,,/ZBitSystems.Wpf.UI;component/Styles/ThemeSemanticColors.xaml"/>
                <ResourceDictionary Source="pack://application:,,,/ZBitSystems.Wpf.UI;component/Styles/ComponentStyles.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

### Design Tokens

The design system provides centralized tokens for spacing, sizing, colors, and typography:

- **Spacing:** `{StaticResource Margin.Card}`, `{StaticResource Padding.Default}`, etc.
- **Sizes:** `{StaticResource Size.Icon.Default}`, `{StaticResource Size.Button.Height}`, etc.
- **Colors:** `{DynamicResource SemanticSuccessBrush}`, `{DynamicResource SemanticErrorBrush}`, etc.
- **Typography:** `{StaticResource Text.Title}`, `{StaticResource Text.Body}`, etc.

For detailed token reference, see `src/ZBitSystems.Wpf.UI/Styles/StyleGuide.md`.

## Converters

The library provides generic value converters for common data binding scenarios.

### Adding Converter Namespace

Add this namespace to your XAML:
```xml
xmlns:converters="clr-namespace:ZBitSystems.Wpf.UI.Converters;assembly=ZBitSystems.Wpf.UI"
```

### Available Converters

#### BooleanToVisibilityConverter
Converts boolean values to Visibility. Supports inverting.

```xml
<Page.Resources>
    <converters:BooleanToVisibilityConverter x:Key="BoolToVis" />
</Page.Resources>

<!-- Show when true -->
<TextBlock Text="Visible when true"
           Visibility="{Binding IsEnabled, Converter={StaticResource BoolToVis}}" />

<!-- Show when false (inverted) -->
<TextBlock Text="Visible when false"
           Visibility="{Binding IsEnabled, Converter={StaticResource BoolToVis}, ConverterParameter=Invert}" />
```

#### InverseBoolConverter
Inverts boolean values.

```xml
<CheckBox IsChecked="{Binding IsDisabled, Converter={StaticResource InverseBool}}" />
```

#### NullToVisibilityConverter
Shows/hides elements based on null check.

```xml
<!-- Show when not null -->
<TextBlock Text="{Binding ErrorMessage}"
           Visibility="{Binding ErrorMessage, Converter={StaticResource NullToVis}}" />

<!-- Show when null (inverted) -->
<TextBlock Text="No errors"
           Visibility="{Binding ErrorMessage, Converter={StaticResource NullToVis}, ConverterParameter=Invert}" />
```

#### StringToVisibilityConverter
Shows element when string matches specific values (semicolon-separated).

```xml
<!-- Show when Status is "Active" or "Running" -->
<Border Visibility="{Binding Status, Converter={StaticResource StringToVis}, ConverterParameter='Active;Running'}">
    <TextBlock Text="Currently active" />
</Border>
```

#### IndexToVisibilityConverter
Shows element when index matches specific values (pipe-separated).

```xml
<!-- Show when SelectedIndex is 0 or 2 -->
<StackPanel Visibility="{Binding SelectedIndex, Converter={StaticResource IndexToVis}, ConverterParameter='0|2'}">
    <!-- Content -->
</StackPanel>
```

## Localization

The localization system provides an interface-based abstraction for multi-language support.

### Step 1: Implement ILocalizationProvider

Create a provider that implements the interface:

```csharp
using System.ComponentModel;
using System.Globalization;
using ZBitSystems.Wpf.UI.Localization;

public class ResourceLocalizationProvider : ILocalizationProvider
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string CurrentCulture => CultureInfo.CurrentUICulture.Name;

    public string GetString(string key)
    {
        // Your resource lookup logic (e.g., ResourceManager, database, etc.)
        return Resources.ResourceManager.GetString(key) ?? $"[{key}]";
    }

    public void ChangeCulture(CultureInfo culture)
    {
        // Update culture and notify listeners
        CultureInfo.CurrentUICulture = culture;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
    }
}
```

### Step 2: Register Provider at Startup

In your `App.xaml.cs`:

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);

    // Set the localization provider before any windows are shown
    LocalizationService.Provider = new ResourceLocalizationProvider();
}
```

### Step 3: Use in XAML

Add namespace:
```xml
xmlns:localization="clr-namespace:ZBitSystems.Wpf.UI.Localization;assembly=ZBitSystems.Wpf.UI"
```

Use the markup extension:
```xml
<TextBlock Text="{localization:Localize Welcome_Message}" />
<Button Content="{localization:Localize Button_Submit}" />
<TextBox Watermark="{localization:Localize Placeholder_EnterName}" />
```

### Dynamic Language Switching

The bindings automatically update when the provider raises `PropertyChanged`:

```csharp
// Switch to Spanish
var spanishCulture = new CultureInfo("es");
localizationProvider.ChangeCulture(spanishCulture);
// All UI elements using {localization:Localize} will update automatically
```

## Window State Management

The window state manager persists window position, size, and maximized state across application sessions.

### Step 1: Implement IWindowStateStorage

Create a storage adapter for your settings class. This keeps your Core/business logic independent from WPF-specific types:

```csharp
using ZBitSystems.Wpf.UI.Services;

public class UserSettingsWindowStateAdapter : IWindowStateStorage
{
    private readonly UserSettings _settings;

    public UserSettingsWindowStateAdapter(UserSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public double WindowWidth
    {
        get => _settings.WindowWidth;
        set => _settings.WindowWidth = value;
    }

    public double WindowHeight
    {
        get => _settings.WindowHeight;
        set => _settings.WindowHeight = value;
    }

    public double? WindowLeft
    {
        get => _settings.WindowLeft;
        set => _settings.WindowLeft = value;
    }

    public double? WindowTop
    {
        get => _settings.WindowTop;
        set => _settings.WindowTop = value;
    }

    public bool IsMaximized
    {
        get => _settings.IsMaximized;
        set => _settings.IsMaximized = value;
    }
}
```

### Step 2: Use in Window Constructor

```csharp
using ZBitSystems.Wpf.UI.Services;

public partial class MainWindow : Window
{
    private readonly IUserSettingsService _userSettingsService;
    private readonly WindowStateManager _windowStateManager;

    public MainWindow(IUserSettingsService userSettingsService)
    {
        _userSettingsService = userSettingsService;

        // Create adapter to bridge your settings with the window state manager
        var adapter = new UserSettingsWindowStateAdapter(userSettingsService.Settings);
        _windowStateManager = new WindowStateManager(this, adapter);

        InitializeComponent();

        // Restore window state before showing
        _windowStateManager.RestoreWindowState();

        // Save state when closing
        Closing += MainWindow_Closing;
    }

    private async void MainWindow_Closing(object? sender, CancelEventArgs e)
    {
        _windowStateManager.SaveWindowState();
        await _userSettingsService.SaveAsync();
    }
}
```

### Features

The manager automatically handles:
- **Multi-monitor configurations** - Works correctly when displays are added/removed
- **DPI scaling** - Handles high-DPI displays correctly
- **Position clamping** - Ensures windows remain accessible and visible
- **Smart centering** - Centers window when saved position is invalid
- **Normal bounds preservation** - Saves normal (non-maximized) bounds when window is maximized

## Theme Management

The theme manager automatically follows Windows OS theme settings (Light/Dark mode).

### Basic Usage

```csharp
using System.ComponentModel;
using ZBitSystems.Wpf.UI.Services;

public class MyPage : INavigableView, INotifyPropertyChanged, IDisposable
{
    private readonly ThemeManager _themeManager;

    public MyPage()
    {
        _themeManager = new ThemeManager();
        _themeManager.PropertyChanged += OnThemeChanged;

        InitializeComponent();
    }

    public bool IsDarkMode => _themeManager.IsDarkMode;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnThemeChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ThemeManager.IsDarkMode))
        {
            // Update UI when theme changes
            OnPropertyChanged(nameof(IsDarkMode));
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Dispose()
    {
        _themeManager.PropertyChanged -= OnThemeChanged;
        _themeManager.Dispose();
    }
}
```

### Using in XAML

React to theme changes with data binding:

```xml
<Page DataContext="{Binding RelativeSource={RelativeSource Self}}">
    <!-- Invert logo colors in dark mode -->
    <Image Source="logo.png">
        <Image.Style>
            <Style TargetType="Image">
                <Style.Triggers>
                    <DataTrigger Binding="{Binding IsDarkMode}" Value="True">
                        <Setter Property="Effect">
                            <Setter.Value>
                                <InvertEffect />
                            </Setter.Value>
                        </Setter>
                    </DataTrigger>
                </Style.Triggers>
            </Style>
        </Image.Style>
    </Image>

    <!-- Show different text based on theme -->
    <TextBlock>
        <TextBlock.Style>
            <Style TargetType="TextBlock">
                <Setter Property="Text" Value="Light Mode Active" />
                <Style.Triggers>
                    <DataTrigger Binding="{Binding IsDarkMode}" Value="True">
                        <Setter Property="Text" Value="Dark Mode Active" />
                    </DataTrigger>
                </Style.Triggers>
            </Style>
        </TextBlock.Style>
    </TextBlock>
</Page>
```

### Integration with MainWindow

For theme watching in your main window:

```csharp
public MainWindow()
{
    // Register for system theme changes
    // Note: This is already handled by WPF-UI's SystemThemeWatcher
    Wpf.Ui.Appearance.SystemThemeWatcher.Watch(this);

    InitializeComponent();
}
```

## Components

For detailed information about available component styles:

- **StyleGuide.md** - Complete style guide with examples
  - Available text styles (Title, Headline, Subtitle, Body, Caption)
  - Button styles (Primary, Secondary, Accent, Danger)
  - Card and container styles
  - Badge and status indicator styles
  - Form control styles
  - Best practices and usage guidelines

See: `src/ZBitSystems.Wpf.UI/Styles/StyleGuide.md`

## Architecture Best Practices

### Keep Core Projects UI-Independent

When integrating with applications that have a Core/business logic layer:

❌ **Don't:** Make Core types implement WPF interfaces
```csharp
// Bad: Creates dependency from Core to WPF library
public class UserSettings : IWindowStateStorage { }
```

✅ **Do:** Create adapters in your UI project
```csharp
// Good: Adapter in UI project, Core remains independent
public class UserSettingsWindowStateAdapter : IWindowStateStorage
{
    private readonly UserSettings _settings;
    // Delegate to Core type
}
```

This keeps your Core project platform-agnostic and testable without UI dependencies.
