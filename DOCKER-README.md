# School Management System - Full Stack Containerized

A complete containerized school management system with MySQL database, ASP.NET Core API backend, and Blazor WebAssembly frontend.

## 🐳 Docker Architecture

```
┌─────────────────────────────────────────────────────────┐
│                     Docker Compose                       │
├──────────────┬──────────────────┬───────────────────────┤
│   MySQL 8    │   SchoolApi      │    SchoolApp          │
│   Database   │   (Backend API)  │    (Frontend)         │
│              │                  │                       │
│  Port: 3306  │   Port: 5178     │   Port: 8080          │
│              │                  │                       │
│  - schooldb  │  - REST API      │   - Blazor WASM       │
│  - 5 classes │  - EF Core       │   - Nginx             │
│  - 20 students│  - Auto migrations│  - Static files     │
└──────────────┴──────────────────┴───────────────────────┘
         │              │                    │
         └──────────────┴────────────────────┘
                  school-network (bridge)
```

## 📋 Prerequisites

- **Docker Desktop** (includes Docker Compose)
- **Git** (optional, to clone the project)

That's it! No need to install .NET, MySQL, or any other dependencies locally.

## 🚀 Quick Start

### 1. Navigate to the project directory

```bash
cd "/Users/dr.attia.cloud.dragon/Downloads/C#/SchoolApi"
```

### 2. Build and start all containers

```bash
docker-compose up --build
```

This command will:
- Build the MySQL container
- Build the SchoolApi backend container
- Build the SchoolApp frontend container
- Apply database migrations automatically
- Seed 5 classes and 20 students
- Start all services

### 3. Access the application

**Frontend (Blazor WebAssembly):**
```
http://localhost:8080
```

**Backend API (Swagger UI):**
```
http://localhost:5178/swagger
```

**Health Check:**
```
http://localhost:5178/health
```

**MySQL Database:**
```
Host: localhost
Port: 3306
Database: schooldb
User: root
Password: example
```

## 🛠️ Docker Commands

### Start services (detached mode)
```bash
docker-compose up -d
```

### Stop services
```bash
docker-compose down
```

### Stop and remove volumes (clean slate)
```bash
docker-compose down -v
```

### View logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f schoolapi
docker-compose logs -f schoolapp
docker-compose logs -f mysql
```

### Rebuild containers
```bash
docker-compose up --build
```

### Check running containers
```bash
docker-compose ps
```

### Execute commands in container
```bash
# Access MySQL
docker-compose exec mysql mysql -uroot -pexample schooldb

# Access API container shell
docker-compose exec schoolapi /bin/bash

# Access frontend container shell
docker-compose exec schoolapp /bin/sh
```

## 📁 Container Details

### MySQL Container
- **Image**: `mysql:8.0`
- **Port**: `3306`
- **Database**: `schooldb`
- **Credentials**: root/example
- **Volume**: Persistent data in `mysql_data` volume
- **Health Check**: Automatic health monitoring

### SchoolApi Container
- **Base Image**: `mcr.microsoft.com/dotnet/aspnet:8.0`
- **Port**: `5178`
- **Features**:
  - Multi-stage Docker build
  - Automatic EF Core migrations on startup
  - CORS enabled for frontend
  - Swagger UI in development mode
  - Connects to MySQL container via Docker network

### SchoolApp Container
- **Base Image**: `nginx:alpine`
- **Port**: `8080` (mapped from container port 80)
- **Features**:
  - Blazor WebAssembly static files
  - Nginx for serving and routing
  - Gzip compression enabled
  - Configured to call backend API via Docker network

## 🔧 Configuration

### Environment Variables

**SchoolApi (docker-compose.yml):**
```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Development
  - ConnectionStrings__DefaultConnection=server=mysql;port=3306;database=schooldb;user=root;password=example;
```

**SchoolApp API URL:**
- Local development: `http://localhost:5178/` (in `appsettings.json`)
- Docker deployment: `http://schoolapi:5178/` (in `appsettings.Production.json`)

### Changing MySQL Password

Edit `docker-compose.yml`:
```yaml
mysql:
  environment:
    MYSQL_ROOT_PASSWORD: your-new-password

schoolapi:
  environment:
    - ConnectionStrings__DefaultConnection=server=mysql;port=3306;database=schooldb;user=root;password=your-new-password;
```

## 🧪 Testing the Containerized App

### 1. Check all services are running
```bash
docker-compose ps
```

