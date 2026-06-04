using LAP_API.DTOs.Common;
using LAP_API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LAP_API.Services;

/// <summary>
/// Class cơ sở cho tất cả service, cung cấp các chức năng chung.
/// Xử lý việc inject dependency và cung cấp các helper method
/// để tạo các API response chuẩn.
/// </summary>
public abstract class BaseService
{
    /// <summary>Instance Unit of Work cho thao tác truy cập dữ liệu.</summary>
    protected readonly IUnitOfWork _unitOfWork;

    /// <summary>Instance logger cho việc ghi log có cấu trúc.</summary>
    protected readonly ILogger _logger;

    /// <summary>
    /// Khởi tạo một instance mới của BaseService.
    /// </summary>
    /// <param name="unitOfWork">Instance Unit of Work.</param>
    /// <param name="logger">Instance logger.</param>
    protected BaseService(IUnitOfWork unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Tạo phản hồi HTTP 200 thành công kèm dữ liệu.
    /// </summary>
    /// <typeparam name="T">Loại dữ liệu payload.</typeparam>
    /// <param name="data">Dữ liệu trả về.</param>
    /// <param name="message">Thông điệp thành công tùy chọn.</param>
    /// <returns>OkObjectResult bọc ApiResponse.</returns>
    protected ActionResult OkResult<T>(T data, string message = "Thành công")
    {
        return new OkObjectResult(ApiResponse<T>.Ok(data, message));
    }

    /// <summary>
    /// Tạo phản hồi HTTP 200 thành công không có dữ liệu.
    /// </summary>
    /// <param name="message">Thông điệp thành công tùy chọn.</param>
    /// <returns>OkObjectResult bọc ApiResponse.</returns>
    protected ActionResult OkResult(string message = "Thành công")
    {
        return new OkObjectResult(ApiResponse.Ok(message));
    }

    /// <summary>
    /// Tạo phản hồi lỗi với mã HTTP được chỉ định.
    /// </summary>
    /// <typeparam name="T">Loại placeholder cho response.</typeparam>
    /// <param name="message">Thông điệp lỗi trả về.</param>
    /// <param name="statusCode">Mã HTTP (mặc định 400).</param>
    /// <returns>ObjectResult lỗi với mã HTTP tương ứng.</returns>
    protected ActionResult FailResult<T>(string message, int statusCode = 400)
    {
        return statusCode switch
        {
            400 => new BadRequestObjectResult(ApiResponse<T>.Fail(message)),
            401 => new UnauthorizedObjectResult(ApiResponse<T>.Fail(message)),
            404 => new NotFoundObjectResult(ApiResponse<T>.Fail(message)),
            _ => new ObjectResult(ApiResponse<T>.Fail(message)) { StatusCode = statusCode },
        };
    }

    /// <summary>
    /// Tạo phản hồi lỗi không có tham số generic.
    /// </summary>
    /// <param name="message">Thông điệp lỗi trả về.</param>
    /// <param name="statusCode">Mã HTTP (mặc định 400).</param>
    /// <returns>ObjectResult lỗi với mã HTTP tương ứng.</returns>
    protected ActionResult FailResult(string message, int statusCode = 400)
    {
        return statusCode switch
        {
            400 => new BadRequestObjectResult(ApiResponse.Fail(message)),
            401 => new UnauthorizedObjectResult(ApiResponse.Fail(message)),
            404 => new NotFoundObjectResult(ApiResponse.Fail(message)),
            _ => new ObjectResult(ApiResponse.Fail(message)) { StatusCode = statusCode },
        };
    }

    /// <summary>
    /// Tạo phản hồi HTTP 400 Bad Request kèm thông tin lỗi validation.
    /// </summary>
    /// <typeparam name="T">Loại placeholder cho response.</typeparam>
    /// <param name="message">Thông điệp lỗi.</param>
    /// <param name="errors">Dictionary chứa lỗi validation theo từng trường.</param>
    /// <returns>BadRequestObjectResult chứa chi tiết lỗi.</returns>
    protected ActionResult FailResult<T>(string message, Dictionary<string, string[]> errors)
    {
        return new BadRequestObjectResult(ApiResponse<T>.Fail(message, errors));
    }
}
