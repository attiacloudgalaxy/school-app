# iOS App Setup Guide

Quick guide to get the School Management iOS app running on your Mac.

## Step 1: Install Xcode

### Option A: Mac App Store (Recommended)
1. Open **Mac App Store**
2. Search for **"Xcode"**
3. Click **Get** or **Install**
4. Wait for installation (10-15 GB download, may take 30-60 minutes)

### Option B: Direct Download
1. Visit [developer.apple.com/download](https://developer.apple.com/download)
2. Sign in with Apple ID
3. Download **Xcode** (latest version)
4. Install the `.xip` file

## Step 2: Configure Xcode

After installation, run these commands in Terminal:

```bash
# Set command line tools path
sudo xcode-select -s /Applications/Xcode.app/Contents/Developer

# Verify path
xcode-select -p
# Should output: /Applications/Xcode.app/Contents/Developer

# Accept license agreement
sudo xcodebuild -license accept

# Install iOS Simulator (if needed)
xcodebuild -downloadPlatform iOS
```

## Step 3: Verify Installation

```bash
# Check Xcode version
xcodebuild -version
# Should show: Xcode 15.x or later

# List available simulators
xcrun simctl list devices | grep iPhone

# Should show simulators like:
#   iPhone 15 (...)
#   iPhone 15 Pro (...)
```

## Step 4: Build the iOS App

```bash
# Navigate to iOS project
cd "/Users/dr.attia.cloud.dragon/Downloads/C#/SchoolApp.iOS"

# Build the app
dotnet build -f net8.0-ios
```

If build succeeds, you'll see:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

## Step 5: Start the Backend API

In a **new terminal window**:

```bash
cd "/Users/dr.attia.cloud.dragon/Downloads/C#/SchoolApi"
dotnet run
```

Wait for:
```
Now listening on: http://localhost:5178
```

## Step 6: Run on iOS Simulator

In the **original terminal** (SchoolApp.iOS directory):

```bash
dotnet build -t:Run -f net8.0-ios
```

This will:
1. Build the app
2. Launch iOS Simulator
3. Install and run the app

## Step 7: Test the App

Once the app launches:

1. **Home Page** should show:
   - ✓ Connected to API (green)
   - Total Classes: 5
   - Total Students: 20

2. Tap **View Classes** to see all 5 classes with students

3. Tap **View Students** to see all 20 students

4. Use **pull-to-refresh** gesture on any page to reload data

## Troubleshooting

### "Could not find a valid Xcode app bundle"
**Solution**: Xcode is not installed or path is incorrect
```bash
# Check if Xcode is installed
ls /Applications/Xcode.app

# If exists, set the path
sudo xcode-select -s /Applications/Xcode.app/Contents/Developer
```

### "No provisioning profile found"
**Solution**: This is for physical devices only. Use simulator first.
```bash
# Just run on simulator
dotnet build -t:Run -f net8.0-ios
```

### "API Connection Failed"
**Solution**: Backend is not running
```bash
# In a new terminal, start the API
cd "../SchoolApi"
dotnet run
```

### Simulator doesn't launch
**Solution**: Open Xcode once to initialize simulators
```bash
# Open Xcode
open /Applications/Xcode.app

# Or use xcrun
open -a Simulator
```

### Build takes very long (first time)
**This is normal!** First build compiles MAUI, downloads iOS SDKs, etc.
- First build: 5-10 minutes
- Subsequent builds: 30-60 seconds

## Quick Reference

### Start Everything
```bash
# Terminal 1: Start API
cd "/Users/dr.attia.cloud.dragon/Downloads/C#/SchoolApi"
dotnet run

# Terminal 2: Run iOS App
cd "/Users/dr.attia.cloud.dragon/Downloads/C#/SchoolApp.iOS"
dotnet build -t:Run -f net8.0-ios
```

### Stop Everything
```bash
# Press Ctrl+C in both terminals
# Or:
killall dotnet
xcrun simctl shutdown all  # Stop all simulators
```

## Running on Physical iPhone/iPad

1. Connect device via USB
2. Trust your Mac on the device
3. Get your Mac's IP address:
   ```bash
   ipconfig getifaddr en0  # WiFi
   ```

4. Update API URL in `Services/SchoolApiService.cs`:
   ```csharp
   private const string BaseUrl = "http://YOUR_IP:5178/";  // e.g., 192.168.1.100
   ```

5. Run with device target:
   ```bash
   dotnet build -t:Run -f net8.0-ios -p:RuntimeIdentifier=ios-arm64
   ```

## Next Steps

- Read full documentation: [README.md](README.md)
- Check API documentation: [../SchoolApi/README.md](../SchoolApi/README.md)
- Docker deployment: [../DOCKER-README.md](../DOCKER-README.md)

## Getting Help

**Build errors**: Check `dotnet build` output for specific error messages
**Runtime errors**: Look at Xcode Console (Window → Devices and Simulators)
**API errors**: Test with `curl http://localhost:5178/health`