Expected output:
```
NAME            IMAGE           STATUS    PORTS
school-api      schoolapi       Up        0.0.0.0:5178->5178/tcp
school-app      schoolapp       Up        0.0.0.0:8080->80/tcp
school-mysql    mysql:8.0       Up        0.0.0.0:3306->3306/tcp
```

### 2. Test backend API
```bash
curl http://localhost:5178/health
# {"status":"ok"}

curl http://localhost:5178/api/classes | jq
# Returns 5 classes with students
```

### 3. Test frontend
Open browser: http://localhost:8080
- Navigate to "Classes" page
- Navigate to "Students" page
- Verify data loads from backend

### 4. Test database
```bash
docker-compose exec mysql mysql -uroot -pexample -e "USE schooldb; SELECT COUNT(*) FROM Students;"
```

Expected: 20 students

## 🐞 Troubleshooting

### Container fails to start

**View logs:**
```bash
docker-compose logs servicename
```

### Database connection errors

**Check if MySQL is healthy:**
```bash
docker-compose ps mysql
```

**Verify connection string** in `docker-compose.yml` matches MySQL credentials

### Frontend can't reach backend

**Check network:**
```bash
docker network inspect schoolapi_school-network
```

**Verify API URL** in `SchoolApp/wwwroot/appsettings.Production.json`

### Port already in use

**Change ports in docker-compose.yml:**
```yaml
ports:
  - "8081:80"  # Instead of 8080:80
```

### Clear everything and restart

```bash
docker-compose down -v
docker system prune -a
docker-compose up --build
```

## 📊 Resource Usage

Typical resource consumption:
- **MySQL**: ~400MB RAM
- **SchoolApi**: ~150MB RAM
- **SchoolApp**: ~20MB RAM (nginx)
- **Total**: ~600MB RAM, ~2GB disk (including images)

## 🔄 Development Workflow

### Making changes to the backend

1. Edit code in `SchoolApi/`
2. Rebuild and restart:
   ```bash
   docker-compose up --build schoolapi
   ```

### Making changes to the frontend

1. Edit code in `SchoolApp/`
2. Rebuild and restart:
   ```bash
   docker-compose up --build schoolapp
   ```

### Database schema changes

1. Add migration locally:
   ```bash
   cd SchoolApi
   dotnet ef migrations add YourMigrationName
   ```
2. Rebuild API container:
   ```bash
   docker-compose up --build schoolapi
   ```
   (Migrations run automatically on startup)

## 🚢 Deployment

### Push images to Docker Hub

```bash
# Tag images
docker tag schoolapi:latest yourusername/schoolapi:latest
docker tag schoolapp:latest yourusername/schoolapp:latest

# Push to Docker Hub
docker push yourusername/schoolapi:latest
docker push yourusername/schoolapp:latest
```

### Deploy to cloud

The docker-compose.yml file can be used with:
- **Docker Swarm**
- **Kubernetes** (with kompose conversion)
- **Azure Container Instances**
- **AWS ECS**
- **Google Cloud Run**

## 📝 Files Added for Containerization

```
SchoolApi/
├── Dockerfile                   # Backend container build
├── .dockerignore               # Exclude build artifacts
├── docker-compose.yml          # Orchestration for all services
└── docker-entrypoint.sh        # Startup script (optional)

SchoolApp/
├── Dockerfile                   # Frontend container build
├── .dockerignore               # Exclude build artifacts
├── nginx.conf                  # Nginx web server config
└── wwwroot/
    ├── appsettings.json        # Local API URL
    └── appsettings.Production.json  # Docker API URL
```

## ✅ Validation Checklist

- [x] MySQL container starts and is healthy
- [x] Backend API connects to MySQL
- [x] EF migrations apply automatically
- [x] Seed data (5 classes, 20 students) loads
- [x] Backend API responds on port 5178
- [x] Frontend serves on port 8080
- [x] Frontend can call backend API
- [x] CORS configured correctly
- [x] Data loads in frontend UI
- [x] Persistent MySQL data in volume

## 🎯 Next Steps

- Add Docker health checks for all services
- Implement backup/restore scripts for MySQL volume
- Add Redis for caching
- Set up CI/CD pipeline for automated builds
- Configure production-ready nginx with SSL
- Add monitoring with Prometheus/Grafana

---

**Your entire stack is now containerized and portable! 🐳**

Run it anywhere Docker is available.
