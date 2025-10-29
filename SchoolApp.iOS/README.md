# School Management iOS App

A native iOS application built with .NET MAUI that connects to the School Management API.

## Overview

This iOS app provides a mobile interface for managing school classes and students. It consumes the SchoolApi backend at `http://localhost:5178/` and displays:
- 5 classes with their enrolled students
- 20 students with their class assignments
- Real-time data from the MySQL database

## Features

- **Home Page**: Dashboard with quick stats and API connection status
- **Classes Page**: View all classes with nested student lists in card layout
- **Students Page**: Browse all students with search/filter functionality
- **Pull-to-Refresh**: Refresh data on any page
- **Native iOS UI**: Built with .NET MAUI for optimal performance

## Prerequisites

### Required Software

1. **Xcode** (Latest version)
   - Download from Mac App Store
   - Required for iOS development and simulators
   - Includes iOS SDK and build tools

2. **.NET 8 SDK** (Already installed)
   ```bash
   dotnet --version  # Should show 8.0.415 or later
   ```

3. **.NET MAUI Workload** (Already installed)
   ```bash
   dotnet workload list  # Should show maui, ios, android, etc.
   ```

### Xcode Setup

After installing Xcode, run these commands:

```bash
# Set Xcode command line tools path
sudo xcode-select -s /Applications/Xcode.app/Contents/Developer

# Accept Xcode license
sudo xcodebuild -license accept

# Install iOS simulator (if not already installed)
xcodebuild -downloadPlatform iOS
```

## Project Structure

```
SchoolApp.iOS/
├── Models/
│   ├── Classroom.cs       # Class model (Id, Name, Students)
│   └── Student.cs         # Student model (Id, Name, ClassroomId)
├── Services/
│   └── SchoolApiService.cs # HTTP client for API communication
├── Pages/
│   ├── HomePage.xaml      # Dashboard with stats and navigation
│   ├── ClassesPage.xaml   # Classes with student lists
│   └── StudentsPage.xaml  # All students with search
├── Platforms/
│   └── iOS/
│       └── Info.plist     # iOS-specific configuration
├── AppShell.xaml          # Navigation structure with tabs
├── MauiProgram.cs         # Dependency injection setup
└── SchoolApp.iOS.csproj   # Project file
```

## Configuration

### API Endpoint Configuration

The app connects to the backend API at:
- **iOS Simulator**: `http://localhost:5178/`
- **Physical Device**: `http://YOUR_COMPUTER_IP:5178/`

To change the API URL, edit `Services/SchoolApiService.cs`:

```csharp
// For iOS Simulator
private const string BaseUrl = "http://localhost:5178/";

// For Physical Device (replace with your computer's IP)
// private const string BaseUrl = "http://192.168.1.100:5178/";
```

**Finding Your Computer's IP Address**:
```bash
# macOS
ipconfig getifaddr en0  # WiFi
ipconfig getifaddr en1  # Ethernet
```

### Network Permissions

The `Info.plist` file is already configured to allow localhost connections:

```xml
<key>NSAppTransportSecurity</key>
<dict>
    <key>NSAllowsLocalNetworking</key>
    <true/>
    <key>NSAllowsArbitraryLoads</key>
    <true/>
</dict>
```

⚠️ **Note**: `NSAllowsArbitraryLoads` is for development only. For production, configure specific domain exceptions.

## Building and Running

### Start the Backend API

Before running the iOS app, ensure the backend API is running:

```bash
cd ../SchoolApi
dotnet run
```

The API should be accessible at `http://localhost:5178/`

### Build the iOS App

```bash
cd SchoolApp.iOS

# Build for iOS
dotnet build -f net8.0-ios
```

### Run on iOS Simulator

```bash
# List available iOS simulators
xcrun simctl list devices

# Run the app (will launch default simulator)
dotnet build -t:Run -f net8.0-ios

# Or specify a simulator
dotnet build -t:Run -f net8.0-ios -p:_DeviceName=":v2:runtime=com.apple.CoreSimulator.SimRuntime.iOS-18-0,devicetype=com.apple.CoreSimulator.SimDeviceType.iPhone-15"
```

