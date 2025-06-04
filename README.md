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

- ✅ Player creation and stat registration (Strikers module) --- (currently developing)
- ✅ Advanced stat fields: goals, assists, xG, key passes, shot accuracy, and more --- (currently developing)
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


---


