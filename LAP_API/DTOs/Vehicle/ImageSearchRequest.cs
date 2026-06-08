using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LAP_API.DTOs.Vehicle;

public class ImageSearchRequest
{
    [JsonPropertyName("vehiclePlate")]
    public string? VehiclePlate { get; set; }

    [JsonPropertyName("customerId")]
    public int CustomerId { get; set; }

    [JsonPropertyName("channels")]
    public List<int> Channels { get; set; } = new();

    [Required(ErrorMessage = "StartTime là bắt buộc")]
    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "EndTime là bắt buộc")]
    [JsonPropertyName("endTime")]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("sortOrder")]
    public string SortOrder { get; set; } = "desc";

    [Range(1, int.MaxValue, ErrorMessage = "PageNumber phải lớn hơn 0")]
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; } = 1;

    [Range(1, 100, ErrorMessage = "PageSize phải từ 1 đến 100")]
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 20;
}
