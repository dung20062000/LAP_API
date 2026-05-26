---
name: backend-security-coder
description: Expert in secure .NET 8 backend coding. Use PROACTIVELY when writing ASP.NET Core API endpoints, services, or data access code.
risk: unknown
source: community
date_added: "2026-02-27"
---

# Backend Security Coder (.NET 8)

> Focus on **writing secure C# backend code** — not auditing. Use `security-auditor` for audits/threat modeling.

## Architecture

This project uses **Clean Architecture**:
- `LAP_API/LAP_API/` — Controllers, Program.cs
- `ApplicationDbContext` — EF Core context
- **Dapper** — for high-performance raw queries

## Naming Conventions (enforced by `.editorconfig`)

| Type | Convention | Example |
|------|------------|---------|
| Private field | `_camelCase` | `_logger`, `_context` |
| Interface | `I` + PascalCase | `IUserRepository` |
| Class / Method | PascalCase | `UserService.GetUser()` |

## Key Patterns

### Authorization (ASP.NET Core)

```csharp
// Policy-based in Program.cs
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthPolicies.ViewAllUsersPolicy,
        policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ViewUsers));

// Use [Authorize] attribute on controller actions
[Authorize(Policy = AuthPolicies.ManageAllUsersPolicy)]
public async Task<IActionResult> DeleteUser(string id) { ... }
```

### Current User Access

```csharp
// In BaseApiController (if available)
protected string GetCurrentUserId(string errorMsg = "Error retrieving userId")
{
    return Utilities.GetUserId(User) ?? throw new UserNotFoundException(errorMsg);
}

// In services (via IUserIdAccessor)
public class MyService
{
    private readonly IUserIdAccessor _userIdAccessor;
    public async Task DoSomething()
    {
        var userId = _userIdAccessor.GetCurrentUserId();
    }
}
```

### EF Core — Prevent SQL Injection

```csharp
// ✅ Safe — always use parameterized queries via LINQ
var customer = await _context.Customers.FindAsync(id);

// ✅ Safe — parameterized raw query
await _context.Database.ExecuteSqlRawAsync(
    "UPDATE Customers SET Name = {0} WHERE Id = {1}", name, id);

// ❌ Never concatenate user input into raw SQL
await _context.Database.ExecuteSqlRawAsync($"UPDATE Customers SET Name = '{name}'");
```

### FluentValidation — Input Validation

```csharp
// DTO must implement FluentValidation.AbstractValidator<T>
public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).EmailAddress();
    }
}

// Program.cs — register
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
```

### Audit Trail (IAuditableEntity)

```csharp
// Implement IAuditableEntity for auto audit fields
public class Customer : BaseEntity, IAuditableEntity
{
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
}
```

### Error Handling — No Data Leaks

```csharp
catch (Exception ex)
{
    _logger.LogError(ex, "Operation failed");
    // Return generic message — never expose ex.Message or stack trace
    return StatusCode(500, new { message = "An error occurred. Please try again." });
}
```

### CORS Configuration

```csharp
// Program.cs — be strict in production
app.UseCors(builder => builder
    .AllowAnyOrigin()  // ❌ Only for dev
    .AllowAnyHeader()
    .AllowAnyMethod());

// Production: specify exact origins
    .WithOrigins("https://yourdomain.com")
```

### Rate Limiting (.NET 8)

```csharp
// Add RateLimiter middleware in Program.cs
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("strict", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(15)
            }));
});
```

## Checklist

- [ ] All API endpoints require `[Authorize]` or `[AllowAnonymous]`
- [ ] Policy-based authorization for sensitive operations
- [ ] All user inputs validated with FluentValidation
- [ ] No raw SQL with string concatenation — use LINQ or parameterized queries
- [ ] Audit fields (`IAuditableEntity`) set on new entities
- [ ] Current user ID captured via `IUserIdAccessor` or `GetCurrentUserId()`
- [ ] Error responses do not leak internal details
- [ ] CORS restricted to known origins (not `AllowAnyOrigin`)
- [ ] No secrets or credentials in code — use `appsettings.json` or env vars

## When to Use

- Writing new API endpoints in `Controllers/`
- Adding service logic
- Creating new models or DTOs
- Configuring authorization policies
- Adding FluentValidation validators
- Reviewing backend code for security issues