### Run on Physical Device

1. Connect your iPhone/iPad via USB
2. Trust your Mac on the device
3. Configure a provisioning profile in Xcode
4. Update the API URL in `SchoolApiService.cs` to use your Mac's IP address
5. Run:
   ```bash
   dotnet build -t:Run -f net8.0-ios -p:RuntimeIdentifier=ios-arm64
   ```

## Using the App

### Home Page
- View connection status to the API
- See quick stats (5 classes, 20 students)
- Navigate to Classes or Students pages
- Test API connection with a button

### Classes Page
- Scroll through all 5 classes
- Each class card shows:
  - Class name
  - List of enrolled students (4 per class)
  - Total student count
- Pull down to refresh data

### Students Page
- View all 20 students in a scrollable list
- Each student card shows:
  - Student name
  - Student ID
  - Class assignment
- Search by name, ID, or class number
- Pull down to refresh data

## Troubleshooting

### Build Errors

**"Could not find a valid Xcode app bundle"**
```bash
# Verify Xcode installation
xcode-select -p  # Should show /Applications/Xcode.app/Contents/Developer

# If not, set it:
sudo xcode-select -s /Applications/Xcode.app/Contents/Developer
```

**"No iOS simulators found"**
```bash
# Open Xcode and download iOS simulators
# Xcode → Settings → Platforms → iOS → Download

# Or use command line
xcodebuild -downloadPlatform iOS
```

### API Connection Issues

**"Failed to connect to API"**
1. Check backend is running: `curl http://localhost:5178/health`
2. For simulator, use `localhost`
3. For physical device:
   - Use Mac's IP address (not localhost)
   - Ensure Mac and device are on the same network
   - Check firewall settings

**JSON Deserialization Errors**
- Ensure backend is running version with `[JsonIgnore]` on `Student.Classroom`
- Check API responses: `curl http://localhost:5178/api/classes`

### Simulator Issues

**Simulator runs but app crashes**
- Check Console output in Xcode (Window → Devices and Simulators)
- Look for exceptions in Terminal output

**Simulator is slow**
- Allocate more CPU/RAM in Xcode
- Use newer iOS version (iOS 17+)
- Close other applications

## Development Commands

```bash
# Clean build artifacts
dotnet clean

# Restore dependencies
dotnet restore

# Build without running
dotnet build -f net8.0-ios

# Run with detailed logs
dotnet build -t:Run -f net8.0-ios -v:detailed

# List all MAUI workloads
dotnet workload list

# Update MAUI workloads
sudo dotnet workload update
```

## API Endpoints Used

The app consumes these backend endpoints:

- `GET /health` - Health check (tests connection)
- `GET /api/classes` - Get all classes with students
- `GET /api/students` - Get all students

## Architecture

### Service Layer
- **SchoolApiService**: Singleton service for API communication
  - Uses `HttpClient` with 30-second timeout
  - Implements `GetClassesAsync()`, `GetStudentsAsync()`, `TestConnectionAsync()`
  - Handles errors with try-catch and debug logging

### Pages
- All pages use **MVVM-like pattern** with code-behind
- Implement `INotifyPropertyChanged` for data binding
- Use `ObservableCollection` for dynamic lists
- Support pull-to-refresh with `RefreshView`

### Navigation
- **AppShell** defines tab-based navigation
- Three tabs: Home, Classes, Students
- Routes registered: `HomePage`, `ClassesPage`, `StudentsPage`

## Next Steps

1. **Install Xcode** from Mac App Store
2. **Configure Xcode** command line tools
3. **Build and run** on iOS Simulator
4. **Test API connectivity** from Home page
5. **Explore Classes and Students** pages

## Related Documentation

- [SchoolApi README](../SchoolApi/README.md) - Backend API documentation
- [SchoolApp README](../SchoolApp/README.md) - Blazor Web frontend
- [Docker README](../DOCKER-README.md) - Containerization guide

## Support

For issues related to:
- **iOS build errors**: Check Xcode setup and ensure all simulators are installed
- **API connectivity**: Verify backend is running and firewall allows connections
- **Data not loading**: Check Console output for detailed error messages
