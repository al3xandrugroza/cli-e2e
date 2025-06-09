# CLI E2E Examples Plugin for JetBrains Rider

A plugin that provides easy access to CLI E2E test examples through a searchable action system.

## Features

- Access via **Ctrl+Shift+A** → search for "CLI E2E Examples"
- Categories available:
  - Analyze
  - Deploy
  - Manage Assets
  - Pack
  - Run Job
  - Run Tests

## Prerequisites

1. **JDK 21**
   - Download from [Adoptium](https://adoptium.net/)

2. **Gradle** (for initial setup)
   - Install via Chocolatey: `choco install gradle`

## Building the Plugin

1. Initialize the Gradle wrapper (first time only):
   ```bash
   gradle wrapper
   ```
2. Build the plugin:
   ```bash
   ./gradlew.bat buildPlugin
   ```
3. The plugin ZIP will be created in `build/distributions/`

## Installing in Rider

1. Open Rider
2. Go to **File** → **Settings** → **Plugins**
3. Click the gear icon → **Install Plugin from Disk...**
4. Select the generated ZIP file from `build/distributions/`
5. Restart Rider

## Usage

1. Press **Ctrl+Shift+A**
2. Type "CLI E2E Examples"
3. Select the action from the list
4. Choose a category from the dialog
5. View the examples in the read-only editor
6. Copy the examples you need