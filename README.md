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

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    Email NVARCHAR(100),
    FullName NVARCHAR(100),
    Role NVARCHAR(50) DEFAULT 'User',
    Avatar NVARCHAR(500),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);

CREATE TABLE Banners (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ImageUrl NVARCHAR(500) NOT NULL,
    TitleVi NVARCHAR(255) NOT NULL,
    TitleEn NVARCHAR(255),
    ShortContentVi NVARCHAR(500),
    ShortContentEn NVARCHAR(500),
    Link NVARCHAR(500),
    DisplayOrder INT DEFAULT 0,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);

CREATE TABLE Branches (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CityVi NVARCHAR(100) NOT NULL,
    CityEn NVARCHAR(100),
    Address NVARCHAR(500) NOT NULL,
    IsActive BIT DEFAULT 1,
    DisplayOrder INT DEFAULT 0,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);

-- Insert default admin user (password: admin@123)
INSERT INTO Users (Username, PasswordHash, Email, FullName, Role)
VALUES ('admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 'admin@bagps.com', 'Administrator', 'Admin');
```

> Password hash for `admin@123` = `jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=` (SHA256 base64)

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

---

## 4. API Endpoints

### Authentication
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/auth` | No | Login |
| POST | `/api/auth/logout` | Yes | Logout |
| POST | `/api/auth/refresh` | No | Refresh token |

### Banners
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| GET | `/api/banners` | No | Get all active banners |
| GET | `/api/banners/{id}` | No | Get banner by ID |
| POST | `/api/banners` | Yes | Create banner |
| PUT | `/api/banners/{id}` | Yes | Update banner |
| DELETE | `/api/banners/{id}` | Yes | Soft delete banner |

---

## 5. Update LAP_CLIENT to use API

In `LAP_CLIENT/src/app/services/auth.service.ts`, replace the mock login with:

```typescript
login(credentials: LoginRequest, rememberMe: boolean): Observable<ApiResponse<LoginResponse>> {
  return this.http.post<ApiResponse<LoginResponse>>(`${this.API_URL}`, credentials);
}
```

In `LAP_CLIENT/src/app/shared/services/banner.service.ts`, replace the mock with:

```typescript
readonly getBanners = (): Observable<BannerSlide[]> => {
  return this.http.get<BannerSlide[]>('/api/banners').pipe(
    map(res => res.success && res.data ? res.data : [])
  );
};
```

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
