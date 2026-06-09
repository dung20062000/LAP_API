using System.Net;
using System.Text.Json;
using LAP_API.DTOs.Common;

namespace LAP_API.Middlewares;

/// <summary>
/// Middleware xử lý ngoại lệ tập trung cho toàn bộ ứng dụng.
/// Giúp bắt các lỗi chưa được xử lý và trả về phản hồi chuẩn hóa cho phía Client.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Thực thi middleware để xử lý request và bắt lỗi nếu có.
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log chi tiết lỗi vào hệ thống log
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            // Trả về phản hồi lỗi chuẩn cho client
            await HandleExceptionAsync(context);
        }
    }

    /// <summary>
    /// Định dạng và ghi phản hồi lỗi JSON cho client khi có exception xảy ra.
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// dungbt 6/9/2026 created
    /// </Modified>
    private static async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = ApiResponse.Fail("Đã xảy ra lỗi phía máy chủ. Vui lòng thử lại sau.");
        var options = new JsonSerializerOptions { PropertyNamingPolicy = null };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
