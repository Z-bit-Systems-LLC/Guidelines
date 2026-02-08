# Z-bit Systems Guidelines

This repository contains shared company guidelines and reusable WPF components for Z-bit Systems projects.

## Contents

### Documentation (`docs/`)
- **azure-devops-ci-guide.md** - Guide for setting up Azure DevOps CI/CD pipelines
- **readme-template.md** - Template for creating project README files

### Shared WPF Library (`src/ZBitSystems.Wpf.UI/`)
A reusable WPF component library providing:
- **Design System** - Consistent styling and design tokens
- **Converters** - Generic value converters for data binding
- **Helpers** - Utility classes for common WPF scenarios
- **Localization** - Interface-based abstraction for multi-language support
- **Window Management** - State persistence with multi-monitor support
- **Theme Management** - Automatic Windows system theme following
- Built on WPF-UI 4.2.0
- Targets .NET 10.0-windows
- Comprehensive unit test coverage (75 tests)

#### Features

**Design System:**
- **Design Tokens** - Centralized spacing, sizing, colors, and typography
- **Semantic Colors** - Theme-aware color system with automatic light/dark mode support
- **Component Styles** - Pre-styled controls (buttons, textboxes, cards, badges, etc.)

**Converters:**
- **BooleanToVisibilityConverter** - Convert bool to Visibility with optional invert parameter
- **InverseBoolConverter** - Invert boolean values
- **NullToVisibilityConverter** - Show/hide based on null check
- **StringToVisibilityConverter** - Show/hide based on string match (supports semicolon-separated values)
- **IndexToVisibilityConverter** - Show/hide based on index match (supports pipe-separated indices)

**Helpers:**
- **CopyTextBoxHelper** - Attached property for copy-to-clipboard functionality on read-only textboxes
- **RelayCommand** - Simple ICommand implementation for MVVM patterns

**Localization:**
- **ILocalizationProvider** - Interface for implementing custom resource providers
- **LocalizationService** - Static service for managing the localization provider
- **LocalizeExtension** - XAML markup extension for accessing localized resources
- **LocalizedStringBinding** - Dynamic binding that updates when culture changes

**Window Management:**
- **IWindowStateStorage** - Interface for implementing window state persistence
- **WindowStateManager** - Manages window position, size, and maximized state
  - Multi-monitor support with smart positioning
  - Ensures windows remain accessible across monitor configuration changes
  - Handles DPI scaling correctly

**Theme Management:**
- **ThemeManager** - Automatically follows Windows OS theme settings
  - Provides `IsDarkMode` property for theme-aware UI
  - Raises `PropertyChanged` events when system theme changes
  - Built on WPF-UI's ApplicationThemeManager

## Usage

### As a Git Submodule
Add to your project:
```bash
git submodule add https://github.com/Z-bit-Systems-LLC/Guidelines.git lib/Guidelines
git submodule update --init --recursive
```

### Referencing the Library
Add to your .sln:
```bash
dotnet sln add lib/Guidelines/src/ZBitSystems.Wpf.UI/ZBitSystems.Wpf.UI.csproj
```

Add to your WPF project's .csproj:
```xml
<ProjectReference Include="..\..\lib\Guidelines\src\ZBitSystems.Wpf.UI\ZBitSystems.Wpf.UI.csproj" />
```

### Using the Design System
In your `App.xaml`:
```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <!-- WPF-UI Theme -->
            <ui:ThemesDictionary Theme="Dark"/>
            <ui:ControlsDictionary/>

            <!-- Z-bit Systems Design System -->
            <ResourceDictionary Source="pack://application:,,,/ZBitSystems.Wpf.UI;component/Styles/DesignTokens.xaml"/>
            <ResourceDictionary Source="pack://application:,,,/ZBitSystems.Wpf.UI;component/Styles/ThemeSemanticColors.xaml"/>
            <ResourceDictionary Source="pack://application:,,,/ZBitSystems.Wpf.UI;component/Styles/ComponentStyles.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### Using Converters
Add namespace in your XAML:
```xml
xmlns:converters="clr-namespace:ZBitSystems.Wpf.UI.Converters;assembly=ZBitSystems.Wpf.UI"
```

Use in bindings:
```xml
<Page.Resources>
    <converters:BooleanToVisibilityConverter x:Key="BoolToVis" />
