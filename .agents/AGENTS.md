# LAP_API — Agent System

## Tech Stack

- **ASP.NET Core 8** (Web API)
- **Entity Framework Core 8** + **Dapper**
- **SQL Server** via `Microsoft.Data.SqlClient`
- **C# 12** with nullable reference types
- **Repository + Unit of Work** pattern
- **JWT Bearer** authentication

## Architecture

LAP_API tuân theo **3-tier layered architecture** (phân tầng):

```
Controller  →  Service  →  Repository  →  DbContext
(HTTP)        (Logic)      (Data)
```

**Luồng dữ liệu bắt buộc:**
```
HTTP Request
    ↓
[Controller]  ← validate, route, status code (KHÔNG viết logic nghiệp vụ)
    ↓
[Service]     ← business logic, mapping DTO, authorization (KHÔNG truy cập DbContext)
    ↓
[Repository]  ← data access only (EF Core / Dapper)
    ↓
[DbContext]   ← ONLY chỗ này truy cập database
```

**Nguyên tắc vàng:**
- Controller KHÔNG viết logic nghiệp vụ
- Service KHÔNG truy cập DbContext trực tiếp (phải qua Repository)
- Repository KHÔNG có logic nghiệp vụ (chỉ CRUD)
- DTO mapping ở Service layer, không ở Controller

## Folder Structure

```
LAP_API/
├── LAP_API/
│   ├── Controllers/              # HTTP entry points (inherit BaseApiController)
│   ├── Models/                   # EF Core entities (inherit BaseEntity)
│   ├── DTOs/                     # Request/Response objects
│   ├── Repositories/             # Data access layer
│   ├── Services/                 # Business logic layer
│   ├── Helpers/                   # Utility (JwtHelper, etc.)
│   ├── Middlewares/               # HTTP pipeline
│   ├── Data/                      # ApplicationDbContext
│   ├── Scripts/                   # SQL scripts
│   └── Program.cs                # DI, middleware registration
├── .cursor/rules/
│   └── api-development.mdc        # ⚠️ READ THIS FIRST
└── .agents/
    └── AGENTS.md
```

## Rule Files

| File | Scope | Purpose |
|------|-------|---------|
| `.cursor/rules/api-development.mdc` | **All .cs files** | Architecture, patterns, naming, security — **ĐỌC TRƯỚC KHI CODE** |

## Skills

| Skill | When to Use |
|-------|------------|
| `backend-security-coder` | Writing C# API endpoints, services, data access |
| `security-auditor` | Full security audit, compliance review |
| `api-security-best-practices` | API endpoint design, REST patterns |
| `idor-testing` | Testing access control vulnerabilities |
| `concise-planning` | "Make a plan", "How to approach" |

## Workflow

1. Read `AGENTS.md` for project context
2. **Read `.cursor/rules/api-development.mdc` FIRST** — contains architecture rules
3. Read `.agents/AGENTS.md` for additional context
4. Follow `concise-planning` skill when asked for a plan
5. Follow `backend-security-coder` skill when writing C# code
6. Follow `api-security-best-practices` skill for API patterns
7. Follow `security-auditor` skill for security audits

## Key Conventions

- **Braces**: Allman style (opening brace on new line)
- **Naming**: PascalCase (class/method/property), `_camelCase` (private field), `I` prefix (interface)
- **Line length**: Max 120 characters
- **Async**: All I/O must be `async`/`await` — never `.Result` or `.Wait()`
- **Response**: Always use `ApiResponse<T>` wrapper — never expose exception details
- **Soft delete**: Use `IsActive = false` — never hard delete unless required

## Adding a New Entity

When adding a new entity (e.g., `Product`), you MUST create ALL of these files in order:

1. `Models/Product.cs` — entity inheriting `BaseEntity`
2. `DTOs/Product/ProductDto.cs` — response DTO
3. `DTOs/Product/CreateProductRequest.cs` — request DTO
4. `DTOs/Product/UpdateProductRequest.cs` — request DTO
5. `Repositories/IProductRepository.cs` — interface inheriting `IGenericRepository<Product>`
6. `Repositories/ProductRepository.cs` — implementation
7. `Repositories/IUnitOfWork.cs` — add `IProductRepository Products { get; }`
8. `Repositories/UnitOfWork.cs` — initialize `Products = new ProductRepository(context)`
9. `Services/IProductService.cs` — interface
10. `Services/ProductService.cs` — implementation (use `IUnitOfWork`, NOT `DbContext`)
11. `Controllers/ProductsController.cs` — inherit `BaseApiController`
12. Register DI in `Program.cs` if needed
