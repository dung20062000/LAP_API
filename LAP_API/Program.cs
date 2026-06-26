using System.Data;
using System.Text.Json.Serialization;
using LAP_API.Data;
using LAP_API.DTOs.Common;
using LAP_API.Middlewares;
using LAP_API.Repositories;
using LAP_API.Repositories.DriverRepo;
using LAP_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// HTTP Client Factory
builder.Services.AddHttpClient();

// Dapper IDbConnection — dùng SqlConnection (Scoped, mỗi request một instance)
builder.Services.AddScoped<IDbConnection>(_ =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository (EF Core — UnitOfWork)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repository (Dapper — Driver)
builder.Services.AddScoped<IDriverRepository, DriverRepository>();

// Services
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IUserVehicleGroupService, UserVehicleGroupService>();
builder.Services.AddScoped<IDriverService, DriverService>();

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // cấu hình phản hồi khi dữ liệu đầu vào không hợp lệ (ModelState invalid)
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = new Dictionary<string, string[]>();

            foreach (var (key, modelState) in context.ModelState)
            {
                var messages = modelState.Errors
                    .Select(e => e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .ToArray();

                if (messages.Length > 0)
                    errors[key] = messages;
            }

            var response = new ApiResponse
            {
                Success = false,
                Message = "Dữ liệu đầu vào không hợp lệ",
                Errors = errors,
                Timestamp = DateTime.UtcNow.ToString("o")
            };

            return new BadRequestObjectResult(response);
        };
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LAP API",
        Version = "v1",
        Description = "BA GPS Tracking System API",
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:4200" };
        policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();
app.Run();
