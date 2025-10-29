# School Management System - Complete Project Documentation

**Date:** October 28, 2025  
**Project:** Full-Stack C# School Management System with Docker Containerization

---

## Table of Contents

1. [Project Overview](#project-overview)
2. [Technology Stack](#technology-stack)
3. [Initial Setup](#initial-setup)
4. [Backend API (SchoolApi)](#backend-api-schoolapi)
5. [Frontend App (SchoolApp)](#frontend-app-schoolapp)
6. [Docker Containerization](#docker-containerization)
7. [Database Access](#database-access)
8. [Troubleshooting](#troubleshooting)
9. [Issues Resolved](#issues-resolved)
10. [Final Project Structure](#final-project-structure)

---

## Project Overview

This project is a complete full-stack school management system built with C# and .NET 8, featuring:

- **Backend**: ASP.NET Core Web API with Entity Framework Core
- **Frontend**: Blazor WebAssembly with Bootstrap UI
- **Database**: MySQL 8.0 with seeded data (5 classes, 20 students)
- **Containerization**: Complete Docker setup for production deployment

### Key Features

- RESTful API with Swagger documentation
- Real-time data fetching from backend
- Responsive web interface
- CRUD operations ready
- Fully containerized with Docker
- Auto-migrations on startup
- CORS-enabled for cross-origin requests

---

## Technology Stack

### Backend
- **.NET 8 SDK** - Runtime and development framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 8** - ORM for database operations
- **Pomelo.EntityFrameworkCore.MySql** - MySQL provider
- **Swashbuckle** - Swagger/OpenAPI documentation

### Frontend
- **Blazor WebAssembly** - Client-side C# framework
- **Bootstrap 5** - UI components and styling
- **HttpClient** - API communication

### Database
- **MySQL 8.0** - Relational database
- **Persistent storage** via Docker volumes

### DevOps
- **Docker** - Containerization platform
- **Docker Compose** - Multi-container orchestration
- **Nginx** - Web server for frontend static files

---

## Initial Setup

### Prerequisites Installation

**1. .NET 8 SDK Installation (macOS)**

```bash
# Uninstall conflicting .NET 9 version
/opt/homebrew/bin/brew uninstall --cask dotnet-sdk

# Install .NET 8 SDK
/opt/homebrew/bin/brew install --cask dotnet-sdk@8

# Verify installation
/opt/homebrew/bin/dotnet --info
```

**Output:**
```
.NET SDK:
 Version:           8.0.415
 Runtime Environment:
 OS Name:     Mac OS X
 RID:         osx-arm64
```

**2. Add .NET to PATH**

```bash
echo 'export PATH="/opt/homebrew/bin:$HOME/.dotnet/tools:$PATH"' >> ~/.zprofile
source ~/.zprofile
```

**3. Install Entity Framework CLI Tools**

```bash
dotnet tool install --global dotnet-ef --version "8.*"
```

---

## Backend API (SchoolApi)

### Project Structure

```
SchoolApi/
├── Controllers/
│   ├── ClassesController.cs     # GET /api/classes
│   └── StudentsController.cs    # GET /api/students
├── Models/
│   ├── Classroom.cs             # Class entity
│   └── Student.cs               # Student entity
├── Data/
│   └── SchoolContext.cs         # EF DbContext with seeded data
├── Migrations/
│   └── InitialCreate.cs         # Database schema migration
├── Program.cs                   # Application entry point
├── appsettings.json             # Configuration
├── Dockerfile                   # Container definition
└── docker-compose.yml           # Multi-container orchestration
```

### Models

**Classroom.cs:**
```csharp
namespace SchoolApi.Models;

public class Classroom
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Student> Students { get; set; } = new();
}
```

**Student.cs:**
```csharp
using System.Text.Json.Serialization;

namespace SchoolApi.Models;

public class Student
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int ClassroomId { get; set; }
    
    [JsonIgnore]  // Prevents circular reference
    public Classroom? Classroom { get; set; }
}
```

### Database Context with Seed Data

**SchoolContext.cs:**
```csharp
using Microsoft.EntityFrameworkCore;
using SchoolApi.Models;

namespace SchoolApi.Data;

public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }

    public DbSet<Classroom> Classrooms => Set<Classroom>();
    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed 5 classrooms
        modelBuilder.Entity<Classroom>().HasData(
            new Classroom { Id = 1, Name = "Class 1" },
            new Classroom { Id = 2, Name = "Class 2" },
            new Classroom { Id = 3, Name = "Class 3" },
            new Classroom { Id = 4, Name = "Class 4" },
            new Classroom { Id = 5, Name = "Class 5" }
        );

        // Seed 20 students (4 per class)
        var students = new List<Student>();
        int studentId = 1;
        for (int classId = 1; classId <= 5; classId++)
        {
            for (int i = 1; i <= 4; i++)
            {
                students.Add(new Student
                {
                    Id = studentId,
                    Name = $"Student {studentId}",
                    ClassroomId = classId
                });
                studentId++;
            }
        }
        modelBuilder.Entity<Student>().HasData(students);
    }
}
```

### API Endpoints

**ClassesController.cs:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly SchoolContext _context;
    
    public ClassesController(SchoolContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Classroom>>> Get()
    {
        var classes = await _context.Classrooms
            .Include(c => c.Students)
            .ToListAsync();
        return Ok(classes);
    }
}
```

**StudentsController.cs:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly SchoolContext _context;
    
    public StudentsController(SchoolContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> Get()
    {
        var students = await _context.Students.AsNoTracking().ToListAsync();
        return Ok(students);
    }
}
```

### Configuration (Program.cs)

```csharp
using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// MySQL connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "server=localhost;port=3306;database=schooldb;user=root;password=example;";

builder.Services.AddDbContext<SchoolContext>(options =>
{
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));
});

var app = builder.Build();

// Auto-run migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchoolContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowBlazorApp");
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
```

### Running the Backend

**Build and Run:**
```bash
cd SchoolApi
dotnet restore
dotnet build
dotnet run --project SchoolApi.csproj
```

**Access Points:**
- API: http://localhost:5178
- Swagger: http://localhost:5178/swagger
- Health: http://localhost:5178/health

---

## Frontend App (SchoolApp)

### Project Structure

```
SchoolApp/
├── Pages/
│   ├── Home.razor          # Dashboard
│   ├── Classes.razor       # Classes with students
│   └── Students.razor      # Student list
├── Models/
│   ├── Classroom.cs
│   └── Student.cs
├── Services/
│   └── SchoolApiService.cs # API integration
├── Layout/
│   ├── MainLayout.razor
│   └── NavMenu.razor       # Navigation
├── Program.cs              # App configuration
└── wwwroot/
    ├── appsettings.json
    └── index.html
```

### API Service

**SchoolApiService.cs:**
```csharp
using System.Net.Http.Json;
using SchoolApp.Models;

namespace SchoolApp.Services;

public class SchoolApiService
{
    private readonly HttpClient _httpClient;

    public SchoolApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Classroom>?> GetClassesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Classroom>>("api/classes");
    }

    public async Task<List<Student>?> GetStudentsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Student>>("api/students");
    }
}
```

### Pages

**Home.razor:**
```razor
@page "/"
<PageTitle>School Management</PageTitle>

<h1>Welcome to School Management System</h1>
<p class="lead">Manage your school's classes and students efficiently.</p>

<div class="row mt-4">
    <div class="col-md-6">
        <div class="card text-white bg-primary mb-3">
            <div class="card-header"><h4>Classes</h4></div>
            <div class="card-body">
                <a href="/classes" class="btn btn-light">Go to Classes</a>
            </div>
        </div>
    </div>
    <div class="col-md-6">
        <div class="card text-white bg-success mb-3">
            <div class="card-header"><h4>Students</h4></div>
            <div class="card-body">
                <a href="/students" class="btn btn-light">Go to Students</a>
            </div>
        </div>
    </div>
</div>
```

**Classes.razor:**
```razor
@page "/classes"
@inject SchoolApiService ApiService

<h1>School Classes</h1>

@if (classes == null)
{
    <p><em>Loading...</em></p>
}
else
{
    @foreach (var classroom in classes)
    {
        <div class="card mb-4">
            <div class="card-header bg-primary text-white">
                <h5>@classroom.Name</h5>
            </div>
            <div class="card-body">
                <ul class="list-group">
                    @foreach (var student in classroom.Students)
                    {
                        <li class="list-group-item">@student.Name</li>
                    }
                </ul>
            </div>
        </div>
    }
}

@code {
    private List<Classroom>? classes;

    protected override async Task OnInitializedAsync()
    {
        classes = await ApiService.GetClassesAsync();
    }
}
```

### Running the Frontend

```bash
cd SchoolApp
dotnet build
dotnet run
```

**Access:** http://localhost:5244

---

## Docker Containerization

### Architecture

```
┌────────────────────────────────────────────┐
│           Docker Host                       │
│  ┌──────────────────────────────────────┐  │
│  │      school-network (bridge)         │  │
│  │                                       │  │
│  │  ┌─────────┐  ┌──────────┐  ┌─────┐ │  │
│  │  │ MySQL   │  │SchoolApi │  │ App │ │  │
│  │  │ :3306   │◄─│  :5178   │◄─│ :80 │ │  │
│  │  └─────────┘  └──────────┘  └─────┘ │  │
│  └──────────────────────────────────────┘  │
│       │             │              │        │
│    Port 3306    Port 5178      Port 8080   │
└────────────────────────────────────────────┘
```

### Docker Compose Configuration

**docker-compose.yml:**
```yaml
version: "3.9"

services:
  mysql:
    image: mysql:8.0
    container_name: school-mysql
    environment:
      MYSQL_ROOT_PASSWORD: example
      MYSQL_DATABASE: schooldb
    ports:
      - "3306:3306"
    volumes:
      - mysql_data:/var/lib/mysql
    healthcheck:
      test: ["CMD", "mysqladmin", "ping", "-h", "localhost"]
      interval: 10s
      timeout: 5s
      retries: 5
    networks:
      - school-network

  schoolapi:
    build:
      context: .
      dockerfile: Dockerfile
    container_name: school-api
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=server=mysql;port=3306;database=schooldb;user=root;password=example;
    ports:
      - "5178:5178"
    depends_on:
      mysql:
        condition: service_healthy
    networks:
      - school-network

  schoolapp:
    build:
      context: ../SchoolApp
      dockerfile: Dockerfile
    container_name: school-app
    ports:
      - "8080:80"
    depends_on:
      - schoolapi
    networks:
      - school-network

volumes:
  mysql_data:

networks:
  school-network:
    driver: bridge
```

### Dockerfiles

**Backend Dockerfile (SchoolApi/Dockerfile):**
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY SchoolApi.csproj .
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5178
EXPOSE 5178
ENTRYPOINT ["dotnet", "SchoolApi.dll"]
```

**Frontend Dockerfile (SchoolApp/Dockerfile):**
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY SchoolApp.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

### Running with Docker

**One-command startup:**
```bash
cd SchoolApi
docker-compose up --build
```

**Or use the script:**
```bash
cd "/Users/dr.attia.cloud.dragon/Downloads/C#"
./start-docker.sh
```

**Access Points:**
- Frontend: http://localhost:8080
- Backend API: http://localhost:5178/swagger
- MySQL: localhost:3306

---

## Database Access

### Via Docker

```bash
# Access MySQL CLI
docker-compose exec mysql mysql -uroot -pexample schooldb

# View tables
docker-compose exec mysql mysql -uroot -pexample -e "SHOW TABLES" schooldb

# Query students
docker-compose exec mysql mysql -uroot -pexample -e "SELECT * FROM Students" schooldb
```

### Connection Details

- **Host:** localhost
- **Port:** 3306
- **Database:** schooldb
- **Username:** root
- **Password:** example

### GUI Tools

**Recommended:**
- MySQL Workbench: `brew install --cask mysqlworkbench`
- TablePlus: `brew install --cask tableplus`
- DBeaver: `brew install --cask dbeaver-community`

---

## Troubleshooting

### Issues Resolved During Development

#### 1. .NET SDK Not Found
**Error:** `dotnet: command not found`

**Solution:**
```bash
brew install --cask dotnet-sdk@8
echo 'export PATH="/opt/homebrew/bin:$HOME/.dotnet/tools:$PATH"' >> ~/.zprofile
```

#### 2. JSON Circular Reference
**Error:** `A possible object cycle was detected`

**Solution:** Added `[JsonIgnore]` to `Student.Classroom` navigation property

```csharp
[JsonIgnore]
public Classroom? Classroom { get; set; }
```

#### 3. EF Migrations at Design Time
**Error:** `Unable to connect to MySQL hosts` during migration

**Solution:** Changed from `ServerVersion.AutoDetect()` to explicit version:
```csharp
options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));
```

#### 4. CORS Errors
**Error:** Frontend couldn't call backend API

**Solution:** Added CORS policy in Program.cs:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
```

---

## Final Project Structure

```
C#/
├── SchoolApi/                     # Backend API
│   ├── Controllers/
│   │   ├── ClassesController.cs
│   │   └── StudentsController.cs
│   ├── Data/
│   │   └── SchoolContext.cs
│   ├── Models/
│   │   ├── Classroom.cs
│   │   └── Student.cs
│   ├── Migrations/
│   │   └── InitialCreate.cs
│   ├── Program.cs
│   ├── Dockerfile
│   ├── docker-compose.yml
│   ├── .dockerignore
│   └── README.md
│
├── SchoolApp/                     # Frontend Blazor App
│   ├── Pages/
│   │   ├── Home.razor
│   │   ├── Classes.razor
│   │   └── Students.razor
│   ├── Models/
│   │   ├── Classroom.cs
│   │   └── Student.cs
│   ├── Services/
│   │   └── SchoolApiService.cs
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   └── NavMenu.razor
│   ├── wwwroot/
│   │   ├── appsettings.json
│   │   └── appsettings.Production.json
│   ├── Program.cs
│   ├── Dockerfile
│   ├── nginx.conf
│   └── README.md
│
├── start-docker.sh                # One-command launcher
├── DOCKER-README.md              # Docker documentation
└── CONTAINERIZATION-SUMMARY.md    # Quick reference
```

---

## Summary

### What Was Built

1. **Full-Stack Application**
   - ASP.NET Core Web API backend
   - Blazor WebAssembly frontend
   - MySQL database with seeded data

2. **Complete Docker Setup**
   - Multi-container orchestration
   - Auto-migrations
   - Production-ready configuration

3. **Features**
   - RESTful API with Swagger
   - Responsive web UI
   - CRUD-ready architecture
   - Persistent data storage

### Key Achievements

✅ .NET 8 development environment configured  
✅ EF Core migrations with seed data  
✅ Backend API with 3 endpoints  
✅ Blazor WebAssembly frontend  
✅ CORS configuration  
✅ Complete Docker containerization  
✅ Auto-deployment scripts  
✅ Comprehensive documentation  

### Technologies Mastered

- ASP.NET Core Web API
- Entity Framework Core
- Blazor WebAssembly
- Docker & Docker Compose
- MySQL database management
- Multi-stage Docker builds
- Nginx configuration
- CORS policies

---

**Project Completion Date:** October 28, 2025  
**Total Development Time:** Full session  
**Final Status:** ✅ Production Ready