</Page.Resources>

<TextBlock Visibility="{Binding IsEnabled, Converter={StaticResource BoolToVis}}" />
```

### Using Localization

The localization system provides an interface-based abstraction that allows applications to implement their own resource management strategy.

**Step 1: Implement ILocalizationProvider**

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

**Step 2: Register Provider at Startup**

In your `App.xaml.cs`:
```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);

    // Set the localization provider before any windows are shown
    LocalizationService.Provider = new ResourceLocalizationProvider();
}
```

**Step 3: Use in XAML**

Add namespace:
```xml
xmlns:localization="clr-namespace:ZBitSystems.Wpf.UI.Localization;assembly=ZBitSystems.Wpf.UI"
```

Use the markup extension:
```xml
<TextBlock Text="{localization:Localize Welcome_Message}" />
<Button Content="{localization:Localize Button_Submit}" />
```

The bindings automatically update when the provider raises PropertyChanged, enabling dynamic language switching without restarting the application.

### Using Window State Management

The window state manager persists window position, size, and maximized state across application sessions.

**Step 1: Implement IWindowStateStorage**

Create a storage adapter for your settings class:
```csharp
using ZBitSystems.Wpf.UI.Services;

public class UserSettingsWindowStateAdapter : IWindowStateStorage
{
    private readonly UserSettings _settings;

    public UserSettingsWindowStateAdapter(UserSettings settings)
    {
        _settings = settings;
    }

    public double WindowWidth
    {
        get => _settings.WindowWidth;
        set => _settings.WindowWidth = value;
    }

    // Implement other properties: WindowHeight, WindowLeft, WindowTop, IsMaximized
}
```

**Step 2: Use in Window Constructor**

```csharp
public MainWindow(IUserSettingsService userSettingsService)
{
    InitializeComponent();

    var adapter = new UserSettingsWindowStateAdapter(userSettingsService.Settings);
    var windowStateManager = new WindowStateManager(this, adapter);

    // Restore window state before showing
    windowStateManager.RestoreWindowState();

    // Save state when closing
    Closing += async (s, e) =>
    {
        windowStateManager.SaveWindowState();
        await userSettingsService.SaveAsync();
    };
}
```

The manager automatically handles:
- Multi-monitor configurations
- DPI scaling
- Window position clamping to keep windows accessible
- Centering when saved position is invalid

### Using Theme Management

The theme manager automatically follows Windows OS theme settings (Light/Dark mode).

**Basic Usage:**

```csharp
using ZBitSystems.Wpf.UI.Services;

public class MyPage : INotifyPropertyChanged, IDisposable
{
    private readonly ThemeManager _themeManager;

    public MyPage()
    {
        _themeManager = new ThemeManager();
        _themeManager.PropertyChanged += OnThemeChanged;
    }

    public bool IsDarkMode => _themeManager.IsDarkMode;

    private void OnThemeChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ThemeManager.IsDarkMode))
        {
            // Update UI when theme changes
            OnPropertyChanged(nameof(IsDarkMode));
        }
    }

    public void Dispose()
    {
        _themeManager.PropertyChanged -= OnThemeChanged;
        _themeManager.Dispose();
    }
}
```

**In XAML:**

```xml
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
```

### Using Components
See `src/ZBitSystems.Wpf.UI/Styles/StyleGuide.md` for:
- Available styles and their usage
- Design token reference
- Component examples
- Best practices

## Development

### Building
```bash
dotnet build Guidelines.slnx
```

### Testing
Run unit tests:
```bash
dotnet test
```

Test project: `test/ZBitSystems.Wpf.UI.Tests/`
- Framework: NUnit 4.3.2
- Mocking: Moq 4.20.72
- Coverage: 75 tests covering converters, helpers, localization, window management, and theme management

### Versioning
Version is managed in `Directory.Build.props`. Update `VersionPrefix` for new releases.

## License
Apache License 2.0

## Contributing
This is an internal Z-bit Systems repository. For questions or contributions, contact the development team.