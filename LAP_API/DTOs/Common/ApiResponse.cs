using System.Text.Json.Serialization;

namespace LAP_API.DTOs.Common;

/// <summary>
/// Wrapper phản hồi API chuẩn không có payload dữ liệu generic.
/// Dùng cho các phản hồi chỉ mang thông điệp (ví dụ: xác nhận hành động).
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Cho biết yêu cầu có thành công hay không.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Thông điệp mô tả kết quả.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Dữ liệu tùy chọn. Chỉ được đưa vào khi không null.
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; set; }

    /// <summary>
    /// Dictionary lỗi validation. Chỉ được đưa vào khi phản hồi
    /// mang thông tin lỗi theo từng trường.
    /// </summary>
    [JsonPropertyName("errors")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string[]>? Errors { get; set; }

    /// <summary>
    /// Timestamp ISO 8601 khi phản hồi được tạo (UTC).
    /// </summary>
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o");

    /// <summary>
    /// Tạo một phản hồi thành công.
    /// </summary>
    /// <param name="message">Thông điệp thành công.</param>
    /// <param name="data">Dữ liệu tùy chọn.</param>
    /// <returns>ApiResponse mới với Success = true.</returns>
    public static ApiResponse Ok(string message = "Thành công", object? data = null)
    {
        return new ApiResponse { Success = true, Message = message, Data = data };
    }

    /// <summary>
    /// Tạo một phản hồi lỗi.
    /// </summary>
    /// <param name="message">Thông điệp lỗi.</param>
    /// <param name="errors">Dictionary lỗi validation tùy chọn.</param>
    /// <returns>ApiResponse mới với Success = false.</returns>
    public static ApiResponse Fail(string message, Dictionary<string, string[]>? errors = null)
    {
        return new ApiResponse { Success = false, Message = message, Errors = errors };
    }
}

/// <summary>
/// Wrapper phản hồi API chuẩn có payload dữ liệu strongly-typed.
/// Tất cả API endpoint phải trả về wrapper này để client tiêu thụ nhất quán.
/// </summary>
/// <typeparam name="T">Loại dữ liệu payload.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Cho biết yêu cầu có thành công hay không.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Thông điệp mô tả kết quả.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Dữ liệu payload có kiểu. Chỉ được đưa vào khi không null
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }

    /// <summary>
    /// Dictionary lỗi validation. Chỉ được đưa vào khi phản hồi
    /// mang thông tin lỗi theo từng trường.
    /// </summary>
    [JsonPropertyName("errors")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string[]>? Errors { get; set; }

    /// <summary>
    /// Timestamp ISO 8601 khi phản hồi được tạo (UTC).
    /// </summary>
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o");

    /// <summary>
    /// Tạo một phản hồi thành công có dữ liệu.
    /// </summary>
    /// <param name="data">Dữ liệu payload.</param>
    /// <param name="message">Thông điệp thành công tùy chọn.</param>
    /// <returns>ApiResponse mới với Success = true.</returns>
    public static ApiResponse<T> Ok(T data, string message = "Thành công")
    {
        return new ApiResponse<T> { Success = true, Message = message, Data = data };
    }

    /// <summary>
    /// Tạo một phản hồi lỗi.
    /// </summary>
    /// <param name="message">Thông điệp lỗi.</param>
    /// <param name="errors">Dictionary lỗi validation tùy chọn.</param>
    /// <returns>ApiResponse mới với Success = false.</returns>
    public static ApiResponse<T> Fail(string message, Dictionary<string, string[]>? errors = null)
    {
        return new ApiResponse<T> { Success = false, Message = message, Errors = errors };
    }
}
