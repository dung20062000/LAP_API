# LAP_API Setup Guide

## Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB, Express, or Full)
- (Optional) Visual Studio 2022 or VS Code with C# extension

---

## 1. Database Setup

### Option A: Using EF Core Migrations (Recommended)

```bash
cd LAP_API/LAP_API

# Install EF Core tools (if not installed)
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update
```

### Option B: Manual SQL Script

Run this SQL script in your SQL Server:

```sql
CREATE DATABASE LAP_DB;
GO

USE LAP_DB;
GO

---

## 2. Configure Connection String

Edit `LAP_API/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=LAP_DB;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  }
}
```

---

## 3. Run the API

```bash
cd LAP_API/LAP_API
dotnet run
```

The API will start at:
- HTTP: http://localhost:5055
- HTTPS: https://localhost:7202
- Swagger UI: http://localhost:5055/swagger


Also update `LAP_CLIENT/proxy.conf.json` to proxy `/api` requests to the backend:

```json
{
  "/api": {
    "target": "http://localhost:5055",
    "secure": false
  }
}
```

And add to `angular.json`:
```json
"options": {
  "proxyConfig": "proxy.conf.json"
}
```
