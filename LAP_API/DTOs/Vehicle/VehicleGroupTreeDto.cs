using System.Text.Json.Serialization;

namespace LAP_API.DTOs.Vehicle;


/// <summary>
/// DTO đại diện cho một nút trong cây nhóm xe (PrimeNG TreeNode).
/// </summary>
/// <Modified>
/// Name Date Comments
/// dungbt 6/9/2026 created
/// </Modified>
public class VehicleGroupTreeDto
{
    /// <summary>
    /// ID của nhóm dưới dạng chuỗi.
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    /// <summary>
    ///  Nhãn hiển thị trên cây (bao gồm tên nhóm và số lượng xe).
    /// </summary>
    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Dữ liệu bổ sung đi kèm (thường là ID nhóm).
    /// </summary>
    [JsonPropertyName("data")]
    public string Data { get; set; } = string.Empty;

    /// <summary>
    /// Danh sách các nhóm con.
    /// </summary>
    [JsonPropertyName("children")]
    public List<VehicleGroupTreeDto> Children { get; set; } = new();
}
