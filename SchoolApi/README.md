# SchoolApi - ASP.NET Core + MySQL School Management API

A simple REST API for managing an imaginary school with 5 classes and 20 students (dummy data).

## Tech Stack

- **ASP.NET Core 8.0** (Web API)
- **Entity Framework Core 8.0** (ORM)
- **MySQL 8.0** (Database)
- **Pomelo.EntityFrameworkCore.MySql** (MySQL provider)
- **Swagger/OpenAPI** (API documentation)

## Project Structure

```
SchoolApi/
├── Controllers/
│   ├── ClassesController.cs    # GET /api/classes
│   └── StudentsController.cs   # GET /api/students
├── Models/
│   ├── Classroom.cs            # Classroom entity
│   └── Student.cs              # Student entity with [JsonIgnore] on Classroom nav property
├── Data/
│   └── SchoolContext.cs        # EF Core DbContext with seeded data (5 classes, 20 students)
├── Migrations/
│   └── InitialCreate.cs        # EF migration for schema + seed data
├── Program.cs                  # App entry point, DI, middleware
├── appsettings.json
├── appsettings.Development.json
├── docker-compose.yml          # MySQL 8 container (optional)
└── README.md
```

## Prerequisites

- **.NET 8 SDK** (installed via Homebrew on macOS)
- **MySQL 8** (via Docker or Homebrew)
- (Optional) **Docker Desktop** for containerized MySQL

## Setup Instructions

### 1. Install .NET 8 SDK (macOS)

```bash
/opt/homebrew/bin/brew install --cask dotnet-sdk@8
```

Verify:
```bash
/opt/homebrew/bin/dotnet --version
# Should show 8.0.x
```

### 2. Add dotnet to PATH

```bash
echo 'export PATH="/opt/homebrew/bin:$HOME/.dotnet/tools:$PATH"' >> ~/.zprofile
source ~/.zprofile
```

### 3. Start MySQL

**Option A: Docker (Recommended)**
```bash
cd SchoolApi
docker compose up -d
```

This starts MySQL 8 on `localhost:3306` with:
- Root password: `example`
- Database: `schooldb`

**Option B: Homebrew MySQL**
```bash
brew install mysql@8.4
brew services start mysql@8.4
# Create database manually or update connection string in appsettings.json
```

### 4. Apply EF Core Migrations

```bash
cd SchoolApi
dotnet ef database update
```

This creates the database schema and seeds:
- **5 classrooms**: Class 1, Class 2, Class 3, Class 4, Class 5
- **20 students**: Student 1-20 (4 per class)

### 5. Run the API

```bash
dotnet run --project SchoolApi.csproj
```

The API starts on **http://localhost:5178**

## API Endpoints

| Method | Endpoint          | Description                              |
|--------|-------------------|------------------------------------------|
| GET    | `/health`         | Health check (returns `{"status":"ok"}`) |
| GET    | `/api/classes`    | Get all classes with students            |
| GET    | `/api/students`   | Get all students                         |

### Example Requests

**Health Check:**
```bash
curl http://localhost:5178/health
# {"status":"ok"}
```

**Get All Classes (with students):**
```bash
curl http://localhost:5178/api/classes | jq
```

Response:
```json
[
  {
    "id": 1,
    "name": "Class 1",
    "students": [
      {"id": 1, "name": "Student 1", "classroomId": 1},
      {"id": 2, "name": "Student 2", "classroomId": 1},
      {"id": 3, "name": "Student 3", "classroomId": 1},
      {"id": 4, "name": "Student 4", "classroomId": 1}
    ]
  },
  ...
]
```

**Get All Students:**
```bash
curl http://localhost:5178/api/students | jq
```

Response:
```json
[
  {"id": 1, "name": "Student 1", "classroomId": 1},
  {"id": 2, "name": "Student 2", "classroomId": 1},
  ...
  {"id": 20, "name": "Student 20", "classroomId": 5}
]
```

## Swagger UI

Visit **http://localhost:5178/swagger** in your browser for interactive API documentation.

## Database Connection

Default connection string (from `appsettings.json`):
```
server=localhost;port=3306;database=schooldb;user=root;password=example;
```

To customize, edit `appsettings.Development.json` or set the `ConnectionStrings__DefaultConnection` environment variable.

## Development Commands

**Restore packages:**
```bash
dotnet restore
```

**Build:**
```bash
dotnet build
```

**Run in watch mode (auto-reload):**
```bash
dotnet watch run
```

**Add a new migration:**
```bash
dotnet ef migrations add MigrationName
```

**Update database:**
```bash
dotnet ef database update
```

**Rollback migration:**
```bash
dotnet ef database update PreviousMigrationName
```

## Troubleshooting

### Error: "Unable to connect to any of the specified MySQL hosts"

- Ensure MySQL is running (`docker ps` or `brew services list`)
- Verify connection string in `appsettings.Development.json`
- Check that port 3306 is not in use by another process

### Error: "A possible object cycle was detected"

- Fixed by adding `[JsonIgnore]` to `Student.Classroom` navigation property
- This prevents circular references during JSON serialization

### EF Migrations fail at design time

- Fixed by replacing `ServerVersion.AutoDetect()` with explicit `new MySqlServerVersion(new Version(8, 0, 36))`
- This avoids DB connection attempts during `dotnet ef` commands

## Project Notes

- **Data seeding**: Done via `OnModelCreating` in `SchoolContext.cs`
- **JSON serialization**: Uses `System.Text.Json` (ASP.NET Core default)
- **Navigation properties**: `Classroom.Students` is included in GET requests; `Student.Classroom` is ignored to prevent cycles
- **AsNoTracking**: Used in `StudentsController` for read-only queries (performance optimization)

## License

This is a demo project for educational purposes.
