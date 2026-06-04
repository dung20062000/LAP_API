using LAP_API.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace LAP_API.Controllers;

/// <summary>
/// Class cơ sở cho tất cả controller, cung cấp các helper method
/// tạo response chuẩn hóa cho toàn bộ API.
/// </summary>
public abstract class BaseApiController : ControllerBase
{
    /// <summary>
    /// Tạo phản hồi HTTP 200 thành công kèm dữ liệu có kiểu.
    /// </summary>
    /// <typeparam name="T">Loại dữ liệu phản hồi.</typeparam>
    /// <param name="data">Dữ liệu trả về.</param>
    /// <param name="message">Thông điệp thành công tùy chọn.</param>
    /// <returns>OkResult bọc ApiResponse.</returns>
    protected ActionResult OkResponse<T>(T data, string message = "Thành công")
    {
        return Ok(ApiResponse<T>.Ok(data, message));
    }

    /// <summary>
    /// Tạo phản hồi HTTP 200 thành công không có dữ liệu.
    /// </summary>
    /// <param name="message">Thông điệp thành công tùy chọn.</param>
    /// <returns>OkResult bọc ApiResponse.</returns>
    protected ActionResult OkResponse(string message = "Thành công")
    {
        return Ok(ApiResponse.Ok(message));
    }

    /// <summary>
    /// Tạo phản hồi lỗi với mã HTTP được chỉ định.
    /// </summary>
    /// <typeparam name="T">Loại placeholder cho phản hồi.</typeparam>
    /// <param name="message">Thông điệp lỗi trả về.</param>
    /// <param name="statusCode">Mã HTTP (mặc định 400).</param>
    /// <returns>Phản hồi lỗi với mã HTTP tương ứng.</returns>
    protected ActionResult FailResponse<T>(string message, int statusCode = 400)
    {
        return statusCode switch
        {
            400 => BadRequest(ApiResponse<T>.Fail(message)),
            401 => Unauthorized(ApiResponse<T>.Fail(message)),
            403 => StatusCode(403, ApiResponse<T>.Fail(message)),
            404 => NotFound(ApiResponse<T>.Fail(message)),
            409 => Conflict(ApiResponse<T>.Fail(message)),
            _ => StatusCode(statusCode, ApiResponse<T>.Fail(message)),
        };
    }

    /// <summary>
    /// Tạo phản hồi lỗi không có tham số generic.
    /// </summary>
    /// <param name="message">Thông điệp lỗi trả về.</param>
    /// <param name="statusCode">Mã HTTP (mặc định 400).</param>
    /// <returns>Phản hồi lỗi với mã HTTP tương ứng.</returns>
    protected ActionResult FailResponse(string message, int statusCode = 400)
    {
        return statusCode switch
        {
            400 => BadRequest(ApiResponse.Fail(message)),
            401 => Unauthorized(ApiResponse.Fail(message)),
            403 => StatusCode(403, ApiResponse.Fail(message)),
            404 => NotFound(ApiResponse.Fail(message)),
            409 => Conflict(ApiResponse.Fail(message)),
            _ => StatusCode(statusCode, ApiResponse.Fail(message)),
        };
    }

    /// <summary>
    /// Tạo phản hồi HTTP 400 Bad Request kèm thông tin lỗi validation.
    /// </summary>
    /// <typeparam name="T">Loại placeholder cho phản hồi.</typeparam>
    /// <param name="message">Thông điệp lỗi.</param>
    /// <param name="errors">Dictionary chứa lỗi validation theo từng trường.</param>
    /// <returns>BadRequestResult chứa chi tiết lỗi.</returns>
    protected ActionResult FailResponse<T>(string message, Dictionary<string, string[]> errors)
    {
        return BadRequest(ApiResponse<T>.Fail(message, errors));
    }

    /// <summary>
    /// Tạo phản hồi HTTP 201 Created sau khi tạo resource thành công.
    /// </summary>
    /// <typeparam name="T">Loại resource được tạo.</typeparam>
    /// <param name="data">Dữ liệu resource vừa tạo.</param>
    /// <param name="message">Thông điệp thành công tùy chọn.</param>
    /// <returns>StatusCode 201 bọc ApiResponse.</returns>
    protected ActionResult CreatedResponse<T>(T data, string message = "Tạo thành công")
    {
        return StatusCode(201, ApiResponse<T>.Ok(data, message));
    }
}
