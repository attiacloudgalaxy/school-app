#!/bin/bash

echo "🐳 School Management System - Docker Deployment"
echo "================================================"
echo ""

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker Desktop first."
    echo "   Download from: https://www.docker.com/products/docker-desktop"
    exit 1
fi

# Check if Docker is running
if ! docker info &> /dev/null; then
    echo "❌ Docker is not running. Please start Docker Desktop."
    exit 1
fi

echo "✅ Docker is installed and running"
echo ""

# Navigate to SchoolApi directory (where docker-compose.yml is)
cd "$(dirname "$0")/SchoolApi"

echo "🏗️  Building and starting containers..."
echo "   This may take a few minutes on first run..."
echo ""

# Build and start all services
docker-compose up --build -d

# Wait for services to be ready
echo ""
echo "⏳ Waiting for services to start..."
sleep 10

# Check service status
echo ""
echo "📊 Service Status:"
docker-compose ps

echo ""
echo "✅ Deployment complete!"
echo ""
echo "🌐 Access your application:"
echo "   Frontend:  http://localhost:8080"
echo "   API:       http://localhost:5178/swagger"
echo "   Health:    http://localhost:5178/health"
echo ""
echo "📝 Useful commands:"
echo "   View logs:      docker-compose -f SchoolApi/docker-compose.yml logs -f"
echo "   Stop services:  docker-compose -f SchoolApi/docker-compose.yml down"
echo "   Restart:        docker-compose -f SchoolApi/docker-compose.yml restart"
echo ""
echo "🎉 Happy coding!"
