using System.Text.Json.Serialization;

namespace LAP_API.DTOs.Vehicle;

/// <summary>
/// DTO chứa thông tin cơ bản của xe để hiển thị trong danh sách.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
public class VehicleDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("vehiclePlate")]
    public string VehiclePlate { get; set; } = string.Empty;

    /// Mã xí nghiệp.
    [JsonPropertyName("XNCode")]
    public int XNCode { get; set; }

    /// Tên hiển thị 
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

}
