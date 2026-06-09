using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LAP_API.DTOs.Vehicle;

/// <summary>
/// Request chứa các tham số để tìm kiếm ảnh lịch sử của xe.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
public class ImageSearchRequest
{
    /// Biển số xe cần tìm ảnh.
    [JsonPropertyName("vehiclePlate")]
    public string? VehiclePlate { get; set; }

    /// ID khách hàng (là trường XnCode của xe)
    [JsonPropertyName("customerId")]
    public int CustomerId { get; set; }

    /// Danh sách các kênh camera cần lấy ảnh.
    [JsonPropertyName("channels")]
    public List<int> Channels { get; set; } = new();

    /// Thời gian bắt đầu tìm kiếm.
    [Required(ErrorMessage = "StartTime là bắt buộc")]
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }

    /// Thời gian kết thúc tìm kiếm.
    [Required(ErrorMessage = "EndTime là bắt buộc")]
    [JsonPropertyName("endTime")]
    public DateTime EndTime { get; set; }

    /// Thứ tự sắp xếp (asc/desc theo thời gian).
    [JsonPropertyName("sortOrder")]
    public string SortOrder { get; set; } = "desc";

    /// Số trang hiện tại.
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber phải lớn hơn 0")]
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; } = 1;

    /// Số lượng bản ghi trên một trang.
    [Range(1, 100, ErrorMessage = "PageSize phải từ 1 đến 100")]
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 20;
}
