# SchoolApp - Blazor WebAssembly Frontend

A modern web frontend for the School Management System, built with Blazor WebAssembly and Bootstrap.

## Features

- **Home Page**: Dashboard with quick navigation and stats
- **Classes View**: Display all 5 classes with their enrolled students
- **Students View**: Table view of all 20 students
- **Responsive Design**: Bootstrap-based UI that works on all devices
- **Real-time API Integration**: Connects to SchoolApi backend

## Tech Stack

- **Blazor WebAssembly** (Client-side C#)
- **ASP.NET Core 8.0**
- **Bootstrap 5** (UI framework)
- **HttpClient** (API calls)

## Prerequisites

- **.NET 8 SDK** (already installed)
- **SchoolApi backend** running on http://localhost:5178

## Project Structure

```
SchoolApp/
├── Pages/
│   ├── Home.razor           # Dashboard page
│   ├── Classes.razor        # Classes list with students
│   └── Students.razor       # Students table
├── Models/
│   ├── Classroom.cs         # Classroom model
│   └── Student.cs           # Student model
├── Services/
│   └── SchoolApiService.cs  # HTTP service for API calls
├── Layout/
│   ├── MainLayout.razor     # Main app layout
│   └── NavMenu.razor        # Navigation sidebar
├── Program.cs               # App entry point with DI
└── wwwroot/                 # Static files (CSS, images)
```

## How to Run

### 1. Ensure Backend is Running

The backend API must be running first:
```bash
cd ../SchoolApi
dotnet run --project SchoolApi.csproj
```

Backend should be listening on: **http://localhost:5178**

### 2. Start the Frontend

```bash
cd SchoolApp
dotnet run
```

The app will start on: **http://localhost:5244** (or similar port)

### 3. Open in Browser

Visit: **http://localhost:5244**

## Pages & Features

### 🏠 Home (/)
- Welcome dashboard
- Quick navigation cards
- School statistics summary

### 📚 Classes (/classes)
- Card view of all 5 classes
- Each card shows:
  - Class name
  - Student count
  - List of enrolled students with IDs

### 👥 Students (/students)
- Table view of all students
- Columns: ID, Name, Classroom ID
- Total student count

## API Configuration

The frontend is configured to connect to the backend at:
```
http://localhost:5178/
```

To change the API URL, edit `Program.cs`:
```csharp
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("http://your-api-url/") 
});
```

## Development

**Run in watch mode (auto-reload):**
```bash
dotnet watch run
```

**Build for production:**
```bash
dotnet publish -c Release
```

Output will be in: `bin/Release/net8.0/publish/wwwroot/`

## Troubleshooting

### "Failed to fetch" or CORS errors

- Ensure the backend has CORS enabled (already configured)
- Verify backend is running: `curl http://localhost:5178/health`
- Check browser console for detailed errors

### Blank data / "Loading..."

- Open browser developer tools (F12)
- Check Console tab for API errors
- Verify API endpoints return data: `curl http://localhost:5178/api/classes`

### Port already in use

If port 5244 is taken, dotnet will auto-assign another port.
Check the console output for the actual URL:
```
Now listening on: http://localhost:XXXX
```

## Architecture

### Service Layer
`SchoolApiService` handles all HTTP communication:
- `GetClassesAsync()` → GET /api/classes
- `GetStudentsAsync()` → GET /api/students

### Dependency Injection
- `HttpClient` configured with backend base URL
- `SchoolApiService` registered as scoped service
- Injected into Razor components with `@inject`

### Component Lifecycle
Pages use `OnInitializedAsync()` to:
1. Call API service
2. Load data into local state
3. Trigger UI re-render

## Future Enhancements

- Add student/class creation forms
- Edit and delete functionality
- Search and filtering
- Pagination for large datasets
- Loading spinners and error messages
- Authentication/authorization

## License

Demo project for educational purposes.
