#!/bin/bash
set -e  # Exit on first error

# Color definitions
GREEN='\033[0;32m'
RED='\033[0;31m'
PURPLE='\033[0;35m'
YELLOW='\033[1;33m'
NC='\033[0m' 

# Load environment variables
if [ ! -f .env ]; then
  echo -e "${RED}[ERROR] .env file not found. Please create it based on .env.example.${NC}"
  exit 1
fi

set -a
source .env
set +a

echo -e "${PURPLE}[INFO] Starting Docker containers...${NC}"
docker-compose up -d

echo -e "${PURPLE}[INFO] Waiting for MySQL to be ready...${NC}"
until docker exec mysql_db_ballq mysqladmin ping -h "localhost" --silent; do
  echo -e "${YELLOW}  ...waiting for MySQL...${NC}"
  sleep 2
done

echo -e "${PURPLE}[INFO] Giving MySQL time to finish initializing...${NC}"
sleep 10

echo -e "${PURPLE}[INFO] Dropping existing database to start fresh...${NC}"
docker exec mysql_db_ballq mysql -u $MYSQL_USER -p$MYSQL_PASSWORD -e "DROP DATABASE IF EXISTS $MYSQL_DATABASE; CREATE DATABASE $MYSQL_DATABASE;"

echo -e "${PURPLE}[INFO] Removing existing migrations...${NC}"
rm -rf Infrastructure/Migrations/

echo -e "${PURPLE}[INFO] Building the solution...${NC}"
dotnet build Presentation

echo -e "${PURPLE}[INFO] Creating new InitialCreate migration...${NC}"
dotnet ef migrations add InitialCreate -p Infrastructure -s Presentation

echo -e "${PURPLE}[INFO] Applying EF Core migrations...${NC}"
dotnet ef database update -p Infrastructure -s Presentation

echo -e "${PURPLE}[INFO] Starting the application...${NC}"
dotnet run --no-build --project Presentation > logs.txt 2>&1 &
APP_PID=$!

echo -e "${YELLOW}[INFO] Waiting for application to start...${NC}"
sleep 15

echo -e "${PURPLE}[INFO] Seeding striker data...${NC}"
curl -X POST http://localhost:5286/api/load-data/load-strikers \
     -H "Content-Type: application/json" \
     -d '{}'

echo -e "${PURPLE}[INFO] Opening Swagger in browser...${NC}"
start http://localhost:5286/swagger

echo -e "${GREEN}[SUCCESS] Setup complete! The app is still running.${NC}"
wait $APP_PID
