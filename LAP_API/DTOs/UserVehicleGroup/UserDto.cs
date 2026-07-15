using System.Text.Json.Serialization;

namespace LAP_API.DTOs.UserVehicleGroup;

/// <summary>
/// DTO trả về thông tin người dùng trong danh sách.
/// </summary>
/// <Modified>
/// Name     Date         Comments
/// dungbt   6/11/2026    created
/// </Modified>
public class UserDto
{
    /// <summary>
    /// ID người dùng (GUID).
    /// </summary>
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }

    /// <summary>
    /// Tên đăng nhập.
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Tên hiển thị đầy đủ.
    /// </summary>
    [JsonPropertyName("fullname")]
    public string Fullname { get; set; } = string.Empty;
}
