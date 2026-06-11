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
    /// ID người dùng (GUID).
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }

    /// Tên đăng nhập.
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    /// Tên hiển thị đầy đủ.
    [JsonPropertyName("fullname")]
    public string Fullname { get; set; } = string.Empty;

    /// Email người dùng.
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// Loại người dùng (0=Normal, 1=Admin).
    [JsonPropertyName("userType")]
    public byte UserType { get; set; }
}
