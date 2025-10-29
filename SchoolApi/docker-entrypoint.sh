#!/bin/bash
set -e

echo "Waiting for MySQL to be ready..."
until dotnet ef database update --no-build; do
  echo "Migration failed, retrying in 5 seconds..."
  sleep 5
done

echo "Database migrations completed successfully!"
exec dotnet SchoolApi.dll
