using System.Text.Json.Serialization;

namespace LAP_API.DTOs.Vehicle;

/// <summary>
/// Dùng để nhận dữ liệu phản hồi từ API bên thứ 3 cung cấp ảnh xe rồi map vào DTO này để trả về cho frontend. 
/// Vì API bên thứ 3 có thể trả về dữ liệu với các trường khác nhau (data, Data, items, Items), 
/// nên DTO này được thiết kế để linh hoạt nhận tất cả các trường đó và cung cấp phương thức GetItems() để lấy danh sách ảnh một cách nhất quán.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/23/2026 created
/// </Modified>
public class ExternalImageApiResponse
{
    [JsonPropertyName("data")]
    public List<ExternalImageItem> Data { get; set; } = new();

    [JsonPropertyName("items")]
    public List<ExternalImageItem> Items { get; set; } = new();

    public List<ExternalImageItem> GetItems()
    {
        if (Data != null && Data.Any()) return Data;
        if (Items != null && Items.Any()) return Items;
        return new List<ExternalImageItem>();
    }
}

/// <summary>
/// DTO đại diện cho một ảnh xe nhận được từ API bên thứ 3.
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/23/2026 created
/// </Modified>
public class ExternalImageItem
{
    [JsonPropertyName("v")]
    public string VehiclePlate { get; set; } = string.Empty;

    [JsonPropertyName("c")]
    public DateTime ImageTime { get; set; }

    [JsonPropertyName("u")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("s")]
    public int Speed { get; set; }

    [JsonPropertyName("k")]
    public int Channel { get; set; }

    [JsonPropertyName("n")]
    public string DriverName { get; set; } = string.Empty;
}