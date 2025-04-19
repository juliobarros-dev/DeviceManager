#!/bin/bash
set -euo pipefail

cd "$(dirname "$0")"

echo "🟡 Starting database container..."

cd Docker/Local/Infrastructure/Database

if ! docker network ls --format '{{.Name}}' | grep -qw deviceManager-net; then
  echo "🔧 Creating Docker network 'deviceManager-net'..."
  docker network create deviceManager-net
else
  echo "✅ Docker network 'deviceManager-net' already exists. Skipping creation."
fi

docker-compose -p device_manager_postgres up -d --build

echo "⏳ Waiting for PostgreSQL to accept connections..."
until docker exec device_manager_postgres pg_isready -U postgres > /dev/null 2>&1; do
    sleep 1
done

sleep 5

echo "🟢 PostgreSQL is ready!"

echo "🚀 Checking if the 'DeviceManager' database already exists..."
if docker exec -i device_manager_postgres psql -U postgres -d postgres -tAc "SELECT 1 FROM pg_database WHERE datname = 'postgres';" | grep -qw 1; then
  echo "✅ Database 'postgres' already exists. Skipping creation."
else
  echo "🚧 Database does not exist yet. Creating..."
  docker exec -i device_manager_postgres psql -U postgres -d postgres -c "CREATE DATABASE postgres;"
  echo "✅ Database 'postgres' created successfully."
fi

echo "🟡 Starting service container..."

cd ../../Application

docker-compose -p device_manager_api up -d --build

echo "✅ All set! Access: http://localhost:7019/"
