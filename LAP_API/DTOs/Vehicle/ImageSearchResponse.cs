using System.Text.Json.Serialization;

namespace LAP_API.DTOs.Vehicle;


/// <summary>
/// Kết quả trả về sau khi tìm kiếm ảnh, bao gồm phân trang.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
public class ImageSearchResponse
{
    /// Tổng số lượng ảnh tìm thấy.
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// Danh sách các ảnh trên trang hiện tại.
    [JsonPropertyName("items")]
    public List<ImageItemDto> Items { get; set; } = new();
}


/// <summary>
/// Chi tiết một mục ảnh.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
public class ImageItemDto
{
    /// Biển số xe.
    [JsonPropertyName("vehiclePlate")]
    public string VehiclePlate { get; set; } = string.Empty;

    /// Thời điểm chụp ảnh.
    [JsonPropertyName("imageTime")]
    public DateTime ImageTime { get; set; }

    /// Đường dẫn tải ảnh.
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    /// Vận tốc xe tại thời điểm đó (km/h).
    [JsonPropertyName("speed")]
    public int Speed { get; set; }

    /// Kênh camera.
    [JsonPropertyName("channel")]
    public int Channel { get; set; }

    /// Tên lái xe (nếu có).
    [JsonPropertyName("driverName")]
    public string DriverName { get; set; } = string.Empty;
}
