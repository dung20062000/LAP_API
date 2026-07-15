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
    /// <summary>
    /// Biển số xe cần tìm ảnh.
    /// </summary>
    [JsonPropertyName("vehiclePlate")]
    public string? VehiclePlate { get; set; }

    /// <summary>
    /// ID khách hàng (là trường XnCode của xe)
    /// </summary>
    [JsonPropertyName("customerId")]
    public int CustomerId { get; set; }

    /// <summary>
    /// Danh sách các kênh camera cần lấy ảnh.
    /// </summary>
    [JsonPropertyName("channels")]
    public List<int> Channels { get; set; } = new();

    /// <summary>
    /// Thời gian bắt đầu tìm kiếm.
    /// </summary>
    [Required(ErrorMessage = "StartTime là bắt buộc")]
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Thời gian kết thúc tìm kiếm.
    /// </summary>
    [Required(ErrorMessage = "EndTime là bắt buộc")]
    [JsonPropertyName("endTime")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Thứ tự sắp xếp (asc/desc theo thời gian).
    /// </summary>
    [JsonPropertyName("sortOrder")]
    public string SortOrder { get; set; } = "desc";

    /// <summary>
    /// Số trang hiện tại.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "PageNumber phải lớn hơn 0")]
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Số lượng bản ghi trên một trang.
    /// </summary>
    [Range(1, 100, ErrorMessage = "PageSize phải từ 1 đến 100")]
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 20;
}
