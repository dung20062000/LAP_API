# LAP_API — Agent System

## Tech Stack

- **ASP.NET Core 8** (Web API)
- **Entity Framework Core 8** + **Dapper**
- **SQL Server** via `Microsoft.Data.SqlClient`
- **C# 12** with nullable reference types

## Agent Rules

| File | Trigger | Scope |
|------|---------|-------|
| `csharp.mdc` | Always | All `.cs` files |

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
2. Read `.cursor/rules/csharp.mdc` for coding standards
3. Follow `concise-planning` skill when asked for a plan
4. Follow `backend-security-coder` skill when writing C# code
5. Follow `api-security-best-practices` skill for API patterns
6. Follow `security-auditor` skill for security audits

## Project Structure

```
LAP_API/
└── LAP_API/
    ├── Controllers/         # API controllers
    ├── Program.cs           # App entry point & DI setup
    └── WeatherForecast.cs   # Example model (replace with real models)
```

## Key Conventions

- **Allman** brace style (opening brace on new line)
- **PascalCase** for classes, methods, properties; **camelCase** for parameters
- Private fields: **_camelCase** with underscore prefix
- Interfaces: **I** + PascalCase (e.g., `IUserRepository`)
- Max line length: **120 characters**
- All async I/O: `async`/`await` — never `.Result` or `.Wait()`
- Error responses: generic messages — never expose exception details
