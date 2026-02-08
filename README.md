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
- Built on WPF-UI 4.2.0
- Targets .NET 10.0-windows
- Comprehensive unit test coverage (39 tests)

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
- Coverage: 39 tests covering all converters and helpers

### Versioning
Version is managed in `Directory.Build.props`. Update `VersionPrefix` for new releases.

## License
Apache License 2.0

## Contributing
This is an internal Z-bit Systems repository. For questions or contributions, contact the development team.