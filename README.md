## ⚙️ Initial Setup Instructions

To get started with **BallIQ**, you'll need to do an initial one-time setup that prepares the database, seeds data, and launches the application.

### 🧾 1. Create Your `.env` File

Before anything else, create a `.env` file in the root of the project. You can use the provided `.env.example` as a reference for all required variables (e.g., database credentials, connection strings, etc.).

```bash
cp .env.example .env
```

---

### 🛠️ 2. Run the Initialization Script (First-Time Only)

Run the following command from your terminal (**Git Bash** or **WSL** recommended):

```bash
bash setup.sh
```

This script will:

- 🔄 Drop and recreate the MySQL database from scratch
- 🧹 Remove any existing EF Core migrations
- 🏗️ Rebuild the solution
- ⚙️ Apply fresh migrations
- 📥 Insert striker data from `processed_strikers.sql`
- 🌐 Automatically start the ASP.NET Core app
- 🚀 Open Swagger in your browser

> ✅ This script is meant to be used **only the first time** you want to initialize the system or when you need a **complete reset** of the environment.

---

### 📌 3. What Happens If You Re-run `setup.sh`?

If you run `setup.sh` again:

- The `Migrations/` folder will be deleted
- The MySQL database will be dropped and recreated
- Existing data will be lost
- The application will reseed fresh striker data

So if you **want data persistence**, do **not** run `setup.sh` again.  
Instead, simply do:

```bash
docker-compose up -d
```

Then run the application from your IDE (e.g., Visual Studio, Rider, or VS Code) to continue working with your existing data.

---

### 📨 4. About the `/api/load-data/load-strikers` Endpoint

The `/api/load-data/load-strikers` endpoint is designed to remain available in the application:

- ✅ You can call it **multiple times** to simulate a large number of striker records
- 📦 Useful during development or testing before bulk datasets are finalized
- 🧪 Helps validate filtering, performance, and pagination features with large data volumes

> When the application reaches its final version, this endpoint will no longer be critical — because the system is intended to work with **3,000+ players** across all positions — but for now, it's a handy development tool.

---

### 🔁 Alternative Seeding: Fake Data Generator

You can now seed the system without relying on a static `.sql` script.

The `/api/load-data/load-fake-strikers` endpoint leverages the `FakeStrikerSeederService`, which:

- 🧠 Dynamically generates striker profiles using realistic stat modeling
- 🎯 Simulates performance tiers (low → world-class) with position-specific behaviors
- 💵 Estimates market value based on performance-driven heuristics

This approach is especially useful when:

- You want varied or large volumes of test data
- You don’t want to maintain or rely on static seed files
- You're stress-testing sorting, filtering, and pagination performance

> Note: This service is implemented in `FakeStrikerSeederService.cs` and integrated via dependency injection. You can configure the quantity of fake strikers by passing a `quantity` parameter in the request.

Example:

```bash
curl -X POST "http://localhost:5286/api/load-data/load-fake-strikers?quantity=500"
```


# ⚽ BallIQ – Advanced Football Player Analytics Platform

**BallIQ** is a robust football player analytics system currently under active development. It offers a scalable backend built using Clean Architecture principles and will provide detailed statistics, player comparison features, and performance insights through a clean, responsive user interface.

---

## 🚀 Project Overview

BallIQ aims to become a comprehensive analytics dashboard where users can:

- 🔍 View advanced player stats (e.g., goals per 90, shot accuracy, assists)
- 📊 Compare multiple players side by side
- 🧠 Evaluate performance through custom metrics
- 📈 Track player development across seasons and competitions
- 📂 Upload datasets and fetch player insights in real time

---

## 🧱 Architecture

This project is being built following **Clean Architecture** principles to ensure separation of concerns, testability, and scalability. The solution is split across multiple projects:

- **Domain** – Core business entities and logic
- **Application** – Use cases and CQRS pattern with MediatR
- **Infrastructure** – Data access layer and external service implementations
- **Presentation** – ASP.NET Core Web API as the backend entry point

---

## 🛠️ Technologies Used

| Layer           | Technologies |
|----------------|--------------|
| Backend         | ASP.NET Core 9, MediatR, Entity Framework Core, AutoMapper |
| Architecture    | Clean Architecture (Domain → Application → Infrastructure → Presentation) |
| Database        | MySQL |
| API Testing     | Swagger (OpenAPI) |
| Data Loading    | DotNetEnv for `.env` file handling |
| Logging & Middleware | Built-in ASP.NET Core logging |
| Dev Tools       | JetBrains Rider / Visual Studio, Postman |

---

## 📦 Current Features (Backend)

- ✅ Player creation and stat registration (Strikers module) — *(currently developing)*
- ✅ Advanced stat fields: goals, assists, xG, key passes, shot accuracy, and more — *(currently developing)*
- ✅ Environment-based database config (via `.env`)
- ✅ API endpoint structure with RESTful design
- ✅ Swagger UI for exploring endpoints
- 🛠️ DataLoader repository for bulk CSV/stat ingestion

---

## 🧩 Features Coming Soon

- 🧠 Scoring and rating algorithm (per role: striker, midfielder, defender)
- 📊 Player comparison endpoints
- 📈 Aggregate statistics and insights
- ⚙️ Admin panel & management endpoints

---

## 🎨 Frontend Roadmap (Other Repository when started)

The frontend will be built immediately after the backend MVP is complete.

### 🧰 Planned Stack

- **Frontend Framework**: Angular 19 or 20 (latest stable version at development time)
- **Styling**: SCSS for modular, maintainable styles
- **Data Visualization**: Recharts or ngx-charts (depending on compatibility)
- **Routing & State Management**: Angular Router + RxJS and/or NgRx (for reactive state)

### 🔧 Dashboard Features

- 📊 **Stat dashboards** to visualize performance metrics
- 🔍 **Player comparison interface** for side-by-side evaluations
- 🧩 **Search and filtering** by club, nationality, age, and more
- 📁 **Responsive UI** designed for analysts, scouts, and coaches