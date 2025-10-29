# School Management System

A complete full-stack school management application built with C# and .NET 8, featuring a REST API backend, Blazor WebAssembly frontend, iOS mobile app, and full Docker containerization.

## 🎯 Overview

This project demonstrates a modern, production-ready school management system with:
- **5 Classes** (Class 1 through Class 5)
- **20 Students** (4 students per class)
- **Multi-platform Support**: Web, iOS, and containerized deployment

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────┐
│                     Client Layer                         │
├──────────────────┬──────────────────┬───────────────────┤
│  Blazor Web App  │  iOS MAUI App    │  Any HTTP Client  │
│  (Port 5244)     │  (Native Mobile) │  (curl, Postman)  │
└────────┬─────────┴────────┬─────────┴──────────┬────────┘
         │                  │                     │
         └──────────────────┼─────────────────────┘
                            │
                    ┌───────▼────────┐
                    │   SchoolApi    │
                    │  (Port 5178)   │
                    │  ASP.NET Core  │
                    └───────┬────────┘
                            │
                    ┌───────▼────────┐
                    │  MySQL 8.0     │
                    │  (Port 3306)   │
                    └────────────────┘
```

## 📦 Project Structure

```
C#/
├── SchoolApi/              # Backend REST API
│   ├── Controllers/        # API endpoints (Classes, Students)
│   ├── Models/            # Data models (Classroom, Student)
│   ├── Data/              # DbContext and migrations
│   ├── Migrations/        # EF Core migrations with seed data
│   └── Dockerfile         # Backend container configuration
│
├── SchoolApp/             # Blazor WebAssembly Frontend
│   ├── Pages/             # Razor components (Home, Classes, Students)
│   ├── Services/          # API integration services
│   ├── Models/            # Client-side models
│   └── Dockerfile         # Frontend container (Nginx)
│
├── SchoolApp.iOS/         # iOS MAUI Mobile App
│   ├── Pages/             # XAML pages with native iOS UI
│   ├── Models/            # Mobile data models
│   ├── Services/          # HTTP client for API
│   └── Platforms/iOS/     # iOS-specific configuration
│
├── docker-compose.yml     # Container orchestration
├── start-docker.sh        # One-command startup script
└── Documentation/         # Comprehensive guides
```

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for containerized deployment)
- [Xcode](https://developer.apple.com/xcode/) (for iOS development, macOS only)

### Option 1: Docker (Recommended)

Run everything with one command:

```bash
./start-docker.sh
```

Or manually:

```bash
docker-compose up -d
```

Access:
- **API**: http://localhost:5178
- **Web App**: http://localhost:8080
- **Swagger UI**: http://localhost:5178/swagger

### Option 2: Local Development

#### 1. Start MySQL Database
```bash
docker run -d \
  --name school-mysql \
  -e MYSQL_ROOT_PASSWORD=schoolpass \
  -e MYSQL_DATABASE=schooldb \
  -p 3306:3306 \
  mysql:8.0
