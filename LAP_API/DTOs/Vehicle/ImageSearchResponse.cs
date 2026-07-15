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
    /// <summary>
    /// Tổng số lượng ảnh tìm thấy.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Danh sách các ảnh trên trang hiện tại.
    /// </summary>
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
    /// <summary>
    /// Biển số xe.
    /// </summary>
    [JsonPropertyName("vehiclePlate")]
    public string VehiclePlate { get; set; } = string.Empty;

    /// <summary>
    /// Thời điểm chụp ảnh.
    /// </summary>
    [JsonPropertyName("imageTime")]
    public DateTime ImageTime { get; set; }

    /// <summary> 
    /// Đường dẫn tải ảnh.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Vận tốc xe tại thời điểm đó (km/h).
    /// </summary>
    [JsonPropertyName("speed")]
    public int Speed { get; set; }

    /// <summary>
    /// Kênh camera.
    /// </summary>
    [JsonPropertyName("channel")]
    public int Channel { get; set; }

    /// <summary>
    /// Tên lái xe (nếu có).
    /// </summary>
    [JsonPropertyName("driverName")]
    public string DriverName { get; set; } = string.Empty;
}