```

#### 2. Run Backend API
```bash
cd SchoolApi
dotnet run
```

#### 3. Run Web Frontend
```bash
cd SchoolApp
dotnet run
```

#### 4. Run iOS App (macOS only)
```bash
cd SchoolApp.iOS
dotnet build -t:Run -f net8.0-ios
```

## 📱 Features

### Backend API (SchoolApi)
- ✅ RESTful API with Swagger/OpenAPI documentation
- ✅ Entity Framework Core with MySQL
- ✅ Automatic migrations on startup
- ✅ CORS enabled for cross-origin requests
- ✅ Health check endpoint
- ✅ Seed data for 5 classes and 20 students

**Endpoints:**
- `GET /health` - Health check
- `GET /api/classes` - Get all classes with students
- `GET /api/students` - Get all students

### Web Frontend (SchoolApp)
- ✅ Modern Blazor WebAssembly application
- ✅ Bootstrap 5 UI framework
- ✅ Responsive design
- ✅ Real-time data from API
- ✅ Three main pages: Home, Classes, Students

### iOS Mobile App (SchoolApp.iOS)
- ✅ Native iOS app built with .NET MAUI
- ✅ Tab-based navigation
- ✅ Pull-to-refresh functionality
- ✅ Search and filter students
- ✅ Connection status indicator
- ✅ Supports iOS Simulator and physical devices

### Docker Support
- ✅ Multi-container setup with docker-compose
- ✅ MySQL with persistent volume
- ✅ Health checks and dependencies
- ✅ Network isolation
- ✅ One-command deployment

## 🗄️ Database Schema

### Classrooms Table
| Column | Type    | Description        |
|--------|---------|-------------------|
| Id     | int     | Primary Key       |
| Name   | string  | Class name        |

### Students Table
| Column      | Type    | Description              |
|-------------|---------|--------------------------|
| Id          | int     | Primary Key              |
| Name        | string  | Student name             |
| ClassroomId | int     | Foreign Key to Classroom |

**Sample Data:**
- Class 1: Student 1, Student 2, Student 3, Student 4
- Class 2: Student 5, Student 6, Student 7, Student 8
- ... (continues for all 5 classes and 20 students)

## 🛠️ Technology Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core 8.0
- **Database**: MySQL 8.0 with Pomelo provider
- **API Documentation**: Swagger/OpenAPI

### Frontend (Web)
- **Framework**: Blazor WebAssembly
- **UI**: Bootstrap 5
- **HTTP Client**: System.Net.Http.Json

### Frontend (Mobile)
- **Framework**: .NET MAUI 8.0
- **Platforms**: iOS (18.0+)
- **UI**: XAML with native controls

### DevOps
- **Containerization**: Docker
- **Orchestration**: Docker Compose
- **Web Server**: Nginx (for Blazor static files)

## 📚 Documentation

Detailed documentation is available for each component:

- [📖 Project Documentation](./PROJECT-DOCUMENTATION.md) - Complete project overview
- [🐳 Docker Guide](./DOCKER-README.md) - Containerization instructions
- [📋 Containerization Summary](./CONTAINERIZATION-SUMMARY.md) - Quick reference
- [🔧 SchoolApi README](./SchoolApi/README.md) - Backend API documentation
- [🌐 SchoolApp README](./SchoolApp/README.md) - Web frontend documentation
- [📱 iOS App README](./SchoolApp.iOS/README.md) - Mobile app setup guide

## 🔧 Configuration

### Backend API (`SchoolApi/appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=schooldb;user=root;password=schoolpass"
  }
}
```

### Web Frontend (`SchoolApp/wwwroot/appsettings.json`)
```json
{
  "ApiBaseUrl": "http://localhost:5178/"
}
```

### iOS App (`SchoolApp.iOS/Services/SchoolApiService.cs`)
```csharp
private const string BaseUrl = "http://localhost:5178/"; // For simulator
// private const string BaseUrl = "http://YOUR_IP:5178/"; // For device
```

## 🧪 Testing

### Test Backend API
```bash
# Health check
curl http://localhost:5178/health

# Get all classes with students
curl http://localhost:5178/api/classes

# Get all students
curl http://localhost:5178/api/students
```

### Access Web Interface
1. Open browser to http://localhost:5244 (local) or http://localhost:8080 (Docker)
2. Navigate through Home, Classes, and Students pages
3. Verify data loads from API

### Test iOS App
1. Ensure backend is running
2. Launch app on iOS Simulator
3. Check connection status on Home page
4. Navigate to Classes and Students pages
5. Test pull-to-refresh and search functionality

## 🐛 Troubleshooting

### Docker Issues
```bash
# View container logs
docker-compose logs

# Restart services
docker-compose restart

# Clean rebuild
docker-compose down -v
docker-compose up --build
```

### Database Connection Issues
```bash
# Check if MySQL is running
docker ps | grep mysql

# Connect to MySQL
docker exec -it school-mysql mysql -uroot -pschoolpass schooldb

# Verify data
SELECT * FROM Classrooms;
SELECT * FROM Students;
```

### iOS Build Issues
- Ensure Xcode is installed: `xcode-select -p`
- Install iOS simulator: Open Xcode → Settings → Platforms → iOS
- Verify MAUI workload: `dotnet workload list`

## 🤝 Contributing

This is a demonstration project showcasing:
- Full-stack C# development
- Multi-platform application design
- Docker containerization
- RESTful API design
- Entity Framework Core with MySQL
- Blazor WebAssembly
- .NET MAUI mobile development

## 📄 License

This project is provided as-is for educational and demonstration purposes.

## 🙏 Acknowledgments

Built with:
- .NET 8
- ASP.NET Core
- Entity Framework Core
- Blazor WebAssembly
- .NET MAUI
- MySQL
- Docker
- Nginx

---

**Made with ❤️ using C# and .NET**
# school-app
